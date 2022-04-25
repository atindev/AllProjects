using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAS.Application.Interface;

namespace WAS.API.Controllers
{
    public class ODataDeliveryReportController : ODataController
    {
        private readonly ILogger<ODataDeliveryReportController> _logger;
        private readonly IWasContext _context;

        public ODataDeliveryReportController(
            IWasContext context,
            ILogger<ODataDeliveryReportController> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ODataRoute("DeliveryReportText")]
        [EnableQuery]
        public async Task<IEnumerable<Domain.Entities.DeliveryReportText>> DeliveryReportText()
        {
            try
            {
                var deliveryReportTexts = await _context.DeliveryReportTexts
                    .Include(i => i.NotificationText)
                        .ThenInclude(i => i.Notification)
                    .Include(i => i.Subscription)
                    .IgnoreQueryFilters()
                    .ToListAsync();

                return deliveryReportTexts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        [HttpGet]
        [ODataRoute("DeliveryReportVoice")]
        [EnableQuery]
        public async Task<IEnumerable<Domain.Entities.DeliveryReportVoice>> DeliveryReportVoice()
        {
            try
            {
                var deliveryReportVoices = await _context.DeliveryReportVoices
                    .Include(i => i.NotificationVoice)
                        .ThenInclude(i => i.Notification)
                    .Include(i => i.Subscription)
                    .IgnoreQueryFilters()
                    .ToListAsync();

                return deliveryReportVoices;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        [HttpGet]
        [ODataRoute("DeliveryReportWhatsApp")]
        [EnableQuery]
        public async Task<IEnumerable<Domain.Entities.DeliveryReportWhatsApp>> DeliveryReportWhatsApp()
        {
            try
            {
                var deliveryReportWhatsApps = await _context.DeliveryReportWhatsApps
                    .Include(i => i.NotificationWhatsApp)
                        .ThenInclude(i => i.Notification)
                    .Include(i => i.Subscription)
                    .IgnoreQueryFilters()
                    .ToListAsync();

                return deliveryReportWhatsApps;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}
