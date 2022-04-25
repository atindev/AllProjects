using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface.Services;
using Microsoft.Azure.Cosmos;
using WAS.Application.Common.Models;
using System.Linq;
using Azure;
using Azure.AI.TextAnalytics;
using WAS.Application.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WAS.Domain.Enum;
using submitted = WAS.Application.Features.Survey.GetAllSubmitted;

namespace WAS.Application.Features.Survey.ExtractKeyPhrasesFromShortAnswer
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly ITextAnalyticsProvider _analyticsProvider;
        private readonly IWasContext _context;
        private readonly IMediator _mediator;
        private readonly IAzureStorageService _azureStorageService;
        private readonly ITimeParser _timeParser;

        public Handler(
            ILogger<Handler> logger,
            ITextAnalyticsProvider analyticsProvider,
            IWasContext context,
            IMediator mediator,
            IAzureStorageService azureStorageService,
            ITimeParser timeParser
            )
        {
            _logger = logger;
            _analyticsProvider = analyticsProvider;
            _context = context;
            _azureStorageService = azureStorageService;
            _mediator = mediator;
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

                    var analyticsClient = _analyticsProvider.GetAITextAnalyticsClientProvider();

                    var content = await _azureStorageService.DowloadTemplatejsonFromBlobStorage(survey.Path);
                    
                    var SurveyContent = JsonConvert.DeserializeObject<CreateSurvey>(content);

                    var optionsQuestions = SurveyContent.Questions.Where(x => x.QuestionType == ((int)SurveyQuestionTypes.Short_Answer).ToString());

                    var submittedAudience = await _mediator.Send(new submitted.Request { Id = request.Id, IsOnlySubscriberIds = false });

                    var SubmittedCount = submittedAudience.Answers.Count;

                    var submittedAnswers = submittedAudience.Answers.SelectMany(x => x.Answers).Where(x=>x.QuestionType== ((int)SurveyQuestionTypes.Short_Answer).ToString()).ToList();

                    finalResponse.SubmittedCount = SubmittedCount;
                    finalResponse.isWizard = survey.IsWizard ?? false;

                    //loop through each questions
                    foreach (var question in optionsQuestions)
                    {
                        var answers = new List<string>();
                        var keyPhraseResponseCount = new List<KeyPhrasesCount>();
                        var analysisMainReport = new List<AnswerOption>();

                        var currentQuestionResponse = submittedAnswers.Where(x => x.QuestionNumber == question.QuestionNumber).ToList();
                        answers = currentQuestionResponse.SelectMany(x => x.Answer).ToList();

                        elapsedAverageTime = 0;
                        if ((survey.IsWizard ?? false) && currentQuestionResponse != null && currentQuestionResponse.Any())
                            elapsedAverageTime = (int)currentQuestionResponse.Average(x => x.ElapsedTime);

                        keyPhraseResponseCount = await getKeyPhraseList(answers, analyticsClient);
                        analysisMainReport = await getSentimentList(answers, analyticsClient, SubmittedCount);

                        AnalysisReport AnalysisReport = new AnalysisReport
                        {
                            Question = question.Question,
                            QuestionType = question.QuestionType,
                            QuestionNumber = question.QuestionNumber,
                            KeyPhrasesCounts = keyPhraseResponseCount,
                            SentimentsDetails = analysisMainReport,
                            ElapsedPercentage = _timeParser.GetTime(elapsedAverageTime)
                        };

                        finalResponse.AnalysisReports.Add(AnalysisReport);
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


        private async Task<List<KeyPhrasesCount>> getKeyPhraseList(List<string> answers, Azure.AI.TextAnalytics.TextAnalyticsClient analyticsClient)
        {
            var keyPhraseResponseCount = new List<KeyPhrasesCount>();
            var totalkeyPhraseResponse = new List<string>();

            if (answers.Any())
            {
                int skip = 0, take = 10;

                while (skip < answers.Count)
                {
                    var currentkeyPhraseResponseCount = new List<string>();
                    var keyPhraseResponse = await analyticsClient.ExtractKeyPhrasesBatchAsync(answers.Skip(skip).Take(take).ToList());

                    if (keyPhraseResponse != null && keyPhraseResponse.Value != null && keyPhraseResponse.Value.Any())
                    {
                        currentkeyPhraseResponseCount = keyPhraseResponse.Value.SelectMany(x => x.KeyPhrases).ToList();
                    }

                    if (currentkeyPhraseResponseCount.Any())
                        totalkeyPhraseResponse.AddRange(currentkeyPhraseResponseCount);

                    skip = skip + take;
                }
            }

            if (totalkeyPhraseResponse.Any())
            {
                keyPhraseResponseCount = totalkeyPhraseResponse.GroupBy(x => x).Select(x => new KeyPhrasesCount
                {
                    Text = x.Key,
                    Frequency = x.Count()

                }).ToList();
            }

            return keyPhraseResponseCount;
        }

        private async Task<List<AnswerOption>> getSentimentList(List<string> answers, Azure.AI.TextAnalytics.TextAnalyticsClient analyticsClient, int submittedCount)
        {
            var analysisMainReport = new List<AnswerOption>();
            var totalAnalysisResponse = new List<TextSentiment>();
           
            if (answers.Any())
            {
                int skip = 0, take = 10;

                while (skip < answers.Count)
                {
                    var currentAnalysisResponse = new List<TextSentiment>();

                    var sentiResponse = await analyticsClient.AnalyzeSentimentBatchAsync(answers.Skip(skip).Take(take).ToList());

                    if (sentiResponse != null && sentiResponse.Value != null)
                    {
                        currentAnalysisResponse = sentiResponse.Value.Select(x => x.DocumentSentiment).Select(x => x.Sentiment).ToList();
                    }

                    if (currentAnalysisResponse.Any())
                        totalAnalysisResponse.AddRange(currentAnalysisResponse);

                    skip = skip + take;
                }
            }

            if (totalAnalysisResponse.Any())
                analysisMainReport.AddRange(getAnalysisReport(totalAnalysisResponse,submittedCount));

            return analysisMainReport;
        }

        private List<AnswerOption> getAnalysisReport(List<TextSentiment> totalAnalysisResponse, int submittedCount)
        {
            var analysisMainReport = new List<AnswerOption>();
            var colorPalettes = new string[] { "#2f4b7c", "#665191", "#f95d6a", "#ff7c43", "#ffa600" };
            Dictionary<string, string> SentimentColors = new Dictionary<string, string>()
                                                    {
                                                      {"POSITIVE", "#01B050"},
                                                      {"NEGATIVE", "#CC0000"},
                                                      {"NEUTRAL", "#FFA176"},
                                                      {"MIXED", "#ffb513"}
                                                    };
           
                int index = 0;
                analysisMainReport = totalAnalysisResponse.GroupBy(x => x).Select(x => new AnswerOption
                {
                    Text = x.Key.ToString(),
                    ResponseCount = x.Count(),
                    SelectionPercentage = submittedCount > 0 ? Math.Round((Convert.ToDecimal(x.Count()) / submittedCount) * 100, 2) : 0,
                    PercentageText = (submittedCount > 0 ? Math.Round((Convert.ToDecimal(x.Count()) / submittedCount) * 100, 2) : 0) + "%",
                    Color = (SentimentColors.ContainsKey(x.Key.ToString().ToUpper())) ? SentimentColors[x.Key.ToString().ToUpper()] : colorPalettes[index++]

                }).ToList();
            
            return analysisMainReport;
        }
    }
}

