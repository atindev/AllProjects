using Flurl.Http;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Extensions;
using WAS.Application.Interface.Services;
using GetAllSubscriptions = WAS.Application.Features.Subscription.GetAll;

namespace WAS.Application.Features.Groups.GetByIds
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IServiceBase _serviceBase;
        private readonly WasApiSettings _wASApiSettings;
        private readonly IMediator _mediator;

        public Handler(
            ILogger<Handler> logger,
            IServiceBase serviceBase,
            IOptions<WasApiSettings> wASApiSettings,
              IMediator mediator
            )
        {
            _logger = logger;
            _serviceBase = serviceBase;
            _wASApiSettings = wASApiSettings.Value;
            _mediator = mediator;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _serviceBase.GenerateAuthenticationTokenAsync();

                var getAll = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetSubscriptionsByGroup}"
                    .WithOAuthBearerToken(token)
                    .PostJsonAsync(request, cancellationToken);

                if (!getAll.IsSuccessStatusCode)
                    return null;

                var result = await getAll.Content.ReadAsStringAsync();

                GetAllSubscriptions.Request peopleRequest = new GetAllSubscriptions.Request();
                var subscriptions = await _mediator.Send(peopleRequest);
                var response = JsonConvert.DeserializeObject<Response>(result);
                response.Subscriptions = subscriptions.Subscriptions;

                return response;
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
