using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using WAS.Domain.Entities;
using GroupGetById = WAS.Application.Features.Group.GetByIds;

namespace WAS.Functions
{
    public class SendTextNotification
    {
        private readonly IWasContextAdmin _context;
        private readonly IMediator _mediator;
        private readonly ITwilioService _twilioService;
        private readonly List<DeliveryReportText> deliveryReportTexts;

        public SendTextNotification(
            IWasContextAdmin context,
            IMediator mediator,
            ITwilioService twilioService)
        {
            _context = context;
            _mediator = mediator;
            _twilioService = twilioService;
            deliveryReportTexts = new List<DeliveryReportText>();
        }

        [FunctionName("SendTextNotification")]
        public async Task Run([QueueTrigger("was-text-notification", Connection = "AzureWebJobsStorage")] TextNotification textNotification, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                log.LogInformation("C# Queue trigger SendTextNotification function started " + textNotification.Id);

                var notification = await _context.Notifications
                    .Include(o => o.NotificationText)
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(s => s.Id == textNotification.Id);

                var groupDetails = await _mediator.Send(new GroupGetById.Request { Ids = textNotification.GroupIds, SubscriptionIds = textNotification.SubscriptionIds });

                if (notification != null && notification.NotificationText != null)
                {
                    var sender = _context.Subscriptions.FirstOrDefault(s => s.OfficialEmail == notification.CreatedBy);
                    var signature = notification.IsSignatureRequired ? $"{sender.LastName}, {sender.FirstName} from West Alert System" : "Admin from West Alert System";

                    Parallel.ForEach(groupDetails?.Group.Where(sg => sg.SubscriberOfficialMobile != null && sg.IsTextOfficeMobile), item =>
                    {
                        SendTwilioTextNotification(notification, item, log, item.SubscriberOfficialMobile, signature);
                    });

                    Parallel.ForEach(groupDetails?.Group.Where(sg => sg.SubscriberPersonalMobile != null && sg.IsTextPersonalMobile), item =>
                    {
                        SendTwilioTextNotification(notification, item, log, item.SubscriberPersonalMobile, signature);
                    });

                    if (notification.NotificationTypeId == 2 && notification.IsText)
                    {
                        Parallel.ForEach(groupDetails?.Group.Where(sg => sg.SubscriberOfficialMobile != null && sg.IsTextOfficeMobile), item =>
                        {
                            _twilioService.SendSMS(new Application.Common.Models.TwilioSms
                            {
                                FromNumber = item.CountryPhoneNumber,
                                Body = $"Hello {item.SubscriberFirstName}, \nAdmin is expecting a response to the above notification. Please respond in the below format.\n\nNR-<your message>",
                                ToNumber = item.SubscriberOfficialMobile
                            });
                        });

                        Parallel.ForEach(groupDetails?.Group.Where(sg => sg.SubscriberPersonalMobile != null && sg.IsTextPersonalMobile), item =>
                        {
                            _twilioService.SendSMS(new Application.Common.Models.TwilioSms
                            {
                                FromNumber = item.CountryPhoneNumber,
                                Body = $"Hello {item.SubscriberFirstName}, \nAdmin is expecting a response to the above notification. Please respond in the below format.\n\nNR-<your message>",
                                ToNumber = item.SubscriberPersonalMobile
                            });
                        });
                    }

                    await _context.DeliveryReportTexts.AddRangeAsync(deliveryReportTexts, cancellationToken);
                    notification.NotificationText.SentDate = DateTime.UtcNow;
                    notification.SentDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync(cancellationToken);
                }

                log.LogInformation($"C# Queue trigger function processed: {textNotification.Id}");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
            }
        }

        private void SendTwilioTextNotification(Notification notification, Application.Common.Models.Group item, ILogger log, string mobileNumber, string signature)
        {
            string ErrorMessage = null;
            int? ErrorCode = null;
            try
            {
                var messageResponse = _twilioService.SendSMS(new Application.Common.Models.TwilioSms
                {
                    FromNumber = item.CountryPhoneNumber,
                    Body = $"Hello {item.SubscriberFirstName},\n\n{notification.NotificationText.Message.Trim()}\n\nSent by {signature}.",
                    ToNumber = mobileNumber
                });

                deliveryReportTexts.Add(new DeliveryReportText
                {
                    SubscriptionId = item.SubscriberId,
                    NotificationTextId = notification.NotificationText.Id,
                    SmsId = messageResponse.Result.Sid,
                    Status = messageResponse.Status.ToString(),
                    ErrorMessage = (messageResponse.Result.ErrorMessage != null) ? messageResponse.Result.ErrorMessage : null,
                    ErrorCode = (messageResponse.Result.ErrorCode != null && messageResponse.Result.ErrorCode != 0) ? messageResponse.Result.ErrorCode : null,
                    ToNumber = mobileNumber
                });
            }
            catch (Exception ex)
            {
                if (ex != null && ex.InnerException != null && ex.InnerException.Message != null && ex.InnerException.Message != "")
                    ErrorMessage = ex.InnerException.Message;
                if (ex != null && ex.InnerException != null)
                {
                    ErrorCode = ((Twilio.Exceptions.ApiException)ex.InnerException).Code;
                }
                deliveryReportTexts.Add(new DeliveryReportText
                {
                    SubscriptionId = item.SubscriberId,
                    NotificationTextId = notification.NotificationText.Id,
                    SmsId = Convert.ToString(Guid.NewGuid()),
                    Status = "failed",
                    ToNumber = mobileNumber,
                    ErrorMessage = ErrorMessage,
                    ErrorCode = (ErrorCode != null && ErrorCode != 0) ? ErrorCode : null
                });
                log.LogError(ex.Message, ex);
            }
        }
    }
    public class TextNotification
    {
        public Guid Id { get; set; }

        public List<int> GroupIds { get; set; }

        public List<Guid> SubscriptionIds { get; set; }
    }
}
