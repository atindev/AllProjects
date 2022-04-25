using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class SubscriptionPercentage
    {
        /// <summary>
        /// Gets or sets the name of the location.
        /// </summary>
        /// <value>
        /// The name of the location.
        /// </value>
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or sets the subscribed count.
        /// </summary>
        /// <value>
        /// The subscribed count.
        /// </value>
        public decimal SubscribedCount { get; set; }
    }
}
