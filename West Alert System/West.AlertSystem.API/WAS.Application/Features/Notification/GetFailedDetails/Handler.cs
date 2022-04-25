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
using WAS.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace WAS.Application.Features.Notification.GetFailedDetails
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly TwilioSettings _twilioSettings;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            IOptions<TwilioSettings> twilioSettings
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _twilioSettings = twilioSettings.Value;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var failedNotifications = new List<Common.Models.FailedNotification>();

                if (request.PublishingType == "SMS")
                {
                    var notifications = await _context.NotificationTexts
                        .Include(i => i.DeliveryReportTexts)
                          .ThenInclude(i => i.Subscription)
                        .IgnoreQueryFilters()
                        .SingleOrDefaultAsync(x => x.NotificationId == request.Id,cancellationToken);

                    failedNotifications = _mapper.Map<List<Common.Models.FailedNotification>>(notifications.DeliveryReportTexts);
                }
                else if (request.PublishingType == "WhatsApp")
                {
                    var notifications = await _context.NotificationWhatsApps
                        .Include(i => i.DeliveryReportWhatsApps)
                          .ThenInclude(i => i.Subscription)
                        .IgnoreQueryFilters()
                        .SingleOrDefaultAsync(x => x.NotificationId == request.Id, cancellationToken);

                    failedNotifications = _mapper.Map<List<Common.Models.FailedNotification>>(notifications.DeliveryReportWhatsApps);
                }
                else if(request.PublishingType == "Voice")
                {
                    var notifications = await _context.NotificationVoices
                      .Include(i => i.DeliveryReportVoices)
                        .ThenInclude(i => i.Subscription)
                      .IgnoreQueryFilters()
                      .SingleOrDefaultAsync(x => x.NotificationId == request.Id, cancellationToken);

                    failedNotifications = _mapper.Map<List<Common.Models.FailedNotification>>(notifications.DeliveryReportVoices);
                }

                if (failedNotifications != null)
                {
                    var userList = failedNotifications.Select(n => n.SubscriberEmail).Where(i => i != null).Distinct().ToList()
                           .ConvertAll(d => d.ToLower());
                    var subscriptionLocation = await _context.Subscriptions
                           .IgnoreQueryFilters()
                           .Include(i => i.Location)
                           .Where(s => userList.Contains(s.OfficialEmail.ToLower()))
                           .ToListAsync(cancellationToken);

                    failedNotifications = failedNotifications.Where(x => x.Status == request.Status).ToList();
                    failedNotifications.ForEach(x => {
                        x.ErrorURL = (x.ErrorCode != null && x.ErrorCode != "") ? (_twilioSettings.ErrorUrl + x.ErrorCode): null;

                        if (x.SubscriberEmail!=null && subscriptionLocation.Any(i => i.OfficialEmail.ToLower() == x.SubscriberEmail.ToLower()))
                            x.SubscriberLocation = subscriptionLocation.Where(i => i.OfficialEmail.ToLower() == x.SubscriberEmail.ToLower()).Select(o => o.Location.Name).ElementAt(0);
                        else
                            x.SubscriberLocation = "";
                    });
                }
                  
                return new Response
                {
                    FailedNotifications = failedNotifications,
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
