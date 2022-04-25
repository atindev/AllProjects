using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using AutoMapper;
using System.Collections.Generic;

namespace WAS.Application.Features.Group.GetActiveDistinctSubscriberCount
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

                if (request.Ids != null && request.Ids.Any())
                {
                    var subscriptionGroups = await _context.SubscriptionGroups
                    .Include(o => o.Subscription)
                    .Include(o => o.Group)
                    .Where(sg => request.Ids.Contains(sg.GroupId)
                    && (sg.CreatedDate <= request.CreatedDate))
                    .ToListAsync(cancellationToken);

                    if (subscriptionGroups.Any())
                    {
                        var uniqueSubscribers = subscriptionGroups.GroupBy(x => x.SubscriptionId).Select(y => y.FirstOrDefault()).Distinct().ToList();
                        var responseSubscribers = _mapper.Map<List<Common.Models.SubscriptionDetails>>(uniqueSubscribers);
                        subscribers.AddRange(responseSubscribers);
                    }
                }

                if (request.SubscriptionIds != null && request.SubscriptionIds.Any())
                {

                    var subscriberData = await _context.Subscriptions.Where(x => request.SubscriptionIds.Contains(x.Id)).ToListAsync(cancellationToken);

                    if (subscriberData.Any())
                    {

                        var responseSubscribers = _mapper.Map<List<Common.Models.SubscriptionDetails>>(subscriberData);
                        subscribers.AddRange(responseSubscribers);
                        subscribers = subscribers.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).Distinct().ToList();
                    }
                }

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
    }
}
