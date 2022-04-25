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
    public class GroupChangeNotificationService : IGroupChangeNotificationService
    {
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly IAzureStorageService _azureStorageService;
        private readonly IEmailFormatService _emailFormatService;
        private readonly ITwilioService _twilioService;
        
        public GroupChangeNotificationService(
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

        public async Task SendGroupChangeMessage(GroupChangeNotification groupChangeNotification, string addresData, string channel, string groupName, string AdminName,string whatsappBody,string smsBody)
        {
            switch (channel)
            {
                case "email":
                    if (groupChangeNotification.Action == "add")
                    {
                        var emailBody = await _emailFormatService.FormatEmail(new
                        {
                            EmployeeFirstName = groupChangeNotification.Subscription.FirstName,
                            GroupName = groupName,
                            AdminName = AdminName

                        }, _azureStorageSettings.GroupChangeNotificationTemplate);

                        await _azureStorageService.AddMessageToStorageQueue(new
                        {
                            Subject = "WAS | Group Change Alert",
                            MailBody = emailBody,
                            To = addresData
                        }, _azureStorageSettings.SendEmailQueue);
                    }
                    else
                    {
                        var emailBody = await _emailFormatService.FormatEmail(new
                        {
                            EmployeeFirstName = groupChangeNotification.Subscription.FirstName,
                            GroupName = groupName,
                            AdminName = AdminName

                        }, _azureStorageSettings.GroupRemovalNotificationTemplate);

                        await _azureStorageService.AddMessageToStorageQueue(new
                        {
                            Subject = "WAS | Group Change Alert",
                            MailBody = emailBody,
                            To = addresData
                        }, _azureStorageSettings.SendEmailQueue);
                    }
                    break;
                case "sms":
                    await _twilioService.SendSMS(new Application.Common.Models.TwilioSms
                    {
                        FromNumber = groupChangeNotification.Subscription.Location.CountryPhoneNumber,
                        Body = smsBody,
                        ToNumber = addresData
                    });
                    break;
                case "whatsapp":
                    await _twilioService.SendWhatsAppMessage(new Application.Common.Models.TwilioSms
                    {
                        FromNumber = groupChangeNotification.Subscription.Location.CountryPhoneNumber,
                        Body = whatsappBody,
                        ToNumber = addresData
                    });
                    break;
                default:
                    break;
            }
        }

    }
}
