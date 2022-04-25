using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class RemainingSubscriptionPercentage
    {
        /// <summary>
        /// Gets or sets the name of the location.
        /// </summary>
        /// <value>
        /// The name of the location.
        /// </value>
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or sets the remaining subscribed count.
        /// </summary>
        /// <value>
        /// The remaining subscribed count.
        /// </value>
        public decimal RemainingSubscribedCount { get; set; }

        /// <summary>
        /// Gets or sets the remaining percent.
        /// </summary>
        /// <value>
        /// The remaining percent.
        /// </value>
        public decimal RemainingPercent { get; set; }

    }
}
