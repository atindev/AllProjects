using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WAS.Application.Common.Settings;
using WAS.Application.Interface.Services;

namespace WAS.Functions
{
    public class SendSubscriptionConfirmation
    {
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly IAzureStorageService _azureStorageService;
        private readonly IEmailFormatService _emailFormatService;
        private readonly ITwilioService _twilioService;

        public SendSubscriptionConfirmation(
            IOptions<AzureStorageSettings> azureStorageSettings,
            IAzureStorageService azureStorageService,
            IEmailFormatService emailFormatService,
            ITwilioService twilioService)
        {
            _azureStorageSettings = azureStorageSettings.Value;
            _azureStorageService = azureStorageService;
            _emailFormatService = emailFormatService;
            _twilioService = twilioService;
        }

        [FunctionName("SendSubscriptionConfirmation")]
        public async Task Run([QueueTrigger("was-subscription-confirmation", Connection = "AzureWebJobsStorage")] Application.Common.Models.SubscriptionConfirmation subscriptionConfirmation, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                log.LogInformation($"Queue trigger SendSubscriptionConfirmation function started: {subscriptionConfirmation.Channel}-{subscriptionConfirmation.To}");

                var wasWebBaseUrl = Environment.GetEnvironmentVariable("WasUrl");

                switch (subscriptionConfirmation.Channel)
                {
                    case "email":
                        var emailBody = await _emailFormatService.FormatEmail(new
                        {
                            EmployeeName = subscriptionConfirmation.Name,
                        }, _azureStorageSettings.SubscriptionConfirmationTemplate);

                        await _azureStorageService.AddMessageToStorageQueue(new
                        {
                            Subject = "WAS | Subscription Confirmation",
                            MailBody = emailBody,
                            To = subscriptionConfirmation.To
                        }, _azureStorageSettings.SendEmailQueue);

                        break;
                    case "sms":
                        await _twilioService.SendSMS(new Application.Common.Models.TwilioSms
                        {
                            FromNumber = subscriptionConfirmation.FromNumber,
                            Body = $"Thank you for subscribing to West Alert System(WAS). You will receive Alerts/Notifications and Survey Requests from WAS Administrators when you're part of the target audience.\n\nReply STOP to unsubscribe anytime. We recommend having your subscription enabled on at least one of the channels (SMS/WhatsApp/Email).\n\nPlease keep your profile and contact information up to date on this link {wasWebBaseUrl}/Subscription/Subscribe to not miss any important notifications.",
                            ToNumber = subscriptionConfirmation.To
                        });
                        break;
                    case "whatsapp":
                        await _twilioService.SendWhatsAppMessage(new Application.Common.Models.TwilioSms
                        {
                            FromNumber = subscriptionConfirmation.FromNumber,
                            Body = $"Thank you for subscribing to *West Alert System(WAS)*. You will receive Alerts/Notifications and Survey Requests from WAS Administrators when you're part of the target audience.\n\nReply *STOP* to unsubscribe anytime. We recommend having your subscription enabled on at least one of the channels (SMS/WhatsApp/Email).\n\nPlease keep your profile and contact information up to date on this link {wasWebBaseUrl}/Subscription/Subscribe to not miss any important notifications.",
                            ToNumber = subscriptionConfirmation.To
                        });
                        break;
                    default:
                        break;
                }

                log.LogInformation($"Queue trigger SendSubscriptionConfirmation function processed: {subscriptionConfirmation.Channel}-{subscriptionConfirmation.To}");
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message, ex);
            }
        }
    }
}
