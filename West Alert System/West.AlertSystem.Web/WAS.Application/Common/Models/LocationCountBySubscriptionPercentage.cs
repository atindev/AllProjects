using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class LocationCountBySubscriptionPercentage
    {
        /// <summary>
        /// Gets or sets the subscription percentages.
        /// </summary>
        /// <value>
        /// The subscription percentages.
        /// </value>
        public List<SubscriptionPercentage> SubscriptionPercentages { get; set; } = new List<SubscriptionPercentage>();

        /// <summary>
        /// Gets or sets the remaining subscription percentages.
        /// </summary>
        /// <value>
        /// The remaining subscription percentages.
        /// </value>
        public List<RemainingSubscriptionPercentage> RemainingSubscriptionPercentages { get; set; } = new List<RemainingSubscriptionPercentage>();

        /// <summary>
        /// Gets or sets the global subscription.
        /// </summary>
        /// <value>
        /// The global subscription.
        /// </value>
        public int GlobalSubscription { get; set; }

        /// <summary>
        /// Gets or sets the global subscription percentage.
        /// </summary>
        /// <value>
        /// The global subscription percentage.
        /// </value>
        public string GlobalSubscriptionPercentage { get; set; }

        /// <summary>
        /// Gets or sets the total subscription.
        /// </summary>
        /// <value>
        /// The total subscription.
        /// </value>
        public int GlobalEmployeeCount { get; set; }
    }
}
