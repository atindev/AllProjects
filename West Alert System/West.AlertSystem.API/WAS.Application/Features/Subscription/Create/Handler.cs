using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Settings;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using ObjectsComparer;
using System.Linq;

namespace WAS.Application.Features.Subscription.Create
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly ISubscriptionConfirmationService _subscriptionConfirmationService;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            ISubscriptionConfirmationService subscriptionConfirmationService
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _subscriptionConfirmationService = subscriptionConfirmationService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var subscriptionEntity = await _context.Subscriptions
                     .FirstOrDefaultAsync(o => o.OfficialEmail == request.OfficialEmail, cancellationToken);

                if (request.Id == Guid.Empty)
                {
                    if (subscriptionEntity != null)
                        return new Response { Success = true };

                    var subscription = _mapper.Map<Domain.Entities.Subscription>(request);
                    await _context.Subscriptions.AddAsync(subscription, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    await _subscriptionConfirmationService.SendSubscriptionConfirmation(subscription);
                }
                else
                {
                    if (subscriptionEntity == null)
                        throw new BadRequestException("Subscription not found");

                    _mapper.Map(request, subscriptionEntity);

                    await _context.SaveChangesAsync(cancellationToken);
                    await _subscriptionConfirmationService.SendSubscriptionConfirmation(subscriptionEntity);
                }

                return new Response { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }
    }
}

