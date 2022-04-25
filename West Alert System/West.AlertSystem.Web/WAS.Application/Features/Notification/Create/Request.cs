using MediatR;
using System;
using System.Collections.Generic;
using WAS.Application.Common.Enum;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Notification.Create
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Event id
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Notification type id
        /// </summary>
        public int NotificationTypeId { get; set; }

        /// <summary>
        /// Notification message body
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// Notification voice body
        /// </summary>
        public string VoiceMessage { get; set; }


        /// <summary>
        /// Notification Voice Repeat count
        /// </summary>
        public int VoiceRepeatCount { get; set; }

        /// <summary>
        /// Email subject
        /// </summary>
        public string EmailSubject { get; set; }

        /// <summary>
        /// Email body
        /// </summary>
        public string EmailBody { get; set; }

        ///// <summary>
        ///// Email attachments
        ///// </summary>
        public List<AttachmentData> EmailAttachments { get; set; }

        /// <summary>
        /// Notification scheduled time
        /// </summary>
        public DateTime? ScheduledTime { get; set; }

        /// <summary>
        /// Notification TimeZone
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Notification TimeZone Offset
        /// </summary>
        public int TimeZoneOffset { get; set; }

        /// <summary>
        /// Group Id
        /// </summary>
        public List<int> GroupId { get; set; }

        /// <summary>
        /// Subscription Id
        /// </summary>
        public List<Guid> SubscriptionId { get; set; }

        /// <summary>
        /// Is text(sms) notification
        /// </summary>
        public bool IsText { get; set; }

        /// <summary>
        /// Is email notification
        /// </summary>
        public bool IsEmail { get; set; }

        /// <summary>
        /// Is voice notification
        /// </summary>
        public bool IsVoice { get; set; }

        /// <summary>
        /// Is teams notification
        /// </summary>
        public bool IsTeams { get; set; }

        /// <summary>
        /// Is whatsapp notification
        /// </summary>
        public bool IsWhatsApp { get; set; }

        /// <summary>
        /// Notification status
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// User who created the record
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Notification Created Timezone
        /// </summary>
        public string CreatedTimeZone { get; set; }

        /// <summary>
        /// Notification whats app message body
        /// </summary>       
        public string WhatsAppMessage { get; set; }

        /// <summary>
        /// Is Approval Required
        /// </summary>
        public bool IsApprovalRequired { get; set; }

        /// <summary>
        /// selected email attachment
        /// </summary>
        public List<string> ExistingEmailAttachments { get; set; }

        /// <summary>
        /// EventName with timezone
        /// </summary>       
        public string NewEventName { get; set; }

        /// <summary>
        /// private notification approver
        /// </summary>       
        public string ApproverForPrivate { get; set; }

        /// <summary>
        /// Is private notification
        /// </summary>
        public bool IsPrivateNotification { get; set; }

        /// <summary>
        /// Notification type 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is creator Signature Required in Notification
        /// </summary>
        public bool IsSignatureRequired { get; set; }

    }
}
