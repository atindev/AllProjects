using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;

namespace WAS.Application.Features.Subscription.DiscardOcrSubscription
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
                var OcrSubscriptionEntity = await _context.OcrSubscriptions
                    .SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

                if (OcrSubscriptionEntity == null)
                    throw new BadRequestException($"There is no subscription found with id {request.Id}");

                _context.OcrSubscriptions.Remove(OcrSubscriptionEntity);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }
    }
}

