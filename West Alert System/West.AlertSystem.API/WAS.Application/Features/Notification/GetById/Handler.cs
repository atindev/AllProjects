using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using Microsoft.EntityFrameworkCore;
using WAS.Application.Interface.Services;
using GetNotificationDistinctSubscribers= WAS.Application.Features.Group.GetNotificationDistinctSubscriberCount;
using GetDeliveryReport = WAS.Application.Features.Notification.DeliveryReport;
using GetGroupSubscriptions = WAS.Application.Features.Group.GetGroupSubscriberCount;
using WAS.Application.Common.Models;
using System.Collections.Generic;
using Entity = WAS.Domain.Entities;

namespace WAS.Application.Features.Notification.GetById
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
                var notificationEntity = await _context.Notifications
                    .Include(i => i.NotificationGroups)
                        .ThenInclude(i => i.Group)
                    .Include(i => i.NotificationSubscriptions)
                        .ThenInclude(i => i.Subscription)
                    .Include(i => i.Event)
                        .ThenInclude(i => i.EventType)
                    .Include(i => i.Event)
                        .ThenInclude(i => i.EventUrgency)
                    .Include(i => i.NotificationEmail)
                        .ThenInclude(i => i.NotificationEmailAttachments)
                    .Include(i => i.NotificationText)
                    .Include(i => i.NotificationVoice)
                    .Include(i => i.NotificationWhatsApp)
                    .Include(i => i.IncomingMessages)
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(o => o.Id == request.Id);

                if (notificationEntity == null)
                    throw new NotFoundException($"Notification not found with the id {request.Id}");

                var response = new Response { Notification = await GetUserLocation(notificationEntity,cancellationToken) };

                response.Notification.IncomingMessages = response.Notification.IncomingMessages.OrderByDescending(i => i.CreatedDate).ToList();

                var groupSubscriptions = await GetGroupSubscriberData(notificationEntity.NotificationGroups, notificationEntity.CreatedDate);
                response.Notification.GroupSubscribers.AddRange(groupSubscriptions);
                
                response.Notification.Updated = _timeParser.RelativeTime(response.Notification.CreatedDate);
                if (notificationEntity.NotificationEmail != null && notificationEntity.NotificationEmail.NotificationEmailAttachments.Any())
                {
                    notificationEntity.NotificationEmail.NotificationEmailAttachments.ToList().ForEach(ea =>
                    {
                        response.Notification.EmailAttachments.Add(new AttachmentData
                        {
                            NotificationEmailId = ea.NotificationEmailId,
                            FileName = ea.FileName,
                            ContentType = ea.ContentType
                        });
                    });
                }

                var groupDetails = await _mediator.Send(new GetNotificationDistinctSubscribers.Request { Ids = notificationEntity.NotificationGroups.Select(s => s.GroupId).ToList(), SubscriptionIds = notificationEntity.NotificationSubscriptions.Select(s => s.SubscriptionId).ToList(), CreatedDate = notificationEntity.CreatedDate });
                if (groupDetails != null)
                {
                    response.Notification.TextSentToCount = groupDetails.SMSCount;
                    response.Notification.VoiceSentToCount = groupDetails.VoiceCount;
                    response.Notification.EmailSentToCount = groupDetails.EmailCount;
                    response.Notification.WhatsAppSentToCount = groupDetails.WhatsappCount;
                }


                var deliveryReport = await _mediator.Send(new GetDeliveryReport.Request { Id = request.Id });
                if (deliveryReport != null)
                {
                    response.Notification.DeliveryStatusText.AddRange(deliveryReport.DeliveryStatusTexts);
                    response.Notification.DeliveryStatusVoice.AddRange(deliveryReport.DeliveryStatusVoices);
                    response.Notification.DeliveryStatusWhatsApp.AddRange(deliveryReport.DeliveryStatusWhatsApps);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }

        private async Task<List<GetGroupSubscriptions.Response>> GetGroupSubscriberData(IEnumerable<Entity.NotificationGroup> notificationGroup, DateTime createdDate)
        {
            List<GetGroupSubscriptions.Response> response = new List<GetGroupSubscriptions.Response>();

            foreach (var x in notificationGroup)
            {
                var groupSubscriptions = await _mediator.Send(new GetGroupSubscriptions.Request { GroupId = x.GroupId, CreatedDate = createdDate });
                response.Add(groupSubscriptions);
            }

            return response;
        }

        private async Task<Common.Models.Notification> GetUserLocation(Domain.Entities.Notification notificationEntity, CancellationToken cancellationToken)
        {
            var notification = _mapper.Map<Common.Models.Notification>(notificationEntity);

            var userList = new List<string>() {notification.CreatedBy,
                notification.ApproverForPrivate,
                notification.ApprovedBy,
                notification.FinalApprovalBy
            }.Where(i=>i!=null).ToList().ConvertAll(d => d.ToLower());

            var subscriptionLocation = await _context.Subscriptions
                .IgnoreQueryFilters()
                .Include(i => i.Location)
                .Where(s => userList.Contains(s.OfficialEmail.ToLower()))
                .ToListAsync(cancellationToken);

            if (notification.CreatedBy!=null && subscriptionLocation.Any(i => i.OfficialEmail.ToLower() == notification.CreatedBy.ToLower()))
                notification.CreaterLocation = subscriptionLocation.Where(i => i.OfficialEmail.ToLower() == notification.CreatedBy.ToLower()).Select(o => o.Location.Name).ElementAt(0);
            else
                notification.CreaterLocation = "";

            if (notification.ApproverForPrivate!=null && notification.IsPrivateNotification && (notification.IsApprovalRequired ?? false) && subscriptionLocation.Any(i => i.OfficialEmail.ToLower() == notification.ApproverForPrivate.ToLower()))
                notification.PrivateApproverLocation = subscriptionLocation.Where(i => i.OfficialEmail.ToLower() == notification.ApproverForPrivate.ToLower()).Select(o => o.Location.Name).ElementAt(0);
            else
                notification.PrivateApproverLocation = "";

            if (notification.ApprovedBy!=null && subscriptionLocation.Any(i => i.OfficialEmail.ToLower() == notification.ApprovedBy.ToLower()))
                notification.FirstLevelApproverLocation = subscriptionLocation.Where(i => i.OfficialEmail.ToLower() == notification.ApprovedBy.ToLower()).Select(o => o.Location.Name).ElementAt(0);
            else
                notification.FirstLevelApproverLocation = "";

            if (notification.FinalApprovalBy!=null && subscriptionLocation.Any(i => i.OfficialEmail.ToLower() == notification.FinalApprovalBy.ToLower()))
                notification.SecondLevelApproverLocation = subscriptionLocation.Where(i => i.OfficialEmail.ToLower() == notification.FinalApprovalBy.ToLower()).Select(o => o.Location.Name).ElementAt(0);
            else
                notification.SecondLevelApproverLocation = "";

            return notification;
        }

    }
}
