using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Enum;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Interface;

namespace WAS.Application.Features.Report.GetAllNotificationModeCount
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                List<NotificationChannelCount> NotificationChannels = new List<NotificationChannelCount>();
                List<Domain.Entities.DeliveryReportWhatsApp> DeliveryReportWhatsAppscount;
                List<Domain.Entities.DeliveryReportText> DeliveryReportTextscount;
                List<Domain.Entities.DeliveryReportVoice> DeliveryReportVoicescount;
                if (request.LocationId!=0)
                {
                    DeliveryReportWhatsAppscount = await _context.DeliveryReportWhatsApps.Include(x=>x.Subscription).Where(x=>x.Subscription.LocationId == request.LocationId).IgnoreQueryFilters().ToListAsync(cancellationToken);
                    DeliveryReportTextscount = await _context.DeliveryReportTexts.Include(x => x.Subscription).Where(x => x.Subscription.LocationId == request.LocationId).IgnoreQueryFilters().ToListAsync(cancellationToken);
                    DeliveryReportVoicescount = await _context.DeliveryReportVoices.Include(x => x.Subscription).Where(x => x.Subscription.LocationId == request.LocationId).IgnoreQueryFilters().ToListAsync(cancellationToken);
                }
                else
                {
                     DeliveryReportWhatsAppscount = await _context.DeliveryReportWhatsApps.IgnoreQueryFilters().ToListAsync(cancellationToken);
                     DeliveryReportTextscount = await _context.DeliveryReportTexts.IgnoreQueryFilters().ToListAsync(cancellationToken);
                     DeliveryReportVoicescount = await _context.DeliveryReportVoices.IgnoreQueryFilters().ToListAsync(cancellationToken);
                }
                
                NotificationChannelCount whatsApp = new NotificationChannelCount()
                {
                    NotificationChannel = Common.Enum.NotificationChannel.WhatsApp.ToString(),
                    Count = DeliveryReportWhatsAppscount.Count(x => x.Status != "failed" || x.Status != "undelivered")
                    };
                NotificationChannelCount Text  = new NotificationChannelCount()
                    {
                        NotificationChannel = Common.Enum.NotificationChannel.Text.ToString(),
                        Count = DeliveryReportTextscount.Count(x=>x.Status != "failed" || x.Status != "undelivered")
                    };
                NotificationChannelCount Voice = new NotificationChannelCount()
                    {
                        NotificationChannel = Common.Enum.NotificationChannel.Voice.ToString(),
                        Count = DeliveryReportVoicescount.Count(x=>x.Status != "failed")
                    };
               
                    NotificationChannels.Add(whatsApp);
                    NotificationChannels.Add(Text);
                    NotificationChannels.Add(Voice);

                var responseNotificationMode = _mapper.Map<List<NotificationChannelCount>>(NotificationChannels);
                return new Response { NotificationModeCount = responseNotificationMode };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }

        }
    }
}
