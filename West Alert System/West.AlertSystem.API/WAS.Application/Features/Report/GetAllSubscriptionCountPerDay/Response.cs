using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Report.GetAllSubscriptionCountPerDay
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the subscription mode count.
        /// </summary>
        /// <value>
        /// The subscription mode count.
        /// </value>
        public List<SubscriberAndUnsubscriberCountPerDay> subscriberAndUnsubscriberCountPerDays { get; set; } = new List<SubscriberAndUnsubscriberCountPerDay>();
    }
}
