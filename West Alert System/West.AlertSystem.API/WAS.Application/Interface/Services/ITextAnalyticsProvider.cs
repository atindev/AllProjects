using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.Cosmos;
using Microsoft.Graph.Auth;
using System.Threading.Tasks;

namespace WAS.Application.Interface.Services
{
    public interface ITextAnalyticsProvider
    {
        TextAnalyticsClient GetTextAnalyticsClientProvider();

        Azure.AI.TextAnalytics.TextAnalyticsClient GetAITextAnalyticsClientProvider();
    }
}
