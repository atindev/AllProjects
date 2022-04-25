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


namespace WAS.Application.Features.Survey.GetByBroadcastId
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ITimeParser _timeParser;

        public Handler(
            IWasContext context,
            IMediator mediator,
            ILogger<Handler> logger,
            IMapper mapper,
            ITimeParser timeParser
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _timeParser = timeParser;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var surveyEntity = await _context.SurveyBroadcasts
                    .Include(i => i.Survey)
                    .Include(i => i.SurveyBroadcastGroups)
                       .ThenInclude(i => i.Group)
                    .Include(i => i.SurveyBroadcastSubscriptions)
                       .ThenInclude(i => i.Subscription)
                    .Include(i => i.SurveyBroadcastDistributionGroups)
                    .Include(i => i.SurveyBroadcastADUsers)
                    .ThenInclude(i => i.Location)
                    .Include(i => i.SurveyBroadcastADUsers)
                    .ThenInclude(i => i.Department)
                    .Include(i => i.SurveyBroadcastFollowup)
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

                if (surveyEntity == null)
                    throw new NotFoundException($"Broadcasted survey not found with the id {request.Id}");

                var response = _mapper.Map<Response>(surveyEntity);

                if (!request.IgnoreAudienceResponseCount)
                {
                    //total Submitted count
                    var SubmittedList = await _mediator.Send(new submitted.Request { Id = request.Id, IsOnlySubscriberIds = false });
                    if (SubmittedList.Answers.Any())
                    {
                        response.SubmittedCount = SubmittedList.Answers.Count;

                        int minTime = SubmittedList.Answers.Min(m => m.SurveyCompletionTime);
                        int avgTime = (int)SubmittedList.Answers.Average(m => m.SurveyCompletionTime);
                        int maxTime = SubmittedList.Answers.Max(m => m.SurveyCompletionTime);

                        response.MinTime = _timeParser.GetTime(minTime);
                        response.AvgTime = _timeParser.GetTime(avgTime);
                        response.MaxTime = _timeParser.GetTime(maxTime);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }


    }
}

