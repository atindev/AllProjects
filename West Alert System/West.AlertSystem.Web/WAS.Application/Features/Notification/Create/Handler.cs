using AutoMapper;
using Flurl.Http;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Extensions;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Notification.Create
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IServiceBase _serviceBase;
        private readonly WasApiSettings _wASApiSettings;
        private readonly IMapper _mapper;


        public Handler(
            ILogger<Handler> logger,
            IServiceBase serviceBase,
            IOptions<WasApiSettings> wASApiSettings,
            IMapper mapper
            )
        {
            _logger = logger;
            _serviceBase = serviceBase;
            _wASApiSettings = wASApiSettings.Value;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _serviceBase.GenerateAuthenticationTokenAsync();
                 
                if (request.EventId == Guid.Empty)
                {
                    var newEvent = _mapper.Map<Events.CreateUpdate.Request>(request);
                    newEvent.Name = request.NewEventName;
                    var eventResponse = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.CreateUpdateEvent}"
                    .WithOAuthBearerToken(token)
                    .PostJsonAsync(newEvent, cancellationToken);

                    if (!eventResponse.IsSuccessStatusCode)
                        return null;

                    var eventResult = await eventResponse.Content.ReadAsStringAsync();
                    var eventResponseContent = JsonConvert.DeserializeObject<Events.CreateUpdate.Response>(eventResult);
                    request.EventId = eventResponseContent.Id;
                }
 
                var notification = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.CreateNotification}"
                    .WithOAuthBearerToken(token)
                    .PostJsonAsync(request, cancellationToken);

                if (!notification.IsSuccessStatusCode)
                    return null;

                var result = await notification.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<Response>(result);

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
