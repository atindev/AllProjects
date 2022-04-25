using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAS.Application.Interface;
using GetAll = WAS.Application.Features.Subscription.GetAll;

namespace WAS.API.Controllers
{
    public class ODataSubscriptionController : ODataController
    {
        private readonly ILogger<ODataSubscriptionController> _logger;
        private readonly IWasContext _context;

        public ODataSubscriptionController(
            IWasContext context,
            ILogger<ODataSubscriptionController> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ODataRoute("Subscription")]
        [EnableQuery]
        public async Task<IEnumerable<Domain.Entities.Subscription>> Get()
        {
            try
            {
                var subscriptions = await _context.Subscriptions
                    .Include(i => i.SubscriptionGroups)
                    .Include(i => i.Department)
                    .Include(i => i.Shift)
                    .Include(i => i.Location)
                        .ThenInclude(i => i.City)
                            .ThenInclude(i => i.State)
                                .ThenInclude(i => i.Country)
                    .IgnoreQueryFilters()
                    .ToListAsync();

                return subscriptions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}
