using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WAS.Application.Common.Settings;
using Models = WAS.Application.Common.Models;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using WAS.Domain.Entities;
using System.Threading;
using System.Collections.Generic;

namespace WAS.Functions
{
    public class SendNotificationToQueue
    {
        private readonly IWasContextAdmin _context;
        private readonly IAzureStorageService _azureStorageService;
        private readonly AzureStorageSettings _azureStorageSettings;

        public SendNotificationToQueue(
            IWasContextAdmin context,
            IAzureStorageService azureStorageService,
            IOptions<AzureStorageSettings> azureStorageSettings)
        {
            _context = context;
            _azureStorageService = azureStorageService;
            _azureStorageSettings = azureStorageSettings.Value;
        }

        [FunctionName("SendNotificationToQueue")]
        public async Task Run([TimerTrigger("*/15 * * * * *")] TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
            try
            {
                var notifications = await _context.Notifications
                    .Include(t => t.NotificationText)
                    .Include(v => v.NotificationVoice)
                    .Include(e => e.NotificationEmail)
                    .Include(e => e.NotificationWhatsApp)
                    .Include(ng => ng.NotificationGroups)
                    .Include(ns => ns.NotificationSubscriptions)
                    .IgnoreQueryFilters()
                    .Where(s => (s.IsApprovalRequired==false || s.Status == Domain.Enum.Status.SecondLevelApproved || s.Status == Domain.Enum.Status.Failed) && s.ScheduledTime <= DateTime.UtcNow && s.IsActive)
                    .ToListAsync(cancellationToken);

                await SendNotificationAsync(notifications, cancellationToken);

                log.LogInformation($"C# Timer trigger function Completed at : {DateTime.UtcNow}");
            }
            catch(Exception ex)
            {
                log.LogError(ex.Message, ex);
            }
        }


        private async Task SendNotificationAsync(List<Domain.Entities.Notification> notifications, CancellationToken cancellationToken)
        {
            foreach (Notification notification in notifications)
            {
                if (notification.NotificationText != null && notification.NotificationText.IsActive)
                {
                    await _azureStorageService.AddMessageToStorageQueue(new Models.SmsNotification
                    {
                        Id = notification.Id,
                        GroupIds = notification.NotificationGroups?.Select(g => g.GroupId).ToList(),
                        SubscriptionIds = notification.NotificationSubscriptions?.Select(s=> s.SubscriptionId).ToList()
                    }, _azureStorageSettings.SmsNotificationQueue);

                    notification.NotificationText.IsActive = false;
                    await _context.SaveChangesAsync(cancellationToken);
                }

                if (notification.NotificationVoice != null && notification.NotificationVoice.IsActive)
                {
                    await _azureStorageService.AddMessageToStorageQueue(new Models.VoiceNotification
                    {
                        Id = notification.Id,
                        GroupIds = notification.NotificationGroups?.Select(g => g.GroupId).ToList(),
                        SubscriptionIds = notification.NotificationSubscriptions?.Select(s => s.SubscriptionId).ToList()
                    }, _azureStorageSettings.VoiceNotificationQueue);

                    notification.NotificationVoice.IsActive = false;
                    await _context.SaveChangesAsync(cancellationToken);
                }

                if (notification.NotificationEmail != null && notification.NotificationEmail.IsActive)
                {
                    await _azureStorageService.AddMessageToStorageQueue(new Models.EmailNotification
                    {
                        Id = notification.Id,
                        GroupIds = notification.NotificationGroups.Select(g => g.GroupId).ToList(),
                        SubscriptionIds = notification.NotificationSubscriptions.Select(s => s.SubscriptionId).ToList(),
                        EmailSendGridTemplateID = _azureStorageSettings.EmailSendGridTemplateID
                    }, _azureStorageSettings.EmailNotificationQueue);

                    notification.NotificationEmail.IsActive = false;

                    var notificatiomEmailAttachments = await _context.NotificationEmailAttachments
                        .IgnoreQueryFilters()
                        .Where(ea => ea.NotificationEmailId == notification.NotificationEmail.Id)
                        .ToListAsync(cancellationToken);
                    notificatiomEmailAttachments.ForEach(emailAttachment =>
                    {
                        emailAttachment.IsActive = false;
                    });

                    await _context.SaveChangesAsync(cancellationToken);
                }


                if (notification.NotificationWhatsApp != null && notification.NotificationWhatsApp.IsActive)
                {
                    await _azureStorageService.AddMessageToStorageQueue(new Models.WhatsAppNotification
                    {
                        Id = notification.Id,
                        GroupIds = notification.NotificationGroups.Select(g => g.GroupId).ToList(),
                        SubscriptionIds = notification.NotificationSubscriptions.Select(s => s.SubscriptionId).ToList()
                    }, _azureStorageSettings.WhatsAppNotificationQueue);

                    notification.NotificationWhatsApp.IsActive = false;
                    await _context.SaveChangesAsync(cancellationToken);
                }

                notification.Status = Domain.Enum.Status.Sent;
                notification.IsActive = false;

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

    }
}
