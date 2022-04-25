using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using GetAllSubscriptionModeCount = WAS.Application.Features.Report.GetAllSubscriptionModeCount;
using GetAllSubscriptionPerMonth = WAS.Application.Features.Report.GetAllSubscriptionPerMonth;
using GetAllNotificationPerMonth = WAS.Application.Features.Report.GetAllNotifiactionPerMonth;
using GetAllFeedbackChannelCount = WAS.Application.Features.Report.GetAllFeedbackChannelCount;
using GetAllNotificationModeCount = WAS.Application.Features.Report.GetAllNotificationModeCount;
using GetAllLocationCount = WAS.Application.Features.Report.GetAllLocationCount;
using GetAllSubscriptionCountPerDay = WAS.Application.Features.Report.GetAllSubscriptionCountPerDay;
using GetAllLocationCountBySubscriptionPercentage = WAS.Application.Features.Report.GetSubscriptionPercentageByLocation;

namespace WAS.Application.Features.Report.GetReports
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public Handler(
            ILogger<Handler> logger,
             IMediator mediator,
            IMapper mapper
            )
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var subscriptionModeCountRequest = _mapper.Map<GetAllSubscriptionModeCount.Request>(request);
                var subscriptionPerMonthsRequest = _mapper.Map<GetAllSubscriptionPerMonth.Request>(request);
                var notificationPerMonthsRequest = _mapper.Map<GetAllNotificationPerMonth.Request>(request);
                var allGroupRequest = _mapper.Map<GetAllGroupSize.Request>(request);
                var feedbackChannelCountList =_mapper.Map<GetAllFeedbackChannelCount.Request>(request);
                var notificationModeCountList = _mapper.Map<GetAllNotificationModeCount.Request>(request);
                var allSubscriptionLocationList = _mapper.Map<GetAllLocationCount.Request>(request);
                var subscriptioncountPerDay = _mapper.Map<GetAllSubscriptionCountPerDay.Request>(request);
                var subscriptionLocationPercentage = _mapper.Map<GetSubscriptionPercentageByLocation.Request>(request);
                var subscriptionModeCount = await _mediator.Send(subscriptionModeCountRequest);
                var subscriptionPerMonths = await _mediator.Send(subscriptionPerMonthsRequest);
                var notificationPerMonths = await _mediator.Send(notificationPerMonthsRequest);
                var allGroup = await _mediator.Send(allGroupRequest);
                var feedbackChannelCount = await _mediator.Send(feedbackChannelCountList);
                var notificationModeCount = await _mediator.Send(notificationModeCountList);
                var allSubscriptionLocation = await _mediator.Send(allSubscriptionLocationList);
                var subscriptionCountPerDay = await _mediator.Send(subscriptioncountPerDay);
                var subscriptionLocationPercentages = await _mediator.Send(subscriptionLocationPercentage);
                return new Response
                {
                    SubscriptionModeCount = subscriptionModeCount.SubscriptionModeCount,
                    SubscriptionPerMonths = subscriptionPerMonths.SubscriptionPerMonths,
                    NotificationPerMonths = notificationPerMonths.NotificationPerMonths,
                    AllGroups = allGroup.AllGroups,
                    FeedbackChannels = feedbackChannelCount.FeedbackChannels,
                    NotificationChannels = notificationModeCount.NotificationModeCount,
                    SubscriptionLocationCounts = allSubscriptionLocation.SubscriptionLocationCounts,
                    SubscriberAndUnsubscriberCountPerDays = subscriptionCountPerDay.subscriberAndUnsubscriberCountPerDays,
                    SubscriptionLocationPercentage = subscriptionLocationPercentages.SubscriptionLocationPercentage
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }

        }
    }
}
 