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

namespace WAS.Application.Features.Report.GetAllSubscriptionCountPerDay
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
                    subscriptionData = await _context.Subscriptions.Where(x => x.LocationId == request.LocationId).IgnoreQueryFilters().ToListAsync(cancellationToken);
                }
                else
                {
                    subscriptionData = await _context.Subscriptions.IgnoreQueryFilters().ToListAsync(cancellationToken);
                }
                DateTime earlietSubscriptiondate = _context.Subscriptions.IgnoreQueryFilters().Select(x => x.CreatedDate).Min(x => x.Date);
                DateTime startDate = earlietSubscriptiondate;
                DateTime endDate = DateTime.UtcNow;
                List<SubscriberAndUnsubscriberCountPerDay> subscriberAndUnsubscriberCountPerDays = new List<SubscriberAndUnsubscriberCountPerDay>();

                for (DateTime date = startDate; date <= endDate ; date = date.AddDays(1))
                {
                    SubscriberAndUnsubscriberCountPerDay subscribersAndUnsubscribersCountPerDay = new SubscriberAndUnsubscriberCountPerDay()
                    {
                        Date = date,
                        SubscriberCountPerDay = subscriptionData.Count(x=>x.CreatedDate.Date == date),
                        UnsubscriberCountPerDay = subscriptionData.Count(x=>x.DeletedDate?.Date == date)
                    };
                    if(subscribersAndUnsubscribersCountPerDay.SubscriberCountPerDay==0 && subscribersAndUnsubscribersCountPerDay.UnsubscriberCountPerDay == 0)
                    {
                        continue;
                    }
                    subscriberAndUnsubscriberCountPerDays.Add(subscribersAndUnsubscribersCountPerDay);
                };
                var responseSubscriberandUnsubscriberPerDay = _mapper.Map<List<SubscriberAndUnsubscriberCountPerDay>>(subscriberAndUnsubscriberCountPerDays);
                return new Response {  subscriberAndUnsubscriberCountPerDays = responseSubscriberandUnsubscriberPerDay };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }

        }
    }
}
