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
using Entity = WAS.Domain.Entities;
using WAS.Application.Common.Models;
using getAudience = WAS.Application.Features.Survey.GetUniqueSubscribers;
using submitted = WAS.Application.Features.Survey.GetAllSubmitted;


namespace WAS.Application.Features.Survey.GetSubmissionReportByLocation
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public Handler(
            IWasContext context,
            IMediator mediator,
            ILogger<Handler> logger,
            IMapper mapper
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var finalResponse = new Response();

                var surveyEntity = await _context.SurveyBroadcasts
                    .Include(i=>i.SurveyBroadcastGroups)
                    .Include(i => i.SurveyBroadcastSubscriptions)
                    .Include(i => i.SurveyBroadcastADUsers)
                        .ThenInclude(i => i.Location)
                    .Include(i => i.SurveyBroadcastADUsers)
                        .ThenInclude(i => i.Department)
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
                 
                if (surveyEntity == null)
                    throw new NotFoundException($"Broadcasted survey not found with the id {request.Id}");
                 
                //total distinct audience 
                var distinctAudience=await _mediator.Send(new getAudience.Request { Ids = surveyEntity.SurveyBroadcastGroups.Select(x=>x.GroupId).ToList(),
                    SubscriptionIds = surveyEntity.SurveyBroadcastSubscriptions.Select(x => x.SubscriptionId).ToList()
                });

                var submissionReportUniqueAudience = GetSubmissionReportByLocationUniqueAudience(distinctAudience.Audience, surveyEntity.SurveyBroadcastADUsers.ToList());

                if (submissionReportUniqueAudience.Any())
                {
                    finalResponse.CompletedSurveyByLocation = new List<CompletedSurveyByLocation>();
                    finalResponse.PendingSubmissionSurveyByLocation = new List<PendingSurveySubmissionByLocation>();

                    var distinctLocations = submissionReportUniqueAudience.GroupBy(x => x.LocationId).Select(y => y.FirstOrDefault()).ToList();

                    var SubmittedAudience = await _mediator.Send(new submitted.Request { Id = request.Id , IsOnlySubscriberIds = false });

                    foreach (var item in distinctLocations)
                    {
                        var totalAudience = submissionReportUniqueAudience.Where(x => x.LocationId == item.LocationId).Select(t => t.OfficialEmail);
                        decimal totalAudienceCount = totalAudience.Count();
                        decimal submittedCount = 0;
                        foreach(var id in totalAudience)
                        {
                            var currentData = SubmittedAudience.Answers.FirstOrDefault(x => x.Email == id);
                            if (currentData != null && currentData.Answers != null)
                                ++submittedCount;
                        }
                        var remainigCount = totalAudienceCount - submittedCount;
                        decimal CompletedPercent = Math.Round((submittedCount / totalAudienceCount) * 100, 2);
                        decimal remainingPercentage = 100 - CompletedPercent;

                        CompletedSurveyByLocation CompletedSurveyByLocation = new CompletedSurveyByLocation
                        {
                            LocationName = item.LocationName,
                            SubmittedCount = submittedCount,
                            SubmissionPercentage = CompletedPercent
                        };

                        PendingSurveySubmissionByLocation RemainingSurveyByLocation = new PendingSurveySubmissionByLocation
                        {
                            LocationName = item.LocationName,
                            PendingCount = remainigCount,
                            PendingPercentage = remainingPercentage
                        };

                        finalResponse.CompletedSurveyByLocation.Add(CompletedSurveyByLocation);
                        finalResponse.PendingSubmissionSurveyByLocation.Add(RemainingSurveyByLocation);
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

        private List<SubmissionReportUniqueAudience> GetSubmissionReportByLocationUniqueAudience(List<Audience>subscribedAudience, List<Entity.SurveyBroadcastADUser> surveyBroadcastAdUser)
        {
            var submissionReportSubscribedeAudience = _mapper.Map<List<SubmissionReportUniqueAudience>>(subscribedAudience);
            var submissionReportAdUsers = _mapper.Map<List<SubmissionReportUniqueAudience>>(surveyBroadcastAdUser);

            var submissionReportUniqueAudiences = submissionReportSubscribedeAudience.Union(submissionReportAdUsers).GroupBy(g => g.OfficialEmail).Select(x => x.First()).ToList();
            return submissionReportUniqueAudiences;
        }
    }
}

