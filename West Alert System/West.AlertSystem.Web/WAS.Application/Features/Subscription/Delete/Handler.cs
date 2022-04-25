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
using WAS.Application.Common.Settings;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Subscription.Delete
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
            var token = await _serviceBase.GenerateAuthenticationTokenAsync();
            try
            {
                var subscription = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.DeleteSubscription}/{request.OfficialEmail}"
                     .WithOAuthBearerToken(token)
                     .DeleteAsync(cancellationToken);

                var result = await subscription.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<Response>(result);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}

