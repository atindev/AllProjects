using Microsoft.Extensions.Options;
using ObjectsComparer;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WAS.Application.Common.Settings;
using WAS.Application.Interface.Services;

namespace WAS.Infrastructure.Services
{
    public class SubscriptionConfirmationService : ISubscriptionConfirmationService
    {
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly IAzureStorageService _azureStorageService;

        public SubscriptionConfirmationService(
            IOptions<AzureStorageSettings> azureStorageSettings,
            IAzureStorageService azureStorageService
            )
        {
            _azureStorageSettings = azureStorageSettings.Value;
            _azureStorageService = azureStorageService;
        }

        public async Task SendSubscriptionConfirmation(Domain.Entities.Subscription subscription)
        {
            if (subscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.OfficialEmail.ToString())
            {
                await AddSubscriptionConfirmationMessageToQueue(subscription,subscription.OfficialEmail,"email");
            }
            else if (subscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.PersonalEmail.ToString())
            {
                await AddSubscriptionConfirmationMessageToQueue(subscription,subscription.PersonalEmail,"email");
            }
            else if (subscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.TextOfficeMobile.ToString())
            {
                await AddSubscriptionConfirmationMessageToQueue(subscription,subscription.OfficeMobile,"sms");
            }
            else if (subscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.TextPersonalMobile.ToString())
            {
                await AddSubscriptionConfirmationMessageToQueue(subscription, subscription.PersonalMobile,"sms");
            }
            else if (subscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.WhatsAppOfficeMobile.ToString())
            {
                await AddSubscriptionConfirmationMessageToQueue(subscription,subscription.OfficeMobile,"whatsapp");
            }
            else if (subscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.WhatsAppPersonalMobile.ToString())
            {
                await AddSubscriptionConfirmationMessageToQueue(subscription,subscription.PersonalMobile,"whatsapp");
            }
        }
    

    private async Task AddSubscriptionConfirmationMessageToQueue(Domain.Entities.Subscription subscription, string addressData,string channel)
    {

        await _azureStorageService.AddMessageToStorageQueue(new Application.Common.Models.SubscriptionConfirmation
        {
            FromNumber = subscription.Location.CountryPhoneNumber,
            Name = subscription.FirstName,
            To = addressData,
            Channel = channel,
        }, _azureStorageSettings.SubscriptionConfirmationQueue);
    }

    }
}
