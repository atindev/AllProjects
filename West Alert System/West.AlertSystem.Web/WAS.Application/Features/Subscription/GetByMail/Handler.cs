using AutoMapper;
using Flurl.Http;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Extensions;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Subscription.GetByMail
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IServiceBase _serviceBase;
        private readonly WasApiSettings _wASApiSettings;

        public Handler(
            ILogger<Handler> logger,
            IServiceBase serviceBase,
            IOptions<WasApiSettings> wASApiSettings
            )
        {
            _logger = logger;
            _serviceBase = serviceBase;
            _wASApiSettings = wASApiSettings.Value;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var response = new Response();
            var token = await _serviceBase.GenerateAuthenticationTokenAsync();
            try
            {
                var subscription = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetSubscriptionByMail}/{request.OfficialEmail}"
                        .WithOAuthBearerToken(token)
                        .GetJsonAsync<Common.Models.Subscription>(cancellationToken);

                response.Subscription = subscription;

                return response;
            }
            catch (FlurlHttpException ex)
            {
                _logger.LogError(ex.Message, ex);
                return response;
            }
        }
    }
}
