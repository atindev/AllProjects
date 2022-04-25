using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using Microsoft.EntityFrameworkCore;
using WAS.Application.Interface.Services;
using Entity = WAS.Domain.Entities;
using WAS.Application.Common.Models;
using getAudience = WAS.Application.Features.Survey.GetUniqueSubscribers;
using submitted = WAS.Application.Features.Survey.GetAllSubmitted;
using Newtonsoft.Json;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Survey.GetAnswerwiseReport
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMediator _mediator;
        private readonly IAzureStorageService _azureStorageService;
        private readonly ITimeParser _timeParser;

        public Handler(
            IWasContext context,
            IMediator mediator,
            ILogger<Handler> logger,
            IAzureStorageService azureStorageService,
            ITimeParser timeParser
            )
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _azureStorageService = azureStorageService;
            _timeParser = timeParser;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                 var finalResponse = new Response();

                //get the survey content
                var survey = await _context.SurveyBroadcasts
                  .IgnoreQueryFilters()
                  .SingleOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

                if (survey != null)
                {
                    int elapsedAverageTime = 0;
                    var content = await _azureStorageService.DowloadTemplatejsonFromBlobStorage(survey.Path);
                    var SurveyContent = JsonConvert.DeserializeObject<CreateSurvey>(content);

                    var optionsQuestions=SurveyContent.Questions.Where(x=>x.QuestionType != ((int)SurveyQuestionTypes.Short_Answer).ToString());
                  
                    var submittedAudience = await _mediator.Send(new submitted.Request { Id = request.Id, IsOnlySubscriberIds = false });

                    var SubmittedCount = submittedAudience.Answers.Count;

                    var submittedAnswers = submittedAudience.Answers.SelectMany(x=>x.Answers).ToList();

                    finalResponse.Answers = new List<SurveyAnswer>();
                    finalResponse.SubmittedCount = SubmittedCount;
                    finalResponse.isWizard = survey.IsWizard ?? false;

                    //loop through each questions
                    foreach (var question in optionsQuestions)
                    {
                        var answerOptions = new List<AnswerOption>();
                        
                        var currentQuestionResponse = submittedAnswers.Where(x=>x.QuestionNumber==question.QuestionNumber).ToList();

                        elapsedAverageTime = 0;
                        if((survey.IsWizard??false) && currentQuestionResponse!=null && currentQuestionResponse.Any())
                            elapsedAverageTime = (int)currentQuestionResponse.Average(x => x.ElapsedTime);

                        GetAnswerOptions(question, currentQuestionResponse, SubmittedCount, answerOptions);

                        SurveyAnswer SurveyAnswer = new SurveyAnswer
                        {
                            Question = question.Question,
                            QuestionType = question.QuestionType,
                            QuestionNumber = question.QuestionNumber,
                            AnswerOptions = answerOptions,
                            ElapsedPercentage = _timeParser.GetTime(elapsedAverageTime),
                            OtherOptions=currentQuestionResponse.Where(quesRes=>!string.IsNullOrEmpty(quesRes.OtherOption)).Select(res=>res.OtherOption)
                        };

                        finalResponse.Answers.Add(SurveyAnswer);
                    }
                }

                return finalResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }

        private void GetAnswerOptions(SurveyQuestions question,List<Answers> currentQuestionResponse,int submittedCount, List<AnswerOption> answerOptions)
        {
            var colorPalettes = new string[] { "#2f4b7c", "#665191", "#f95d6a", "#ff7c43", "#ffa600", "#005595", "#007EB4", "#00A5B6", "#00C7A0", "#8BE481", "#F9F871", "#2DB268", "#9EB0A2", "#f95d6a", "#ff7c43", "#ffa600" };

            if (question.QuestionType == ((int)SurveyQuestionTypes.Rating).ToString() && question.RatingType == (int)SurveyRatingTypes.Slider)
            {
                for (int i = 1; i <= Convert.ToInt32(question.Options[0].Text); i++)
                { 
                    var currentOption = new AnswerOption();
                    decimal responseCount = currentQuestionResponse.Count(x => x.Answer.Contains(i.ToString()));

                    decimal responsePercentage = 0;
                    responsePercentage = submittedCount > 0 ? Math.Round((responseCount / submittedCount) * 100, 2) : 0;

                    currentOption.Text = i.ToString();
                    currentOption.SelectionPercentage = responsePercentage;
                    currentOption.PercentageText = responsePercentage + "%";
                    currentOption.Color = colorPalettes[i-1];
                    currentOption.ResponseCount = (int)responseCount;
                    answerOptions.Add(currentOption);
                }
            }
            else
            {
                var currentIndex = 0;
                //loop through each answers
                foreach (var options in question.Options)
                {
                    var currentOption = new AnswerOption();
                    decimal responseCount = currentQuestionResponse.Count(x => x.Answer.Contains(options.Id.ToString()));

                    decimal responsePercentage = 0;
                    responsePercentage = submittedCount > 0 ? Math.Round((responseCount / submittedCount) * 100, 2) : 0;

                    currentOption.Text = options.Text;
                    currentOption.SelectionPercentage = responsePercentage;
                    currentOption.PercentageText = responsePercentage + "%";
                    currentOption.Color = colorPalettes[currentIndex];
                    currentOption.ResponseCount = (int)responseCount;
                    answerOptions.Add(currentOption);

                    ++currentIndex;
                }
            }

            GetOtherAnswerOptions(question, currentQuestionResponse, submittedCount, answerOptions);
        }

        private void GetOtherAnswerOptions(SurveyQuestions question, List<Answers> currentQuestionResponse, int submittedCount, List<AnswerOption> answerOptions)
        {
            if (question.IsOtherOptionEnabled)
            {
                var currentOption = new AnswerOption();
                decimal otherOptionSubmittedCount = currentQuestionResponse.Count(x => !string.IsNullOrEmpty(x.OtherOption));

                decimal responsePercentage = 0;
                responsePercentage = submittedCount > 0 ? Math.Round((otherOptionSubmittedCount / submittedCount) * 100, 2) : 0;

                currentOption.Text = "others";
                currentOption.SelectionPercentage = responsePercentage;
                currentOption.PercentageText = responsePercentage + "%";
                currentOption.Color = "#005696";
                currentOption.ResponseCount = (int)otherOptionSubmittedCount;
                answerOptions.Add(currentOption);
            }
        }
    }
}

