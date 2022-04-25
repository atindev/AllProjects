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
using GetAllGroups = WAS.Application.Features.Groups.GetAll;
using GetAllSubscriptions = WAS.Application.Features.Subscription.GetAll;

namespace WAS.Application.Features.Notification.CreateView
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IServiceBase _serviceBase;
        private readonly WasApiSettings _wASApiSettings;
        private readonly FileProperties _fileProperties;
        private readonly IMediator _mediator;
        private readonly AzureConfig _azureConfig;

        public Handler(
            ILogger<Handler> logger,
            IServiceBase serviceBase,
            IOptions<WasApiSettings> wASApiSettings,
            IOptions<FileProperties> fileProperties,
            IMediator mediator,
            IOptions<AzureConfig> azureConfig
            )
        {
            _logger = logger;
            _serviceBase = serviceBase;
            _wASApiSettings = wASApiSettings.Value;
            _fileProperties = fileProperties.Value;
            _mediator = mediator;
            _azureConfig = azureConfig.Value;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _serviceBase.GenerateAuthenticationTokenAsync();

                var activeEvents = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetActiveEvent}"
                    .WithOAuthBearerToken(token)
                    .GetJsonAsync<Response>(cancellationToken);

                var activeLanguages = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllLanguages}"
                    .WithOAuthBearerToken(token)
                    .GetJsonAsync<Response>(cancellationToken);

                var whatsAppTemplates = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetWhatsAppTemplates}"
                    .WithOAuthBearerToken(token)
                    .GetJsonAsync<Response>(cancellationToken);

                var TemplatesList = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllTemplates}"
                    .WithOAuthBearerToken(token)
                    .GetJsonAsync<Response>(cancellationToken);

                var TemplatesCategoryList = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllCategories}"
                    .WithOAuthBearerToken(token)
                    .GetJsonAsync<Response>(cancellationToken);

                GetAllSubscriptions.Request peopleRequest = new GetAllSubscriptions.Request();
                var subscriptions = await _mediator.Send(peopleRequest);

                GetAllGroups.Request PagedRequest = new GetAllGroups.Request();
                PagedRequest.IsAccessRequired = true;
                PagedRequest.Email = request.Email;
                PagedRequest.IsGlobalAdmin = request.IsGlobalAdmin;
                var groups = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllGroups}"
                    .WithOAuthBearerToken(token)
                    .PostJsonAsync(PagedRequest, cancellationToken);

                if (!groups.IsSuccessStatusCode)
                    return null;

                var groupResult = await groups.Content.ReadAsStringAsync();
                var groupResponse = JsonConvert.DeserializeObject<GroupList>(groupResult);

                return new Response
                {
                    Groups = groupResponse.Groups,
                    Events = activeEvents.Events,
                    Languages = activeLanguages.Languages,
                    FileProperties = _fileProperties,
                    WhatsAppTemplates = whatsAppTemplates.WhatsAppTemplates,
                    Templates = TemplatesList.Templates,
                    TemplateCategories = TemplatesCategoryList.TemplateCategories,
                    Subscriptions = subscriptions.Subscriptions,
                    EnableApprover = _azureConfig.EnableApprover == "true"
                };
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
