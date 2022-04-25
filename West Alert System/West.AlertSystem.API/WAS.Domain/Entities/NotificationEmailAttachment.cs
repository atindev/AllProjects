using System;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class NotificationEmailAttachment : Entity
    {
        /// <summary>
        /// Notification Email Attachment unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Notification email unique Id
        /// </summary>
        public Guid NotificationEmailId { get; set; }

        /// <summary>
        /// File Name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Attachment Path
        /// </summary>
        public string Attachment { get; set; }

        /// <summary>
        /// Attachment Type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Associated NotificationEmail for this Attachment
        /// </summary>
        public virtual NotificationEmail NotificationEmail { get; set; }
    }
}
