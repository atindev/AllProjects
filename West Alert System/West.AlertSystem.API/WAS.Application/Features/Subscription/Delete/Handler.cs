using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;

namespace WAS.Application.Features.Subscription.Delete
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContextAdmin _context;
        private readonly ILogger<Handler> _logger;      

        public Handler(
            IWasContextAdmin context,
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
                var subscriptionEntity = await _context.Subscriptions
                    .SingleOrDefaultAsync(s => s.OfficialEmail == request.OfficialEmail, cancellationToken);

                if (subscriptionEntity == null)
                    throw new BadRequestException($"There is no subscription found with email {request.OfficialEmail}");

                if(request.IsDeleteRequestFromSubscriber)
                     _context.Subscriptions.Remove(subscriptionEntity);
                else
                {
                    subscriptionEntity.IsActive = false;
                    subscriptionEntity.DeletedDate = DateTime.UtcNow;
                    _context.Subscriptions.Attach(subscriptionEntity).State = EntityState.Modified;
                }

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

