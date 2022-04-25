using System;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class DeliveryReportText : Entity
    {
        /// <summary>
        /// DeliveryReportSms unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Notification Text Id
        /// </summary>
        public Guid NotificationTextId { get; set; }

        /// <summary>
        /// SMS Id
        /// </summary>
        public string SmsId { get; set; }

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
        /// Associated Notification Text for this SMS
        /// </summary>
        public virtual NotificationText NotificationText { get; set; }

        /// <summary>
        /// Subscription
        /// </summary>
        public virtual Subscription Subscription { get; set; }

    }
}
