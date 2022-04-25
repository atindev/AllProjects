using System.Collections.Generic;

namespace WAS.Application.Common.Models
{
    public class GetAllSubscriptionMode
    {
        /// <summary>
        /// Gets or sets the subscription mode count.
        /// </summary>
        /// <value>
        /// The subscription mode count.
        /// </value>
        public List<SubscriptionMode> SubscriptionModeCount { get; set; } = new List<SubscriptionMode>();
    }
}
