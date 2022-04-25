using System;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class DeliveryReportVoice : Entity
    {
        /// <summary>
        /// DeliveryReportVoice unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Notification Voice Id
        /// </summary>
        public Guid NotificationVoiceId { get; set; }

        /// <summary>
        /// Call Id
        /// </summary>
        public string CallId { get; set; }

        /// <summary>
        /// Delivery Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Error Code
        /// </summary>
        public int? ErrorCode { get; set; }

        /// <summary>
        /// Sending Number
        /// </summary>
        public string ToNumber { get; set; }

        /// <summary>
        /// Associated Notification Voice for this Call
        /// </summary>
        public virtual NotificationVoice NotificationVoice { get; set; }

        /// <summary>
        /// Subscription
        /// </summary>
        public virtual Subscription Subscription { get; set; }
    }
}
