using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using AutoMapper;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Notification.GetAll
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly ITimeParser _timeParser;        

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            ITimeParser timeParser
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _timeParser = timeParser;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var notification = await _context.Notifications
                         .Include(i => i.Event)
                         .Include(i => i.NotificationEmail)
                         .Include(i => i.NotificationText)
                         .Include(i => i.NotificationVoice)
                         .Include(i => i.NotificationWhatsApp)
                         .Include(i => i.IncomingMessages)
                         .Include(i => i.NotificationGroups)
                           .ThenInclude(x => x.Group)
                         .Include(i => i.NotificationSubscriptions)
                            .ThenInclude(i => i.Subscription)
                         .OrderByDescending(i => i.CreatedDate)
                         .Where(i => (i.Event.IsActive) && (request.EventFilter == null || i.Event.Name.Equals(request.EventFilter))
                           && (request.StatusFilter == 0 || i.Status == request.StatusFilter)
                           && (request.MessageFilter == null ||
                                (i.NotificationText.Message.Contains(request.MessageFilter)
                                  || i.NotificationVoice.Message.Contains(request.MessageFilter)
                                  || i.NotificationVoice.Message.Contains(request.MessageFilter)
                                  || i.NotificationWhatsApp.Message.Contains(request.MessageFilter)
                                ))
                            &&(request.IsGlobalAdmin || !i.IsPrivateNotification || (i.IsPrivateNotification && i.CreatedBy==request.Email))
                           )
                         .IgnoreQueryFilters()
                         .ToListAsync(cancellationToken);

                int Count = 0;

                var responseNotification = _mapper.Map<List<Common.Models.Notification>>(notification);

                if (responseNotification != null && responseNotification.Count > 0)
                {
                    Count = responseNotification.Count;
                }

                var responseNotificationList =await getRecentNotificationsAsync(responseNotification, request.PageType, request.RowCount, request.PageIndex, cancellationToken);

                return new Response
                {
                    RecentNotifications = responseNotificationList,
                    Count = Count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }

        }
        private async Task<List<Common.Models.Notification>> getRecentNotificationsAsync(List<Common.Models.Notification> responseNotification, string pageType, int rowCount, int pageIndex, CancellationToken cancellationToken)
        {
            if (pageType == "Paged" && responseNotification != null && responseNotification.Count > 0)
            {
                rowCount = (rowCount == 0) ? 7 : rowCount;
                responseNotification = responseNotification.Skip(pageIndex).Take(rowCount).ToList();
            }

            if (responseNotification != null && responseNotification.Count > 0)
            {
                var userList = responseNotification
                .SelectMany(t => new[] { t.CreatedBy, t.ApproverForPrivate })
                .Where(i=>i!=null)
                .Distinct()
                .ToList()
                .ConvertAll(d => d.ToLower());

                var subscriptionLocation = await _context.Subscriptions
                 .IgnoreQueryFilters()
                 .Include(i => i.Location)
                 .Where(s => userList.Contains(s.OfficialEmail.ToLower()))
                 .ToListAsync(cancellationToken);

                responseNotification.ForEach(x =>
                {
                    x = getUpdatedNotificationsAsync(x, subscriptionLocation);
                });
            }
            return responseNotification;
        }
   
        private Common.Models.Notification getUpdatedNotificationsAsync(Common.Models.Notification notification,List<Domain.Entities.Subscription> subscriptionLocation)
        {
            if (notification.TextMessage != null)
                notification.Topic = notification.TextMessage;
            else if (notification.VoiceMessage != null)
                notification.Topic = notification.VoiceMessage;
            else if (notification.EmailMessage != null)
                notification.Topic = notification.EmailMessage;
            else if (notification.WhatsAppMessage != null)
                notification.Topic = notification.WhatsAppMessage;
            else
                notification.Topic = "";

            notification.Updated = _timeParser.RelativeTime(notification.CreatedDate);

            if (notification.FinalApprovalDate != null)
                notification.StatusUpdated = _timeParser.RelativeTime(notification.FinalApprovalDate.Value);

            if (notification.CreatedBy!=null && subscriptionLocation.Any(i => i.OfficialEmail.ToLower() == notification.CreatedBy.ToLower()))
                notification.CreaterLocation = subscriptionLocation.Where(i => i.OfficialEmail.ToLower() == notification.CreatedBy.ToLower()).Select(o => o.Location.Name).ElementAt(0);
            else
                notification.CreaterLocation = "";

            if (notification.ApproverForPrivate != null && notification.IsPrivateNotification && (notification.IsApprovalRequired ?? false) && subscriptionLocation.Any(i => i.OfficialEmail.ToLower() == notification.ApproverForPrivate.ToLower()))
                notification.PrivateApproverLocation = subscriptionLocation.Where(i => i.OfficialEmail.ToLower() == notification.ApproverForPrivate.ToLower()).Select(o => o.Location.Name).ElementAt(0);
            else
                notification.PrivateApproverLocation = "";
            
            return notification;
        }

    }
}
