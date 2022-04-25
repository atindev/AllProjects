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
    public class ODataNotificationController : ODataController
    {
        private readonly ILogger<ODataNotificationController> _logger;
        private readonly IWasContext _context;

        public ODataNotificationController(
            IWasContext context,
            ILogger<ODataNotificationController> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ODataRoute("Notification")]
        [EnableQuery]
        public async Task<IEnumerable<Domain.Entities.Notification>> Index()
        {
            try
            {
                var notifications = await _context.Notifications
                    .Include(i => i.Event)
                    .Include(i => i.NotificationEmail)
                    .Include(i => i.NotificationVoice)
                    .Include(i => i.NotificationText)
                    .Include(i => i.NotificationWhatsApp)
                    .Include(i => i.NotificationType)
                    .Include(i => i.NotificationSubscriptions)
                        .ThenInclude(i => i.Subscription)
                    .IgnoreQueryFilters()
                    .ToListAsync();

                return notifications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}
