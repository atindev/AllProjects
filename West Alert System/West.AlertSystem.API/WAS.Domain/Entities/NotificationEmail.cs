using System;
using System.Collections.Generic;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class NotificationEmail : Entity
    {
        /// <summary>
        /// Notification email unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Email subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Email body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// NotificationEmail Sent Date
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// Notification Id
        /// </summary>
        public Guid NotificationId { get; set; }

        /// <summary>
        /// Associated Notification for this mail
        /// </summary>
        public virtual Notification Notification { get; set; }

        /// <summary>
        /// Notification Email Attachments
        /// </summary>
        public virtual ICollection<NotificationEmailAttachment> NotificationEmailAttachments { get; set; }
    }
}
