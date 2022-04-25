using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;

namespace WAS.Application.Features.Subscription.SubscriptionFeedback
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
                if (request.SubscriptionId == null)
                {
                    var subscriptionEntity = await _context.Subscriptions
                                                .FirstOrDefaultAsync(i => i.Upn == request.Upn, cancellationToken);
                    request.SubscriptionId = subscriptionEntity.Id;
                    request.FeedbackChannel = request.PhoneNumber.Contains("whatsapp:") ? "WhatsApp" : "SMS";
                    request.Feedback = ((Domain.Enum.Feedback)Convert.ToInt32(request.Feedback)).ToString();
                }

                var subscriptionFeedbackEntity = _mapper.Map<Domain.Entities.SubscriptionFeedback>(request);
                
                await _context.SubscriptionFeedbacks.AddAsync(subscriptionFeedbackEntity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response { Success = true};
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }
    }
}
