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
    public class SendBlockedUserNotification
    {
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly IAzureStorageService _azureStorageService;
        private readonly IEmailFormatService _emailFormatService;

        public SendBlockedUserNotification(
            IOptions<AzureStorageSettings> azureStorageSettings,
            IAzureStorageService azureStorageService,
            IEmailFormatService emailFormatService
            )
        {
            _azureStorageSettings = azureStorageSettings.Value;
            _azureStorageService = azureStorageService;
            _emailFormatService = emailFormatService;
        }

        [FunctionName("SendBlockedUserNotification")]
        public async Task Run([QueueTrigger("was-blockeduser-notification", Connection = "AzureWebJobsStorage")] Application.Common.Models.SubscriptionConfirmation subscriptionConfirmation, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                log.LogInformation($"Queue trigger SendBlockedUserNotification function started: {subscriptionConfirmation.Channel}-{subscriptionConfirmation.To}");

                var wasWebBaseUrl = Environment.GetEnvironmentVariable("WasUrl");

                var emailBody = await _emailFormatService.FormatEmail(new
                {
                    EmployeeName = subscriptionConfirmation.Name,
                    AttemptON= subscriptionConfirmation.AttemptON
                }, _azureStorageSettings.BlockedUserEmailTemplate);

                await _azureStorageService.AddMessageToStorageQueue(new
                {
                    Subject = "WAS | New Subscription Attempt",
                    MailBody = emailBody,
                    To = subscriptionConfirmation.To
                }, _azureStorageSettings.SendEmailQueue);

                log.LogInformation($"Queue trigger SendBlockedUserNotification function processed: {subscriptionConfirmation.Channel}-{subscriptionConfirmation.To}");
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message, ex);
            }
        }
    }
}
