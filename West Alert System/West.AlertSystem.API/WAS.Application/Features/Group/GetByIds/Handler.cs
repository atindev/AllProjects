using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;

namespace WAS.Application.Features.Group.GetByIds
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContextAdmin _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;

        public Handler(
            IWasContextAdmin context,
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
                List<Common.Models.Group> responseGroup = new List<Common.Models.Group>();
                var subscriptionGroups = new List<Domain.Entities.SubscriptionGroup>();
                if (request.Ids != null && request.Ids.Any())
                {
                    if (request.IsArchiveGroupRequired)
                    {
                          subscriptionGroups = await _context.SubscriptionGroups
                                                .Include(o => o.Subscription)
                                                    .ThenInclude(o => o.Location)
                                                .Include(o => o.Subscription)
                                                    .ThenInclude(o => o.Shift)
                                                .Include(o => o.Group)
                                                   .IgnoreQueryFilters()
                                                .Where(sg => (request.Ids.Contains(sg.GroupId)) && sg.IsActive && sg.Subscription.IsActive)
                                                .ToListAsync(cancellationToken);
                    }
                    else
                    {
                         subscriptionGroups = await _context.SubscriptionGroups
                                            .Include(o => o.Subscription)
                                                .ThenInclude(o => o.Location)
                                            .Include(o => o.Subscription)
                                                .ThenInclude(o => o.Shift)
                                            .Include(o => o.Group)
                                            .Where(sg => (request.Ids.Contains(sg.GroupId)))
                                            .ToListAsync(cancellationToken);
                    }
                     
                    if (subscriptionGroups.Any())
                    {
                        var uniqueSubscribers = subscriptionGroups.GroupBy(x => x.SubscriptionId).Select(y => y.FirstOrDefault()).Distinct().ToList();
                        responseGroup = _mapper.Map<List<Common.Models.Group>>(uniqueSubscribers);
                    }
                }

                if (request.SubscriptionIds != null && request.SubscriptionIds.Any())
                {
                    var notificationSubscriptions = await _context.NotificationSubscriptions
                    .Include(o => o.Subscription)
                        .ThenInclude(o => o.Location)
                    .Include(o => o.Subscription)
                        .ThenInclude(o => o.Shift)
                    .Where(ns => request.SubscriptionIds.Contains(ns.SubscriptionId))
                    .ToListAsync(cancellationToken);

                    if (notificationSubscriptions.Any())
                    {
                        var responseSubscription = _mapper.Map<List<Common.Models.Group>>(notificationSubscriptions);
                        responseGroup.AddRange(responseSubscription);
                        responseGroup = responseGroup.GroupBy(x => x.SubscriberId).Select(y => y.FirstOrDefault()).Distinct().ToList();
                    }
                }

                return new Response { Group = responseGroup };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }
    }
}
