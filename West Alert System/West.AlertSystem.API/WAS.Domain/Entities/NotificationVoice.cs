using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class NotificationVoice : Entity
    {
        /// <summary>
        /// Notification voice unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// voice body
        /// </summary>
        public string Message { get; set; }


        /// <summary>
        /// Voice Repeat count
        /// </summary>
        public int RepeatCount { get; set; }

        /// <summary>
        /// NotificationVoice Sent Date
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// Notification Id
        /// </summary>
        public Guid NotificationId { get; set; }

        /// <summary>
        /// Associated Notification for this voice
        /// </summary>
        public virtual Notification Notification { get; set; }

        /// <summary>
        /// Notification Voices Delivery Report
        /// </summary>
        public virtual ICollection<DeliveryReportVoice> DeliveryReportVoices { get; set; }
    }
}
