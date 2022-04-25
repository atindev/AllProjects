using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Interface;

namespace WAS.Application.Features.Report.GetAllSubscriptionPerMonth
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
                if (request.LocationId!=0)
                {
                    subscriptionData = await _context.Subscriptions.Where(x => x.LocationId == request.LocationId).ToListAsync(cancellationToken);
                }
                else
                {
                    subscriptionData = await _context.Subscriptions.ToListAsync(cancellationToken);
                }
                var SubscriptionPerMonth = subscriptionData.Select(x => x.CreatedDate.Month);
                var SubscriptionPerMonthdictionary = SubscriptionPerMonth.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
                List<SubscriptionPerMonth> SubscriptionPerMonths = new List<SubscriptionPerMonth>();


                foreach (var item in SubscriptionPerMonthdictionary)
                {

                    SubscriptionPerMonth allsubscriptions = new SubscriptionPerMonth()
                    {

                        MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Key),
                        SubscriptionCount = item.Value
                    };

                    SubscriptionPerMonths.Add(allsubscriptions);
                }



                var responseSubscriptionPerMonth = _mapper.Map<List<SubscriptionPerMonth>>(SubscriptionPerMonths);
                return new Response { SubscriptionPerMonths = responseSubscriptionPerMonth };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }

        }
    }
}
