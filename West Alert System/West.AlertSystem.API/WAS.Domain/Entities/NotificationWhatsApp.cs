using System;
using System.Collections.Generic;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class NotificationWhatsApp : Entity
    {
        /// <summary>
        /// NotificationText unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Email body
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// NotificationText Sent Date
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// Notification Id
        /// </summary>
        public Guid NotificationId { get; set; }

        /// <summary>
        /// Associated Notification for this SMS
        /// </summary>
        public virtual Notification Notification { get; set; }

        /// <summary>
        /// Notification whatsapp Delivery Report
        /// </summary>
        public virtual ICollection<DeliveryReportWhatsApp> DeliveryReportWhatsApps { get; set; }
    }
}
