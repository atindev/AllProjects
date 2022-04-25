using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using System.Collections.Generic;
using System.Linq;

namespace WAS.Application.Features.Subscription.UnsubscribeMobile
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
                string mobileNumber = "";
                var response = new Response();
                if (request.MobileNumber != null && request.MobileNumber != "")
                        mobileNumber= request.MobileNumber.Replace("whatsapp:", "").Trim();

                var subscriptionEntity = await _context.Subscriptions
                                         .FirstOrDefaultAsync(x => x.OfficeMobile == mobileNumber || x.PersonalMobile == mobileNumber);
                    
                if (subscriptionEntity==null)
                    return response;

                response.SubscriberId = subscriptionEntity.Id;
                response.SubscriberFirstName = subscriptionEntity.FirstName;
                if (subscriptionEntity.PersonalMobile == mobileNumber)
                {
                    response.IsPersonalMobile = true;
                    subscriptionEntity.PersonalMobile = null;
                    subscriptionEntity.IsVoicePersonalMobile = false;
                    subscriptionEntity.IsTextPersonalMobile = false;
                    subscriptionEntity.IsWhatsAppPersonalMobile = false;
                    await _context.SaveChangesAsync(cancellationToken);
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
