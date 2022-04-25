using Flurl.Http;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Extensions;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.IncomingMessage.GetAudio
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
            try
            {
                var token = await _serviceBase.GenerateAuthenticationTokenAsync();

                var audio = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAudio}?Path={request.Path}"
                .WithOAuthBearerToken(token)
                .GetJsonAsync<Response>(cancellationToken);

                audio.ContentDisposition = new System.Net.Mime.ContentDisposition
                {
                    Inline = true
                };

                return audio;
            }
            catch (FlurlHttpException ex)
            {
                _logger.LogError(ex.Message, ex);
                var problemDetails = await ex.GetResponseJsonAsync<ProblemDetailsResponse>();

                if (problemDetails == null)
                    throw ProblemDetailsResponseExtensions.Exception(ex.Message);

                throw problemDetails.Exception();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }

        }
    }
}
