using System;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class SubscriptionFeedback : Entity
    {
        /// <summary>
        /// Feedback Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Feedback Rating
        /// </summary>
        public string FeedbackRating { get; set; }

        /// <summary>
        /// Subscriber Id
        /// </summary>
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Feedback Channel
        /// </summary>
        public string FeedbackChannel { get; set; }

        /// <summary>
        /// Gets or sets the Subscription
        /// </summary>
        /// <value>
        /// Subscription.
        /// </value>
        public virtual Subscription Subscription { get; set; }
    }
}
