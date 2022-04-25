using System;
using System.Collections.Generic;
using WAS.Application.Common.Enum;

namespace WAS.Application.Common.Models
{
    public class Notification
    {
        /// <summary>
        /// Notification unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Notification type id
        /// </summary>
        public int NotificationTypeId { get; set; }

        /// <summary>
        /// Notification Topic
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Record created date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// User who created the notification
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Notification approved/rejected date
        /// </summary>
        public DateTime? ApprovedDate { get; set; }

        /// <summary>
        /// User who approved the notification
        /// </summary>
        public string ApprovedBy { get; set; }

        /// <summary>
        /// Approved/Rejected by
        /// </summary>
        public string FinalApprovalBy { get; set; }

        /// <summary>
        /// Approved/Rejected date
        /// </summary>
        public DateTime? FinalApprovalDate { get; set; }

        /// <summary>
        /// Notification sent date
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// Last modified date
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// User who last updated the notification
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Notification text message
        /// </summary>
        public string TextMessage { get; set; }

        /// <summary>
        /// Notification voice message
        /// </summary>
        public string VoiceMessage { get; set; }

        /// <summary>
        /// Notification Voice Repeat count
        /// </summary>
        public int VoiceRepeatCount { get; set; }

        /// <summary>
        /// Notification email message
        /// </summary>
        public string EmailMessage { get; set; }

        /// <summary>
        /// Email subject
        /// </summary>
        public string EmailSubject { get; set; }

        /// <summary>
        /// Email body
        /// </summary>
        public string EmailBody { get; set; }

        /// <summary>
        /// Email Attachments
        /// </summary>
        public List<AttachmentData> EmailAttachments { get; set; }

        /// <summary>
        /// Notification scheduled time
        /// </summary>
        public DateTime? ScheduledTime { get; set; }

        /// <summary>
        /// Group Id
        /// </summary>
        public List<Guid> GroupId { get; set; }

        /// <summary>
        /// Group names
        /// </summary>
        public List<string> GroupNames { get; set; }

        /// <summary>
        /// Group names
        /// </summary>
        public List<GroupSubscribers> GroupSubscribers { get; set; }

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
        /// Event id
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Event name
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// Event status
        /// </summary>
        public string EventStatus { get; set; }

        /// <summary>
        /// Event type
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// Event urgency
        /// </summary>
        public string EventUrgency { get; set; }

        /// <summary>
        /// Event description
        /// </summary>
        public string EventDescription { get; set; }

        /// <summary>
        /// Record last updated
        /// </summary>
        public string Updated { get; set; }

        /// <summary>
        /// Approver comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// SMS sent to number of people
        /// </summary>
        public int TextSentToCount { get; set; }

        /// <summary>
        /// voice sent to number of people
        /// </summary>
        public int VoiceSentToCount { get; set; }

        /// <summary>
        /// email sent to number of people
        /// </summary>
        public int EmailSentToCount { get; set; }

        /// <summary>
        /// WhatsApp message sent to  number of people
        /// </summary>
        public int WhatsAppSentToCount { get; set; }

        /// <summary>
        /// current status of the notification
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Notification Created Timezone
        /// </summary>
        public string CreatedTimeZone { get; set; }

        /// <summary>
        /// Notification scheduled TimeZone
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Notification approved Timezone
        /// </summary>
        public string ApprovedTimeZone { get; set; }

        /// <summary>
        /// Notification second level approved Timezone
        /// </summary>
        public string FinalApprovalTimeZone { get; set; }

        /// <summary>
        /// Is private notification
        /// </summary>
        public bool IsPrivateNotification { get; set; }

        /// <summary>
        /// Notification Text Delivery Status
        /// </summary>
        public List<DeliveryStatus> DeliveryStatusText { get; set; }

        /// <summary>
        /// Notification Voice Delivery Status
        /// </summary>
        public List<DeliveryStatus> DeliveryStatusVoice { get; set; }

        /// <summary>
        /// Notification WhatsApp Delivery Status
        /// </summary>
        public List<DeliveryStatus> DeliveryStatusWhatsApp { get; set; }

        /// <summary>
        /// Event created date
        /// </summary>
        public DateTime EventCreatedDate { get; set; }

        /// <summary>
        /// Notification whatsapp message
        /// </summary>
        public string WhatsAppMessage { get; set; }

        /// <summary>
        /// Is Approval Required
        /// </summary>
        public bool? IsApprovalRequired { get; set; }

        /// <summary>
        /// Incoming Messages
        /// </summary>
        public List<IncomingMessage> IncomingMessages { get; set; }

        /// <summary>
        /// Notification response count
        /// </summary>
        public int ResponseCount { get; set; }

        /// <summary>
        /// Delivered Subscriber Names
        /// </summary>
        public List<string> SubscriberNames { get; set; }

        /// <summary>
        /// private notification approver
        /// </summary>       
        public string ApproverForPrivate { get; set; }

        /// <summary>
        /// Status updated text
        /// </summary>
        public string StatusUpdated { get; set; }

        /// <summary>
        /// Creater location
        /// </summary>
        public string CreaterLocation { get; set; }

        /// <summary>
        /// private notification approver location
        /// </summary>
        public string PrivateApproverLocation { get; set; }

        /// <summary>
        /// first level approver location
        /// </summary>
        public string FirstLevelApproverLocation { get; set; }

        /// <summary>
        /// second level approver location
        /// </summary>
        public string SecondLevelApproverLocation { get; set; }
    }
}
