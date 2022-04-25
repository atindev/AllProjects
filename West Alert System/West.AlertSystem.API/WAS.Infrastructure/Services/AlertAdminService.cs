using Microsoft.Extensions.Options;
using ObjectsComparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Interface.Services;

namespace WAS.Infrastructure.Services
{
    public class AlertAdminService : IAlertAdminService
    {
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly IAzureStorageService _azureStorageService;

        public AlertAdminService(
            IOptions<AzureStorageSettings> azureStorageSettings,
            IAzureStorageService azureStorageService
            )
        {
            _azureStorageSettings = azureStorageSettings.Value;
            _azureStorageService = azureStorageService;
        }

        public async Task SendIncomingMessage(AlertAdmin subscriptions)
        {
            foreach (var adminsubscription in subscriptions.AdminSubscription)
            {
                if (adminsubscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.OfficialEmail.ToString())
                {
                    await AddIncomingMessageToQueue(adminsubscription, adminsubscription.OfficialEmail, "email",subscriptions.IncomingMessage, subscriptions.SenderFullName);
                }
                else if (adminsubscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.PersonalEmail.ToString())
                {
                    await AddIncomingMessageToQueue(adminsubscription, adminsubscription.PersonalEmail, "email", subscriptions.IncomingMessage, subscriptions.SenderFullName);
                }
                else if (adminsubscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.TextOfficeMobile.ToString())
                {
                    await AddIncomingMessageToQueue(adminsubscription, adminsubscription.OfficeMobile, "sms", subscriptions.IncomingMessage,subscriptions.SenderFullName);
                }
                else if (adminsubscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.TextPersonalMobile.ToString())
                {
                    await AddIncomingMessageToQueue(adminsubscription, adminsubscription.PersonalMobile, "sms", subscriptions.IncomingMessage,subscriptions.SenderFullName);
                }
                else if (adminsubscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.WhatsAppOfficeMobile.ToString())
                {
                    await AddIncomingMessageToQueue(adminsubscription, adminsubscription.OfficeMobile, "whatsapp", subscriptions.IncomingMessage,subscriptions.SenderFullName);
                }
                else if (adminsubscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.WhatsAppPersonalMobile.ToString())
                {
                    await AddIncomingMessageToQueue(adminsubscription, adminsubscription.PersonalMobile, "whatsapp", subscriptions.IncomingMessage,subscriptions.SenderFullName);
                }
                else
                {
                    await AddIncomingMessageToQueue(adminsubscription, adminsubscription.OfficialEmail, "email", subscriptions.IncomingMessage,subscriptions.SenderFullName);
                }
            }
        }
    

    private async Task AddIncomingMessageToQueue(Domain.Entities.Subscription subscription, string addressData,string channel, Domain.Entities.IncomingMessage incomingMessage,string senderName)
    {

        await _azureStorageService.AddMessageToStorageQueue(new Application.Common.Models.AlertAdminWithMessage
        {
            FromNumber = subscription.Location.CountryPhoneNumber,
            FirstName = subscription.FirstName,
            To = addressData,
            Channel = channel,
            SenderEmployeeFullName = senderName,
            Message = incomingMessage.IsText || incomingMessage.IsWhatsApp ? incomingMessage.Message : incomingMessage.TwilioVoiceMailUrl,
            MessageType = incomingMessage.IsText || incomingMessage.IsWhatsApp ? "an incoming message" : "an incoming voice mail",
        }, _azureStorageSettings.AdminAlertQueue);
    }

    }
}
