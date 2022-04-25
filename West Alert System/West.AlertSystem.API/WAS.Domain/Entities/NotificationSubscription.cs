using System;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class NotificationSubscription : Entity
    {
        /// <summary>
        /// Notification Subscription unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Notification Id
        /// </summary>
        public Guid NotificationId { get; set; }

        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Associated Notification for this Subscription
        /// </summary>
        public virtual Notification Notification { get; set; }

        /// <summary>
        /// Associated Subscription for this Notification
        /// </summary>
        public virtual Subscription Subscription { get; set; }
    }
}
