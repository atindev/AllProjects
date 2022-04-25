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
    public class AlertAdmin
    {
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly IAzureStorageService _azureStorageService;
        private readonly IEmailFormatService _emailFormatService;
        private readonly ITwilioService _twilioService;

        public AlertAdmin(
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

        [FunctionName("AlertAdmin")]
        public async Task Run([QueueTrigger("was-alert-admin", Connection = "AzureWebJobsStorage")] Application.Common.Models.AlertAdminWithMessage alertAdmin, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                log.LogInformation($"Queue trigger SendAlertToAdmin function started: {alertAdmin.Channel}-{alertAdmin.To}");

                
                var employeeFirstName = alertAdmin.FirstName;
                var senderFullName = alertAdmin.SenderEmployeeFullName;
                var message = alertAdmin.Message;
                var messageType = alertAdmin.MessageType;
                var projectName = "West Alert System";
                var reply = "respond";


                switch (alertAdmin.Channel)
                {
                    case "email":
                        var emailBody = await _emailFormatService.FormatEmail(new
                        {
                            EmployeeFirstName = alertAdmin.FirstName,
                            MessageType = alertAdmin.MessageType,
                            EmployeeFullName= alertAdmin.SenderEmployeeFullName,
                            Message=alertAdmin.Message,
                         
                        }, _azureStorageSettings.AlertAdminTemplate);

                        await _azureStorageService.AddMessageToStorageQueue(new
                        {
                            Subject = "WAS | Incoming Message Alert",
                            MailBody = emailBody,
                            To = alertAdmin.To
                        }, _azureStorageSettings.SendEmailQueue);

                        break;
                    case "sms":
                        await _twilioService.SendSMS(new Application.Common.Models.TwilioSms
                        {
                            FromNumber = alertAdmin.FromNumber,
                            Body = $"Hello {employeeFirstName},\nYou have received {messageType} from {senderFullName}\n\n{message}\n\nPlease logon to {projectName} portal to {reply}.",
                            ToNumber = alertAdmin.To
                        });
                        break;
                    case "whatsapp":
                        await _twilioService.SendWhatsAppMessage(new Application.Common.Models.TwilioSms
                        {
                            FromNumber = alertAdmin.FromNumber,
                            Body = $"Hello {employeeFirstName},\nYou have received *{messageType}* from *{senderFullName}*\n\n*{message}*\n\nPlease logon to {projectName} to {reply}.",
                            ToNumber = alertAdmin.To
                        });
                        break;
                    default:
                        break;
                }

                log.LogInformation($"Queue trigger SendAlertToAdmin function processed: {alertAdmin.Channel}-{alertAdmin.To}");
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message, ex);
            }
        }
    }
}
