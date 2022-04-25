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
    public class NotificationApprovalAlert : INotificationApprovalAlert
    {
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly IAzureStorageService _azureStorageService;
        private readonly IEmailFormatService _emailFormatService;
        private readonly ITwilioService _twilioService;
        
        public NotificationApprovalAlert(
            IOptions<AzureStorageSettings> azureStorageSettings,
            IAzureStorageService azureStorageService,
            IEmailFormatService emailFormatService,
            ITwilioService twilioService
            )
        {
            _azureStorageSettings = azureStorageSettings.Value;
            _azureStorageService = azureStorageService;
            _emailFormatService = emailFormatService;
            _twilioService = twilioService;
        }
        public async  Task NotificationApproval(NotificationApproval notificationApproval)
        {
            foreach (var subscription in notificationApproval.ApproverSubscription)
            {
                if (subscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.OfficialEmail.ToString())
                {
                    await SendNotificationApproval(subscription, subscription.OfficialEmail, "email", notificationApproval.SenderFullName);
                }
                else if (subscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.PersonalEmail.ToString())
                {
                    await SendNotificationApproval(subscription, subscription.PersonalEmail, "email", notificationApproval.SenderFullName);
                }
                else if (subscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.TextOfficeMobile.ToString())
                {
                    await SendNotificationApproval(subscription, subscription.OfficeMobile, "sms", notificationApproval.SenderFullName);
                }
                else if (subscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.TextPersonalMobile.ToString())
                {
                    await SendNotificationApproval(subscription, subscription.PersonalMobile, "sms", notificationApproval.SenderFullName);
                }
                else if (subscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.WhatsAppOfficeMobile.ToString())
                {
                    await SendNotificationApproval(subscription, subscription.OfficeMobile, "whatsapp", notificationApproval.SenderFullName);
                }
                else if (subscription.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.WhatsAppPersonalMobile.ToString())
                {
                    await SendNotificationApproval(subscription, subscription.PersonalMobile, "whatsapp", notificationApproval.SenderFullName);
                }
                else
                {
                    await SendNotificationApproval(subscription, subscription.OfficialEmail, "email", notificationApproval.SenderFullName);
                }
            }
        }
        public async Task SendNotificationApproval(Domain.Entities.Subscription subscription, string addresData, string channel,string senderName)
        {
            var projectName = "West Alert System";
            var employeeFirstName = subscription.FirstName;
            var senderEmployee = senderName;
            switch (channel)
            {
                case "email":
                   
                        var emailBody = await _emailFormatService.FormatEmail(new
                        {
                            EmployeeFirstName = subscription.FirstName,
                            SenderName = senderName
                        }, _azureStorageSettings.NotificationApprovalAlertTemplate);

                        await _azureStorageService.AddMessageToStorageQueue(new
                        {
                            Subject = "WAS | Notification Approval Alert",
                            MailBody = emailBody,
                            To = addresData
                        }, _azureStorageSettings.SendEmailQueue);
                    
                    break;

                case "sms":
                    await _twilioService.SendSMS(new Application.Common.Models.TwilioSms
                    {
                        FromNumber = subscription.Location.CountryPhoneNumber,
                        Body = $"Hello {employeeFirstName},\nYou have received a notification from {senderEmployee} for approval.\nPlease logon to {projectName} portal to review.",
                        ToNumber = addresData
                    });
                    break;
                case "whatsapp":
                    await _twilioService.SendWhatsAppMessage(new Application.Common.Models.TwilioSms
                    {
                        FromNumber = subscription.Location.CountryPhoneNumber,
                        Body = $"Hello {employeeFirstName},\nYou have received a notification from *{senderEmployee}* for approval.\n\nPlease logon to {projectName} to review.",
                        ToNumber = addresData
                    });
                    break;
                default:
                    break;
            }
        }

    }
}
