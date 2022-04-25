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

namespace WAS.Application.Features.Group.GetNotificationDistinctSubscriberCount
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
                var response = new Response();
                var subscribers = new List<Common.Models.SubscriptionDetails>();
                await AddDistinctCurrentlyActiveSubscriberGroups(request, subscribers, cancellationToken);
                await AddDistinctNotificationSubscriptions(request, subscribers, cancellationToken);
                subscribers = subscribers.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).Distinct().ToList();
                if (subscribers.Any())
                {
                    response.TotalSubscribers = subscribers.Count;
                    response.SMSCount = subscribers.Count(o => o.IsTextOfficeMobile || o.IsTextPersonalMobile);
                    response.EmailCount = subscribers.Count(o => o.IsOfficialEmail || o.IsPersonalEmail);
                    response.VoiceCount = subscribers.Count(o => o.IsVoiceHomePhone || o.IsVoiceOfficeMobile || o.IsVoiceOfficePhone || o.IsVoicePersonalMobile);
                    response.WhatsappCount = subscribers.Count(o => o.IsWhatsAppOfficeMobile || o.IsWhatsAppPersonalMobile);
                    response.PeopleId = subscribers.Select(x => x.Id).ToList();
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }

        private async Task AddDistinctCurrentlyActiveSubscriberGroups(Request request, List<SubscriptionDetails> subscribers, CancellationToken cancellationToken) 
        {
            if (request.Ids != null && request.Ids.Any())
            {
                var subscriptionGroups = await _context.SubscriptionGroups
                .Include(o => o.Subscription)
                .Include(o => o.Group)
                .Where(sg => request.Ids.Contains(sg.GroupId)
                && (sg.CreatedDate <= request.CreatedDate))
                .IgnoreQueryFilters()
                .ToListAsync(cancellationToken);
                subscriptionGroups = subscriptionGroups.Where(sg => (sg.IsActive && (sg.Subscription.IsActive || (!sg.Subscription.IsActive && sg.Subscription.DeletedDate > request.CreatedDate)))
                || (!sg.IsActive && sg.DeletedDate > request.CreatedDate)).ToList();

                if (subscriptionGroups.Any())
                {
                    var uniqueSubscribers = subscriptionGroups.GroupBy(x => x.SubscriptionId).Select(y => y.FirstOrDefault()).Distinct().ToList();
                    var responseSubscribers = _mapper.Map<List<Common.Models.SubscriptionDetails>>(uniqueSubscribers);
                    subscribers.AddRange(responseSubscribers);
                }
            }
        }
        private async Task AddDistinctNotificationSubscriptions(Request request, List<SubscriptionDetails> subscribers, CancellationToken cancellationToken)
        {
            if (request.SubscriptionIds != null && request.SubscriptionIds.Any())
            {
                var notificationSubscriptions = await _context.NotificationSubscriptions
                .Include(o => o.Subscription)
                    .ThenInclude(o => o.Location)
                .Include(o => o.Subscription)
                    .ThenInclude(o => o.Shift)
                .Where(ns => request.SubscriptionIds.Contains(ns.SubscriptionId) && (ns.CreatedDate <= request.CreatedDate)).IgnoreQueryFilters().ToListAsync(cancellationToken);
                if (notificationSubscriptions.Any())
                {
                    notificationSubscriptions = notificationSubscriptions.GroupBy(x => x.SubscriptionId).Select(y => y.FirstOrDefault()).Distinct().ToList();
                    var responseSubscribers = _mapper.Map<List<Common.Models.SubscriptionDetails>>(notificationSubscriptions);
                    subscribers.AddRange(responseSubscribers);
                }
            }
        }

    }
}
