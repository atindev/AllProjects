using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;

namespace WAS.Application.Features.Group.GetGroupSubscriberCount
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new Response();
                var subscriptionGroups = await _context.SubscriptionGroups
                                        .Include(o => o.Subscription)
                                            .ThenInclude(o => o.Location)
                                        .Include(o => o.Subscription)
                                            .ThenInclude(o => o.Shift)
                                        .Include(o => o.Group)
                                        .IgnoreQueryFilters()
                                        .Where(sg => sg.GroupId == request.GroupId && sg.CreatedDate <= request.CreatedDate).ToListAsync(cancellationToken);
                
                subscriptionGroups = subscriptionGroups.Where(sg => (sg.IsActive && (sg.Subscription.IsActive || (!sg.Subscription.IsActive && sg.Subscription.DeletedDate > request.CreatedDate)))
                                        || (!sg.IsActive && sg.DeletedDate > request.CreatedDate)).ToList();

                if (subscriptionGroups == null)
                    return new Response();

                response.GroupName = subscriptionGroups.Select(x => x.Group.Name).Distinct().SingleOrDefault();
                response.GroupSubscriberCount = subscriptionGroups.Count;
                response.TextSubscribersCount = subscriptionGroups.Count(i => (i.Subscription.IsTextOfficeMobile || i.Subscription.IsTextPersonalMobile));
                response.VoiceSubscribersCount = subscriptionGroups.Count(i => (i.Subscription.IsVoiceHomePhone || i.Subscription.IsVoiceOfficeMobile || i.Subscription.IsVoiceOfficePhone || i.Subscription.IsVoicePersonalMobile));
                response.EmailSubscribersCount = subscriptionGroups.Count(i => (i.Subscription.IsOfficialEmail || i.Subscription.IsPersonalEmail));
                response.WhatsAppSubscribersCount = subscriptionGroups.Count(i => (i.Subscription.IsWhatsAppOfficeMobile || i.Subscription.IsWhatsAppPersonalMobile));

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
