using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Verification.Mail
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly ITwilioService _twilioService;
        private readonly IMediator _mediator;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            ITwilioService twilioService,
            IMediator mediator
            )
        {
            _context = context;
            _logger = logger;
            _twilioService = twilioService;
            _mediator = mediator;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var twilioVerificationResource = new TwilioVerificationResource
                {
                    Channel = "email",
                    To = request.Email,
                    Otp = request.Otp
                };

                if (request.Otp == null || request.Otp == "")
                {
                    var twilioVerificationResult = await _twilioService.SendOtpAsync(twilioVerificationResource);

                    if (twilioVerificationResult.IsValid)
                        return new Response { Success = true };
                    else
                        return new Response { Success = false };
                }
                else
                {
                    var twilioVerificationResult = await _twilioService.VerifyOtpAsync(twilioVerificationResource);

                    if (twilioVerificationResult.IsValid)
                        return new Response { Success = true };
                    else
                        return new Response { Success = false };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }

        }
    }
}
