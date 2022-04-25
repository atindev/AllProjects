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

namespace WAS.Application.Features.Report.GetAllSubscriptionModeCount
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
                    subscriptionData = await _context.Subscriptions.Where(x=>x.LocationId == request.LocationId).ToListAsync(cancellationToken);
                }
                else
                {
                    subscriptionData = await _context.Subscriptions.ToListAsync(cancellationToken);
                }
                var SubscriptionModes = subscriptionData.Select(x=>x.SubscriptionMode).ToList();
                SubscriptionModes.RemoveAll(x=>x == null);
                var subscriptionModedictionary = SubscriptionModes.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
                List<SubscriptionMode> SubscriptionModesList = new List<SubscriptionMode>();

                foreach (var item in subscriptionModedictionary)
                {
                    SubscriptionMode subscriptionmodes = new SubscriptionMode()
                    {
                        ModeOfSubscription = item.Key,
                        Count = item.Value
                    };

                    SubscriptionModesList.Add(subscriptionmodes);
                }
                var responseSubscriptionMode = _mapper.Map<List<SubscriptionMode>>(SubscriptionModesList);
                return new Response { SubscriptionModeCount = responseSubscriptionMode };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }

        }
    }
}
