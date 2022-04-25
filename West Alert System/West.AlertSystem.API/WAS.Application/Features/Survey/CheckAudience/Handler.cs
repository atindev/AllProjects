using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Settings;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using ObjectsComparer;
using System.Linq;
using GetUniqueSubscribers = WAS.Application.Features.Survey.GetUniqueSubscribers;


namespace WAS.Application.Features.Survey.CheckAudience
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMediator _mediator;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMediator mediator
            )
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new Response();

                var subscriptionEntity = await _context.Subscriptions
                     .FirstOrDefaultAsync(o => (o.OfficialEmail == request.OfficialEmail || o.EmployeeId == request.EmployeeId), cancellationToken);


                var surveys = await _context.SurveyBroadcasts
               .Include(ng => ng.SurveyBroadcastGroups)
               .Include(ns => ns.SurveyBroadcastSubscriptions)
               .Include(ns => ns.SurveyBroadcastADUsers)
               .IgnoreQueryFilters()
               .FirstOrDefaultAsync(s => s.Id == request.BroadcastId, cancellationToken);
                
                if (surveys != null)
                {
                    var GroupIds = surveys.SurveyBroadcastGroups.Select(g => g.GroupId).ToList();
                    var SubscriptionIds = surveys.SurveyBroadcastSubscriptions.Select(s => s.SubscriptionId).ToList();
                    var groupDetails = await _mediator.Send(new GetUniqueSubscribers.Request { Ids = GroupIds, SubscriptionIds = SubscriptionIds });
                    foreach (var audience in groupDetails.Audience)
                    {
                        if (audience.SubscriberOfficialEmail == request.OfficialEmail || audience.EmployeeId == request.EmployeeId)
                        {
                            response.SubscriptionId = audience.SubscriberId;
                            break;
                        }
                    }

                    foreach (var adUser in surveys.SurveyBroadcastADUsers)
                    {
                        if (adUser.EmailId == request.OfficialEmail)
                        {
                            response.SubscriptionId = Guid.NewGuid();
                            break;
                        }
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

