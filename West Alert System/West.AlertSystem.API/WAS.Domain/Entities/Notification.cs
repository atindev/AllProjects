using System;
using System.Collections.Generic;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class Notification : Entity
    {
        /// <summary>
        /// Notification Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Event id
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Notification type id
        /// </summary>
        public int NotificationTypeId { get; set; }

        /// <summary>
        /// Notification scheduled time
        /// </summary>
        public DateTime? ScheduledTime { get; set; }

        /// <summary>
        /// Notification scheduled TimeZone
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Notification scheduled TimeZone Offset
        /// </summary>
        public int TimeZoneOffset { get; set; }

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
        public Enum.Status Status { get; set; }

        /// <summary>
        /// Approved/Rejected by
        /// </summary>
        public string ApprovedBy { get; set; }

        /// <summary>
        /// Approved/Rejected date
        /// </summary>
        public DateTime? ApprovedDate { get; set; }

        /// <summary>
        /// Approved/Rejected by
        /// </summary>
        public string FinalApprovalBy { get; set; }

        /// <summary>
        /// Approved/Rejected date
        /// </summary>
        public DateTime? FinalApprovalDate { get; set; }

        /// <summary>
        /// Notification approve/reject comment by final approver
        /// </summary>
        public string FinalComment { get; set; }

        /// <summary>
        /// Notification approve/reject comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Notification Sent Date
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// Notification approved Timezone
        /// </summary>
        public string ApprovedTimeZone { get; set; }

        /// <summary>
        /// Notification Created Timezone
        /// </summary>
        public string CreatedTimeZone { get; set; }

        /// <summary>
        /// Notification second level approved Timezone
        /// </summary>
        public string FinalApprovalTimeZone { get; set; }

        /// <summary>
        /// Notification Modified Timezone
        /// </summary>
        public string ModifiedTimeZone { get; set; }

        /// <summary>
        /// Notification sent Timezone
        /// </summary>
        public string SentTimeZone { get; set; }

        /// <summary>
        /// Is Approval Required
        /// </summary>
        public bool? IsApprovalRequired { get; set; }

        /// <summary>
        /// private notification approver
        /// </summary>       
        public string ApproverForPrivate { get; set; }

        /// <summary>
        /// Is private notification
        /// </summary>
        public bool IsPrivateNotification { get; set; }

        /// <summary>
        /// Is creator Signature Required in Notification
        /// </summary>
        public bool IsSignatureRequired { get; set; }

        /// <summary>
        /// Email Notification 
        /// </summary>
        public virtual NotificationEmail NotificationEmail { get; set; }

        /// <summary>
        /// SMS Notification
        /// </summary>
        public virtual NotificationText NotificationText { get; set; }

        /// <summary>
        /// WhatsApp notification
        /// </summary>
        public virtual NotificationWhatsApp NotificationWhatsApp { get; set; }

        /// <summary>
        /// Voice Notification
        /// </summary>
        public virtual NotificationVoice NotificationVoice { get; set; }

        /// <summary>
        /// Event 
        /// </summary>
        public virtual Event Event { get; set; }

        /// <summary>
        /// Notification Groups
        /// </summary>
        public virtual ICollection<NotificationGroup> NotificationGroups { get; set; }

        /// <summary>
        /// Notification Subscriptions
        /// </summary>
        public virtual ICollection<NotificationSubscription> NotificationSubscriptions { get; set; }

        /// <summary>
        /// IncomingMessages for this Notification
        /// </summary>
        public virtual ICollection<IncomingMessage> IncomingMessages { get; set; }

        /// <summary>
        /// notification Type
        /// </summary>
        public virtual NotificationType NotificationType { get; set; }
    }
}
