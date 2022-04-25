using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using WAS.Domain.Entities;
using GroupGetById = WAS.Application.Features.Group.GetByIds;

namespace WAS.Functions
{
    public class SendVoiceNotification
    {
        private readonly IWasContextAdmin _context;
        private readonly ILogger<SendEmailNotification> _logger;
        private readonly IMediator _mediator;
        private readonly ITwilioService _twilioService;
        private readonly List<DeliveryReportVoice> deliveryReportVoices;

        public SendVoiceNotification(
            IWasContextAdmin context,
            ILogger<SendEmailNotification> logger,
            IMediator mediator,
            ITwilioService twilioService)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _twilioService = twilioService;
            deliveryReportVoices = new List<DeliveryReportVoice>();
        }

        [FunctionName("SendVoiceNotification")]
        public async Task Run([QueueTrigger("was-voice-notification", Connection = "AzureWebJobsStorage")] VoiceNotification voiceNotification, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                log.LogInformation($"C# Queue trigger SendVoiceNotification function started: {voiceNotification.Id}");

                var notification = await _context.Notifications
                    .Include(o => o.NotificationVoice)
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(s => s.Id == voiceNotification.Id);

                var groupDetails = await _mediator.Send(new GroupGetById.Request { Ids = voiceNotification.GroupIds, SubscriptionIds = voiceNotification.SubscriptionIds });

                if (notification != null && notification.NotificationVoice != null)
                {
                    var sender = _context.Subscriptions.FirstOrDefault(s => s.OfficialEmail == notification.CreatedBy);
                    var signature = notification.IsSignatureRequired ? $"sent by {sender.LastName} {sender.FirstName} from West Alert System" : "sent by Admin from West Alert System";

                    //If work mobile is enabled for notification
                    Parallel.ForEach(groupDetails?.Group.Where(sg => sg.SubscriberOfficialMobile != null && sg.IsVoiceOfficeMobile), item =>
                    {
                        SendTwilioVoiceNotification(notification, item.SubscriberOfficialMobile, item, signature);
                    });

                    //If personal mobile is enabled for notification
                    Parallel.ForEach(groupDetails?.Group.Where(sg => sg.SubscriberPersonalMobile != null && sg.IsVoicePersonalMobile), item =>
                    {
                        SendTwilioVoiceNotification(notification, item.SubscriberPersonalMobile, item, signature);
                    });

                    //If personal mobile is enabled for notification
                    Parallel.ForEach(groupDetails?.Group.Where(sg => sg.SubscriberOfficePhone != null && sg.IsVoiceOfficePhone), item =>
                    {
                        SendTwilioVoiceNotification(notification, item.SubscriberOfficePhone, item, signature);
                    });

                    //If personal mobile is enabled for notification
                    Parallel.ForEach(groupDetails?.Group.Where(sg => sg.SubscriberHomePhone != null && sg.IsVoiceHomePhone), item =>
                    {
                        SendTwilioVoiceNotification(notification, item.SubscriberHomePhone, item, signature);
                    });

                    await _context.DeliveryReportVoices.AddRangeAsync(deliveryReportVoices, cancellationToken);
                    notification.NotificationVoice.SentDate = DateTime.UtcNow;
                    notification.SentDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync(cancellationToken);
                }

                log.LogInformation($"C# Queue trigger function processed: {voiceNotification.Id}");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
            }
        }

        private void SendTwilioVoiceNotification(Notification notification, string mobile, Application.Common.Models.Group item, string signature)
        {
            string ErrorMessage = null;
            int? ErrorCode = null;
            try
            {
                var callResponse = _twilioService.SendVoice(new Application.Common.Models.TwilioVoice
                {
                    FromNumber = item.CountryPhoneNumber,
                    ToNumber = mobile,
                    Body = $"Hello {item.SubscriberFirstName}.\r\n\r\n This is a notification {signature}.\r\n\r\n {notification.NotificationVoice.Message}",
                    RepeatCount = notification.NotificationVoice.RepeatCount,
                    Language = item.PreferredLanguage
                });

                deliveryReportVoices.Add(new DeliveryReportVoice
                {
                    SubscriptionId = item.SubscriberId,
                    NotificationVoiceId = notification.NotificationVoice.Id,
                    CallId = callResponse.Result.Sid,
                    Status = callResponse.Status.ToString(),
                    ToNumber= mobile
                });
            }
            catch(Exception ex)
            {
                if (ex != null && ex.InnerException != null && ex.InnerException.Message != null && ex.InnerException.Message != "")
                    ErrorMessage = ex.InnerException.Message;
                if (ex != null && ex.InnerException != null)
                {
                    ErrorCode = ((Twilio.Exceptions.ApiException)ex.InnerException).Code;
                }

                deliveryReportVoices.Add(new DeliveryReportVoice
                {
                    SubscriptionId = item.SubscriberId,
                    NotificationVoiceId = notification.NotificationVoice.Id,
                    CallId = Convert.ToString(Guid.NewGuid()),
                    Status = "failed",
                    ToNumber = mobile,
                    ErrorMessage = ErrorMessage,
                    ErrorCode = (ErrorCode != null && ErrorCode != 0) ? ErrorCode : null
                });

                _logger.LogError(ex.Message, ex);
            }
        }
    }
    public class VoiceNotification
    {
        public Guid Id { get; set; }

        public List<int> GroupIds { get; set; }

        public List<Guid> SubscriptionIds { get; set; }
    }
}
