using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Report.GetSubscriptionPercentageByLocation
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the subscription location percentage.
        /// </summary>
        /// <value>
        /// The subscription location percentage.
        /// </value>
        public LocationCountBySubscriptionPercentage SubscriptionLocationPercentage { get; set; } = new LocationCountBySubscriptionPercentage();
    }
}
