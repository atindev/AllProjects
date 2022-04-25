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
    public class ODataEventController : ODataController
    {
        private readonly ILogger<ODataEventController> _logger;
        private readonly IWasContext _context;

        public ODataEventController(
            IWasContext context,
            ILogger<ODataEventController> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ODataRoute("Event")]
        [EnableQuery]
        public async Task<IEnumerable<Domain.Entities.Event>> Index()
        {
            try
            {
                var events = await _context.Events
                    .Include(i => i.Notifications)
                    .IgnoreQueryFilters()
                    .ToListAsync();

                return events;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}
