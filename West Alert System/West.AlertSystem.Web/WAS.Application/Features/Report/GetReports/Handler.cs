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

namespace WAS.Application.Features.Report.GetReports
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

                var getreport = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetReports}/{request.LocationId}"
                    .WithOAuthBearerToken(token)
                    .GetJsonAsync<Response>(cancellationToken);

                var locations = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllLocations}"
                    .WithOAuthBearerToken(token)
                    .GetJsonAsync<Response>(cancellationToken);

                return new Response
                {
                    SubscriptionPerMonths = getreport.SubscriptionPerMonths,
                    SubscriptionModeCount = getreport.SubscriptionModeCount,
                    NotificationPerMonths = getreport.NotificationPerMonths,
                    AllGroups = getreport.AllGroups,
                    NotificationChannels = getreport.NotificationChannels,
                    FeedbackChannels = getreport.FeedbackChannels,
                    SubscriptionLocationCounts =getreport.SubscriptionLocationCounts,
                    SubscriberAndUnsubscriberCountPerDays =getreport.SubscriberAndUnsubscriberCountPerDays,
                    SubscriptionLocationPercentage =getreport.SubscriptionLocationPercentage,
                    Locations = locations.Locations,
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
