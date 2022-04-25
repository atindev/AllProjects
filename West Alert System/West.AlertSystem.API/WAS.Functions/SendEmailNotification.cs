using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using WAS.Domain.Entities;
using GroupGetById = WAS.Application.Features.Group.GetByIds;

namespace WAS.Functions
{
    public class SendEmailNotification
    {
        private readonly IWasContextAdmin _context;
        private readonly ILogger<SendEmailNotification> _logger;
        private readonly IMediator _mediator;
        private readonly IAzureStorageService _azureStorageService;
        private readonly IEmailFormatService _emailFormatService;
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly string followupEmailMessage;

        public SendEmailNotification(
            IWasContextAdmin context,
            ILogger<SendEmailNotification> logger,
            IMediator mediator,
            IAzureStorageService azureStorageService,
            IEmailFormatService emailFormatService,
            IOptions<AzureStorageSettings> azureStorageSettings)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _azureStorageService = azureStorageService;
            _emailFormatService = emailFormatService;
            _azureStorageSettings = azureStorageSettings.Value;
            followupEmailMessage = "<em>Admin is expecting a response to the above notification. Please respond</em>";
        }

        [FunctionName("SendEmailNotification")]
        public async Task Run([QueueTrigger("was-email-notification", Connection = "AzureWebJobsStorage")] EmailNotification emailNotification, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                log.LogInformation($"C# Queue trigger SendEmailNotification function started: {emailNotification.Id}");

                var notification = await _context.Notifications
                    .Include(o => o.NotificationEmail)
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(s => s.Id == emailNotification.Id);

                var groupDetails = await _mediator.Send(new GroupGetById.Request { Ids = emailNotification.GroupIds, SubscriptionIds = emailNotification.SubscriptionIds });

                if (notification != null && notification.NotificationEmail != null)
                {
                    var sender = _context.Subscriptions.FirstOrDefault(s => s.OfficialEmail == notification.CreatedBy);
                    var signature = notification.IsSignatureRequired ? $"Sent by <b>{sender.LastName}, {sender.FirstName}</b> from <b>West Alert System</b>" : "Sent by <b>Admin</b> from <b>West Alert System</b>";

                    var notificationEmailAttachments = await _context.NotificationEmailAttachments
                        .IgnoreQueryFilters()
                        .Where(ea => ea.NotificationEmailId == notification.NotificationEmail.Id)
                        .ToListAsync(cancellationToken);

                    List<EmailAttachment> emailAttachments = new List<EmailAttachment>();
                    foreach (var emailAttachment in notificationEmailAttachments)
                    {
                        var url = new Uri(emailAttachment.Attachment);

                        emailAttachments.Add(new EmailAttachment
                        {
                            BlobPath = url.AbsolutePath,
                            FileName = emailAttachment.FileName
                        });
                    }

                    SendEmail(
                            groupDetails,
                            notification.NotificationEmail,
                            emailAttachments,
                            notification.NotificationTypeId,
                            signature);

                    notification.NotificationEmail.SentDate = DateTime.UtcNow;
                    notification.SentDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync(cancellationToken);
                }

                log.LogInformation($"C# Queue trigger SendEmailNotification function processed: {emailNotification.Id}");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
            }
        }

        private void SendEmail(
                            GroupGetById.Response groupDetails,
                            NotificationEmail notificationEmail,
                            List<EmailAttachment> emailAttachments,
                            int notificationTypeId,
                            string signature
                            )
        {
            var wasWebBaseUrl = Environment.GetEnvironmentVariable("WasUrl");

            //If work email is enabled for notification
            Parallel.ForEach(groupDetails?.Group.Where(sg => sg.IsOfficialEmail), async item =>
            {
                try
                {
                    var emailBody = await _emailFormatService.FormatEmail(new
                    {
                        EmployeeName = item.SubscriberFirstName,
                        NotificationMessage = notificationTypeId == 2 ? notificationEmail.Body + "<br/><br/>" + followupEmailMessage : notificationEmail.Body,
                        ViewNotificationUrl = $"{wasWebBaseUrl}/Notification/EmailView?NotificationId={notificationEmail.NotificationId}",
                        Signature = signature
                    }, _azureStorageSettings.WorkEmailTemplate);

                    await _azureStorageService.AddMessageToStorageQueue(new
                    {
                        Subject = notificationEmail.Subject,
                        MailBody = emailBody,
                        To = item.SubscriberOfficialEmail,
                        Attachments = emailAttachments
                    }, _azureStorageSettings.SendEmailQueue);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
            });

            //If personal email is enabled for notification
            Parallel.ForEach(groupDetails?.Group.Where(sg => sg.IsPersonalEmail), async item =>
            {
                try
                {
                    var emailBody = await _emailFormatService.FormatEmail(new
                    {
                        EmployeeName = item.SubscriberFirstName,
                        NotificationMessage = notificationTypeId == 2 ? notificationEmail.Body + "<br/><br/>" + followupEmailMessage : notificationEmail.Body,
                        ViewNotificationUrl = $"{wasWebBaseUrl}/Notification/EmailView?NotificationId={notificationEmail.NotificationId}",
                        Signature = signature,
                        UnsubscriberEmail = $"{wasWebBaseUrl}/Unsubscription/Email?unsubscriberemail={item.SubscriberPersonalEmail}"
                    }, _azureStorageSettings.PersonalEmailTemplate);

                    await _azureStorageService.AddMessageToStorageQueue(new
                    {
                        Subject = notificationEmail.Subject,
                        MailBody = emailBody,
                        To = item.SubscriberPersonalEmail,
                        Attachments = emailAttachments
                    }, _azureStorageSettings.SendEmailQueue);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
            });
        }
    }

    public class EmailNotification
    {
        public Guid Id { get; set; }

        public List<int> GroupIds { get; set; }

        public List<Guid> SubscriptionIds { get; set; }

        public List<DistributionGroup> DistributionGroups { get; set; }

        public List<ADPeople> ADPeople { get; set; }

        public string EmailSendGridTemplateID { get; set; }

        public bool IsFollowUpEmail { get; set; }
    }

    public class EmailAttachment
    {
        public string FileName { get; set; }

        public string BlobPath { get; set; }
    }
}
