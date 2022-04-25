using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Interface;
using WAS.Domain.Entities;

namespace WAS.Application.Features.Notification.DeliveryReport
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IWasContext _context;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try 
            {
                var notificationEntity = await _context.Notifications
                    .Include(i => i.NotificationText)
                        .ThenInclude(i => i.DeliveryReportTexts)
                    .Include(i => i.NotificationVoice)
                        .ThenInclude(i => i.DeliveryReportVoices)
                    .Include(i => i.NotificationWhatsApp)
                        .ThenInclude(i => i.DeliveryReportWhatsApps)
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(o => o.Id == request.Id);

                var response = new Response();
                if (notificationEntity.NotificationText != null && notificationEntity.NotificationText.DeliveryReportTexts.Any())
                {
                    var deliveryStatusTexts = DeliveryStatusText(notificationEntity.NotificationText.DeliveryReportTexts);
                    response.DeliveryStatusTexts.AddRange(deliveryStatusTexts);
                }
                if (notificationEntity.NotificationVoice != null && notificationEntity.NotificationVoice.DeliveryReportVoices.Any())
                {
                    var deliveryStatusVoices = DeliveryStatusVoice(notificationEntity.NotificationVoice.DeliveryReportVoices);
                    response.DeliveryStatusVoices.AddRange(deliveryStatusVoices);
                }
                if (notificationEntity.NotificationWhatsApp != null && notificationEntity.NotificationWhatsApp.DeliveryReportWhatsApps.Any())
                {
                    var deliveryStatusWhatsApps = DeliveryStatusWhatsApp(notificationEntity.NotificationWhatsApp.DeliveryReportWhatsApps);
                    response.DeliveryStatusWhatsApps.AddRange(deliveryStatusWhatsApps);
                }

                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }

        private List<DeliveryStatus> DeliveryStatusText(IEnumerable<DeliveryReportText> deliveryReportTexts)
        {
            List<DeliveryStatus> deliveryStatusTexts = new List<DeliveryStatus>
            {
                new DeliveryStatus { Status = "Accepted", Count = deliveryReportTexts.Count(x => x.Status.Equals("accepted")), Color = "#eeff5c" },
                new DeliveryStatus { Status = "Queued", Count = deliveryReportTexts.Count(x => x.Status.Equals("queued")), Color = "#dadc22" },
                new DeliveryStatus { Status = "Sending", Count = deliveryReportTexts.Count(x => x.Status.Equals("sending")), Color = "#66efe8" },
                new DeliveryStatus { Status = "Sent", Count = deliveryReportTexts.Count(x => x.Status.Equals("sent")), Color = "#61e865" },
                new DeliveryStatus { Status = "Delivered", Count = deliveryReportTexts.Count(x => x.Status.Equals("delivered")), Color = "green" },
                new DeliveryStatus { Status = "Undelivered", Count = deliveryReportTexts.Count(x => x.Status.Equals("undelivered")), Color = "#e3165b" },
                new DeliveryStatus { Status = "Failed", Count = deliveryReportTexts.Count(x => x.Status.Equals("failed")), Color = "red" },
                new DeliveryStatus { Status = "RanToCompletion", Count = deliveryReportTexts.Count(x => x.Status.Equals("RanToCompletion")), Color = "blue" }
            };

            return deliveryStatusTexts;
        }

        private List<DeliveryStatus> DeliveryStatusVoice(IEnumerable<DeliveryReportVoice> deliveryReportVoice)
        {
            List<DeliveryStatus> deliveryStatusVoices = new List<DeliveryStatus>
            {
                new DeliveryStatus { Status = "Queued", Count = deliveryReportVoice.Count(x => x.Status.Equals("queued")), Color = "#dadc22" },
                new DeliveryStatus { Status = "Initiated", Count = deliveryReportVoice.Count(x => x.Status.Equals("initiated")), Color = "#dcab22" },
                new DeliveryStatus { Status = "Ringing", Count = deliveryReportVoice.Count(x => x.Status.Equals("ringing")), Color = "#66efe8" },
                new DeliveryStatus { Status = "In-Progress", Count = deliveryReportVoice.Count(x => x.Status.Equals("in-progress")), Color = "#61e865"  },
                new DeliveryStatus { Status = "Completed", Count = deliveryReportVoice.Count(x => x.Status.Equals("completed")), Color = "green" },
                new DeliveryStatus { Status = "Busy", Count = deliveryReportVoice.Count(x => x.Status.Equals("busy")), Color = "purple" },
                new DeliveryStatus { Status = "No-Answer", Count = deliveryReportVoice.Count(x => x.Status.Equals("no-answer")), Color ="#e3165b" },
                new DeliveryStatus { Status = "Canceled", Count = deliveryReportVoice.Count(x => x.Status.Equals("canceled")), Color = "#f36f3d" },
                new DeliveryStatus { Status = "Failed", Count = deliveryReportVoice.Count(x => x.Status.Equals("failed")), Color = "red" },
                new DeliveryStatus { Status = "RanToCompletion", Count = deliveryReportVoice.Count(x => x.Status.Equals("RanToCompletion")), Color = "blue" }
            };

            return deliveryStatusVoices;
        }

        private List<DeliveryStatus> DeliveryStatusWhatsApp(IEnumerable<DeliveryReportWhatsApp> deliveryReportWhatsApp)
        {
            List<DeliveryStatus> deliveryStatusWhatsApps = new List<DeliveryStatus>
            {
                new DeliveryStatus { Status = "Sent", Count = deliveryReportWhatsApp.Count(x => x.Status.Equals("sent")), Color = "#61e865" },
                new DeliveryStatus { Status = "Delivered", Count = deliveryReportWhatsApp.Count(x => x.Status.Equals("delivered")), Color = "green" },
                new DeliveryStatus { Status = "Read", Count = deliveryReportWhatsApp.Count(x => x.Status.Equals("read")), Color = "#04d8fa" },
                new DeliveryStatus { Status = "Undelivered", Count = deliveryReportWhatsApp.Count(x => x.Status.Equals("undelivered")), Color = "#e3165b" },
                new DeliveryStatus { Status = "Failed", Count = deliveryReportWhatsApp.Count(x => x.Status.Equals("failed")), Color = "red" },
                new DeliveryStatus { Status = "RanToCompletion", Count = deliveryReportWhatsApp.Count(x => x.Status.Equals("RanToCompletion")), Color = "blue" }
            };

            return deliveryStatusWhatsApps;
        }
    }
}
