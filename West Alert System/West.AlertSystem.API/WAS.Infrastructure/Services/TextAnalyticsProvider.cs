using Microsoft.Extensions.Options;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using WAS.Application.Common.Models;
using WAS.Application.Interface.Services;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.Cosmos;
using WAS.Application.Common.Settings;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using System;
using Azure;

namespace WAS.Infrastructure.Services
{
    public class TextAnalyticsProvider : ITextAnalyticsProvider
    {
        private readonly IOptions<TextAnalyticsSettings> _AnalyticsSettings;
            
        public TextAnalyticsProvider(IOptions<TextAnalyticsSettings> options)
        {
            _AnalyticsSettings = options;
        }
        public TextAnalyticsClient GetTextAnalyticsClientProvider()
        {
            var credentials = new ApiKeyServiceClientCredentials(_AnalyticsSettings.Value.SubscriptionKey);
            return new TextAnalyticsClient(credentials)
            {
                Endpoint = _AnalyticsSettings.Value.EndPoint
            };

        }
        public Azure.AI.TextAnalytics.TextAnalyticsClient GetAITextAnalyticsClientProvider()
        {
           return new Azure.AI.TextAnalytics.TextAnalyticsClient(new Uri(_AnalyticsSettings.Value.EndPoint), new AzureKeyCredential(_AnalyticsSettings.Value.SubscriptionKey));
        }

    }
}
