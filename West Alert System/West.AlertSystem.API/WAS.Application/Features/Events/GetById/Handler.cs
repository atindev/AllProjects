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
using GetNotificationDistinctSubscribers = WAS.Application.Features.Group.GetNotificationDistinctSubscriberCount;
using GetDeliveryReport = WAS.Application.Features.Notification.DeliveryReport;
using Entity = WAS.Domain.Entities;
using GetGroupSubscriptions = WAS.Application.Features.Group.GetGroupSubscriberCount;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Events.GetById
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly ITimeParser _timeParser;
        private readonly IMediator _mediator;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            ITimeParser timeParser,
            IMediator mediator

            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _timeParser = timeParser;
            _mediator = mediator;

        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var eventEntity = await _context.Events
                    .Include(i => i.EventType)
                    .Include(i => i.EventUrgency)
                    .Include(i => i.Notifications)
                        .ThenInclude(i => i.NotificationEmail)
                            .ThenInclude(i => i.NotificationEmailAttachments)
                    .Include(i => i.Notifications)
                        .ThenInclude(i => i.NotificationText)
                    .Include(i => i.Notifications)
                        .ThenInclude(i => i.NotificationVoice)
                    .Include(i => i.Notifications)
                        .ThenInclude(i => i.NotificationWhatsApp)
                    .Include(i => i.Notifications)
                        .ThenInclude(i => i.NotificationGroups)
                        .ThenInclude(i => i.Group)
                          .ThenInclude(i => i.SubscriptionGroups)
                           .ThenInclude(i => i.Subscription)
                    .Include(i => i.Notifications)
                        .ThenInclude(i => i.NotificationSubscriptions)
                            .ThenInclude(i => i.Subscription)
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

                var eventsTypes = await _context.EventTypes
                   .ToListAsync(cancellationToken);

                var eventUrgencies = await _context.EventUrgencies
                    .ToListAsync(cancellationToken);

                if (eventEntity == null)
                    throw new NotFoundException($"Event not found with the id {request.Id}");

                var responseNotifications = new List<Common.Models.Notification>();

                if (eventEntity.Notifications.Any())
                {
                    foreach (var notification in eventEntity.Notifications)
                    {
                        //checking for private notification
                        if (request.IsGlobalAdmin || !notification.IsPrivateNotification || (notification.IsPrivateNotification && notification.CreatedBy == request.Email)){
                            var currentNotification = await GetCurrentNotificationData(notification);
                            responseNotifications.Add(currentNotification);
                        }
                    }

                    if (responseNotifications.Any())
                        responseNotifications = responseNotifications.OrderByDescending(i => i.CreatedDate).ToList();
                }

                var response = new Response()
                {
                    Event = _mapper.Map<Common.Models.Event>(eventEntity),
                    EventTypes = _mapper.Map<List<Common.Models.EventType>>(eventsTypes),
                    EventUrgencies = _mapper.Map<List<Common.Models.EventUrgency>>(eventUrgencies)
                };

                response.Event.Notifications = responseNotifications;
                response.Event.Updated = _timeParser.RelativeTime(response.Event.CreatedDate);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }

        private async Task<Common.Models.Notification> GetCurrentNotificationData(Entity.Notification notification)
        {
            var currentNotification = _mapper.Map<Common.Models.Notification>(notification);

            var groupDetails = await _mediator.Send(new GetNotificationDistinctSubscribers.Request { Ids = notification.NotificationGroups.Select(s => s.GroupId).ToList(), SubscriptionIds = notification.NotificationSubscriptions.Select(s => s.SubscriptionId).ToList(), CreatedDate = notification.CreatedDate });
            if (groupDetails != null)
            {
                currentNotification.TextSentToCount = groupDetails.SMSCount;
                currentNotification.VoiceSentToCount = groupDetails.VoiceCount;
                currentNotification.EmailSentToCount = groupDetails.EmailCount;
                currentNotification.WhatsAppSentToCount = groupDetails.WhatsappCount;
            }

            foreach (var x in notification.NotificationGroups)
            {
                var groupSubscriptions = await _mediator.Send(new GetGroupSubscriptions.Request { GroupId = x.GroupId, CreatedDate = currentNotification.CreatedDate });
                currentNotification.GroupSubscribers.Add(groupSubscriptions);
            }

            if (notification.NotificationEmail != null && notification.NotificationEmail.NotificationEmailAttachments.Any())
            {
                notification.NotificationEmail.NotificationEmailAttachments.ToList().ForEach(ea =>
                {
                    currentNotification.EmailAttachments.Add(new AttachmentData
                    {
                        NotificationEmailId = ea.NotificationEmailId,
                        FileName = ea.FileName,
                        ContentType = ea.ContentType
                    });
                });
            }

            var deliveryReport = await _mediator.Send(new GetDeliveryReport.Request { Id = currentNotification.Id });
            if (deliveryReport != null)
            {
                currentNotification.DeliveryStatusText.AddRange(deliveryReport.DeliveryStatusTexts);
                currentNotification.DeliveryStatusVoice.AddRange(deliveryReport.DeliveryStatusVoices);
                currentNotification.DeliveryStatusWhatsApp.AddRange(deliveryReport.DeliveryStatusWhatsApps);
            }

            return currentNotification;
        }
    }
}

