using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using EventGetCount = WAS.Application.Features.Events.GetCount;
using NotificationGetCount = WAS.Application.Features.Notification.GetCount;
using GroupGetCount = WAS.Application.Features.Group.GetCount;
using PeopleGetCount = WAS.Application.Features.Subscription.GetCount;
using recentNotification = WAS.Application.Features.Notification.GetAll;
using incomingMessage = WAS.Application.Features.IncomingMessage.GetAll;
using AutoMapper;

namespace WAS.Application.Features.Dashboard.GetDashboard
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
                var eventCount = await _mediator.Send(new EventGetCount.Request());
                var notificationCount = await _mediator.Send(new NotificationGetCount.Request());
                var groupCount= await _mediator.Send(new GroupGetCount.Request());
                var peopleCount = await _mediator.Send(new PeopleGetCount.Request());

                var incomingMessagesRequest = _mapper.Map<incomingMessage.Request>(request);
                var incomingMessages = await _mediator.Send(incomingMessagesRequest);

                var notificationRequest = _mapper.Map<recentNotification.Request>(request);
                var notifications = await _mediator.Send(notificationRequest);

                return new Response {
                    IncomingMessageCount = incomingMessages.Count,
                    NotificationCount = notificationCount.Count,
                    EventCount = eventCount.Count,
                    GroupCount = groupCount.Count,
                    PeopleCount = peopleCount.Count,
                    IncomingMessages = incomingMessages.IncomingMessages,
                    RecentNotifications = notifications.RecentNotifications
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
