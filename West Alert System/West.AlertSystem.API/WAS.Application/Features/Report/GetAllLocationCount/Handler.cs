using AutoMapper;
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

namespace WAS.Application.Features.Report.GetAllLocationCount
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
                List<Domain.Entities.Subscription> subscriptionData;
                if (request.LocationId != 0)
                {
                    subscriptionData = await _context.Subscriptions.Include(x=>x.Location).Where(x => x.LocationId == request.LocationId).ToListAsync(cancellationToken);
                }
                else
                {
                    subscriptionData = await _context.Subscriptions.Include(x => x.Location).ToListAsync(cancellationToken);
                }
                var locationcount = subscriptionData.Select(x => x.Location.Name).ToList();
                                        
                var subscriptionlocationdictionary = locationcount.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
                List<SubscriptionLocationCount> SubscriptionLocationList = new List<SubscriptionLocationCount>();

                foreach (var item in subscriptionlocationdictionary)
                {
                    SubscriptionLocationCount locationCount = new SubscriptionLocationCount()
                    {
                       LocationCount = item.Value,
                       LocationName = item.Key
                    };

                    SubscriptionLocationList.Add(locationCount);
                }
                var responseSubscriptionMode = _mapper.Map<List<SubscriptionLocationCount>>(SubscriptionLocationList);
                return new Response {  SubscriptionLocationCounts = responseSubscriptionMode };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }
    }
}
