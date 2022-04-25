using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using Microsoft.EntityFrameworkCore;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Notification.GetByStatus
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
                var newNotificationEntities = await getNotificationList(request, cancellationToken);

                var newResponseNotifications = _mapper.Map<List<Common.Models.Notification>>(newNotificationEntities);

                if (newResponseNotifications != null && newResponseNotifications.Any())
                {
                    var userList = newResponseNotifications
                                   .SelectMany(t => new[] { t.CreatedBy, t.ApproverForPrivate })
                                   .Where(i => i != null)
                                   .Distinct()
                                   .ToList()
                                   .ConvertAll(d => d.ToLower());

                    var subscriptionLocation = await _context.Subscriptions
                     .IgnoreQueryFilters()
                     .Include(i => i.Location)
                     .Where(s => userList.Contains(s.OfficialEmail.ToLower()))
                     .ToListAsync(cancellationToken);

                    newResponseNotifications.ForEach(x =>
                    {
                        x.Topic = x.TextMessage ?? x.VoiceMessage ?? x.EmailMessage ?? x.WhatsAppMessage;
                        x.Updated = _timeParser.RelativeTime(x.CreatedDate);

                        if (x.CreatedBy!=null && subscriptionLocation.Any(i => i.OfficialEmail.ToLower() == x.CreatedBy.ToLower()))
                            x.CreaterLocation = subscriptionLocation.Where(i => i.OfficialEmail.ToLower() == x.CreatedBy.ToLower()).Select(o => o.Location.Name).ElementAt(0);
                        else
                            x.CreaterLocation = "";

                        if (x.ApproverForPrivate!=null && x.IsPrivateNotification && (x.IsApprovalRequired ?? false) && subscriptionLocation.Any(i => i.OfficialEmail.ToLower() == x.ApproverForPrivate.ToLower()))
                            x.PrivateApproverLocation = subscriptionLocation.Where(i => i.OfficialEmail.ToLower() == x.ApproverForPrivate.ToLower()).Select(o => o.Location.Name).ElementAt(0);
                        else
                            x.PrivateApproverLocation = "";

                    });
                }

                return new Response { Notifications = newResponseNotifications };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }

        }

        private async Task<List<Domain.Entities.Notification>> getNotificationList(Request request, CancellationToken cancellationToken)
        {
            var newNotificationEntities = new List<Domain.Entities.Notification>();

            if (request.IsGlobalAdmin)
            {
                newNotificationEntities = await _context.Notifications
                    .Include(i => i.Event)
                    .Include(i => i.NotificationEmail)
                    .Include(i => i.NotificationText)
                    .Include(i => i.NotificationVoice)
                    .Include(i => i.NotificationWhatsApp)
                    .Include(i => i.NotificationGroups)
                        .ThenInclude(i => i.Group)
                    .Include(i => i.NotificationSubscriptions)
                        .ThenInclude(i => i.Subscription)
                    .Where(o => ((o.IsApprovalRequired ?? false) && (o.Status == Domain.Enum.Status.FirstLevelApproved || o.Status==Domain.Enum.Status.Submitted)))
                    .OrderByDescending(i=>i.CreatedDate)
                    .IgnoreQueryFilters()
                    .ToListAsync(cancellationToken);
            }
            else
            {
                if (request.IsOnlyPrivateApprover)
                {
                    newNotificationEntities = await _context.Notifications
                        .Include(i => i.Event)
                        .Include(i => i.NotificationEmail)
                        .Include(i => i.NotificationText)
                        .Include(i => i.NotificationVoice)
                        .Include(i => i.NotificationWhatsApp)
                        .Include(i => i.NotificationGroups)
                            .ThenInclude(i => i.Group)
                        .Include(i => i.NotificationSubscriptions)
                            .ThenInclude(i => i.Subscription)
                        .Where(o => (o.Status == Domain.Enum.Status.FirstLevelApproved && o.ApproverForPrivate == request.Email && (o.IsApprovalRequired ?? false)))
                        .OrderByDescending(i=>i.CreatedDate)
                        .IgnoreQueryFilters()
                        .ToListAsync(cancellationToken);
                }
                else
                {
                    newNotificationEntities = await getNotificationListForApprovers(request, cancellationToken);
                }
            }

            return newNotificationEntities;
        }

        private async Task<List<Domain.Entities.Notification>> getNotificationListForApprovers(Request request, CancellationToken cancellationToken)
        {
            var newNotificationEntities = new List<Domain.Entities.Notification>();

            if (request.HavingBothApprovalLevel)
            {
                newNotificationEntities = await _context.Notifications
                 .Include(i => i.Event)
                 .Include(i => i.NotificationEmail)
                 .Include(i => i.NotificationText)
                 .Include(i => i.NotificationVoice)
                 .Include(i => i.NotificationWhatsApp)
                 .Include(i => i.NotificationGroups)
                     .ThenInclude(i => i.Group)
                 .Include(i => i.NotificationSubscriptions)
                     .ThenInclude(i => i.Subscription)
                 .Where(o => ((!o.IsPrivateNotification && (o.IsApprovalRequired ?? false) && (o.Status == Domain.Enum.Status.FirstLevelApproved || o.Status == Domain.Enum.Status.Submitted)) ||
                 (o.IsPrivateNotification && o.Status == Domain.Enum.Status.FirstLevelApproved && o.ApproverForPrivate == request.Email && (o.IsApprovalRequired ?? false))))
                 .OrderByDescending(i => i.CreatedDate)
                 .IgnoreQueryFilters()
                 .ToListAsync(cancellationToken);
            }
            else
            {
                newNotificationEntities = await _context.Notifications
                  .Include(i => i.Event)
                  .Include(i => i.NotificationEmail)
                  .Include(i => i.NotificationText)
                  .Include(i => i.NotificationVoice)
                  .Include(i => i.NotificationWhatsApp)
                  .Include(i => i.NotificationGroups)
                      .ThenInclude(i => i.Group)
                  .Include(i => i.NotificationSubscriptions)
                      .ThenInclude(i => i.Subscription)
                  .Where(o => (o.Status == request.Status || (o.Status == Domain.Enum.Status.FirstLevelApproved && o.ApproverForPrivate == request.Email && (o.IsApprovalRequired ?? false))))
                  .OrderByDescending(i => i.CreatedDate)
                  .IgnoreQueryFilters()
                  .ToListAsync(cancellationToken);
            }
            return newNotificationEntities;
        }
    }
}
