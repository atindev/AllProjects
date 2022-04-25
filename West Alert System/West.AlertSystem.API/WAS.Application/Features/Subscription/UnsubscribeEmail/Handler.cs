using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using VerifyEmail = WAS.Application.Features.Verification.Mail;

namespace WAS.Application.Features.Subscription.UnsubscribeEmail
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMediator _mediator;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMediator mediator
            )
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var subscriptionEntity = new Domain.Entities.Subscription();

                if (request.RemovePersonalEmail == "on")
                {
                    subscriptionEntity = await _context.Subscriptions
                                         .SingleOrDefaultAsync(s => s.PersonalEmail == request.Email, cancellationToken);
                }
                else
                {
                    subscriptionEntity = await _context.Subscriptions
                                .SingleOrDefaultAsync(s => s.IsPersonalEmail && s.PersonalEmail == request.Email, cancellationToken);
                }

                if (subscriptionEntity == null)
                    return new Response { Success = false };

                var response = await _mediator.Send(new VerifyEmail.Request { Email = request.Email, Otp = request.Otp });

                if (response.Success && request.Otp != null && request.Otp != "")
                {
                    subscriptionEntity.IsPersonalEmail = false;
                    if (request.RemovePersonalEmail == "on")
                        subscriptionEntity.PersonalEmail = null;

                    await _context.SaveChangesAsync(cancellationToken);
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
