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

namespace WAS.Application.Features.Subscription.CreateView
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IServiceBase _serviceBase;
        private readonly WasApiSettings _wASApiSettings;
        private readonly Recaptcha _reCaptcha;

        public Handler(
            ILogger<Handler> logger,
            IServiceBase serviceBase,
            IOptions<WasApiSettings> wASApiSettings,
            IOptions<Recaptcha> reCaptcha
            )
        {
            _logger = logger;
            _serviceBase = serviceBase;
            _wASApiSettings = wASApiSettings.Value;
            _reCaptcha = reCaptcha.Value;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var response = new Response();
            var token = await _serviceBase.GenerateAuthenticationTokenAsync();
            response.Recaptcha = _reCaptcha;

            try
            {
                var locations = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllLocations}"
                    .WithOAuthBearerToken(token)
                    .GetJsonAsync<Response>(cancellationToken);

                var shifts = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllShifts}"
                  .WithOAuthBearerToken(token)
                  .GetJsonAsync<Response>(cancellationToken);

                var languages = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllLanguages}"
                        .WithOAuthBearerToken(token)
                        .GetJsonAsync<Response>(cancellationToken);

                response.Languages = languages.Languages;
                response.Locations = locations.Locations;
                response.Shifts = shifts.Shifts;
            }
            catch (FlurlHttpException ex)
            {
                _logger.LogError(ex.Message, ex);
                var problemDetails = await ex.GetResponseJsonAsync<ProblemDetailsResponse>();

                if (problemDetails == null)
                    throw ProblemDetailsResponseExtensions.Exception(ex.Message);

                throw problemDetails.Exception();
            }

            try
            {
                var subscription = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetByMailUnMasked}/{request.OfficialEmail}"
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
