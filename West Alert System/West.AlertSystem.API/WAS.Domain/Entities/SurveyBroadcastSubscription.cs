using System;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class SurveyBroadcastSubscription : Entity
    {
        /// <summary>
        /// Survey Subscription unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// SurveyBroadcast Id
        /// </summary>
        public Guid SurveyBroadcastId { get; set; }

        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Associated Survey for this Subscription
        /// </summary>
        public virtual SurveyBroadcast SurveyBroadcast { get; set; }

        /// <summary>
        /// Associated Subscription for this Survey
        /// </summary>
        public virtual Subscription Subscription { get; set; }
    }
}
