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
    public class ODataGroupController : ODataController
    {
        private readonly ILogger<ODataGroupController> _logger;
        private readonly IWasContext _context;

        public ODataGroupController(
            IWasContext context,
            ILogger<ODataGroupController> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ODataRoute("Group")]
        [EnableQuery]
        public async Task<IEnumerable<Domain.Entities.Group>> Index()
        {
            try
            {
                var groups = await _context.Groups
                    .Include(i => i.SubscriptionGroups)
                        .ThenInclude(i=>i.Subscription)
                    .IgnoreQueryFilters()
                    .ToListAsync();

                return groups;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}
