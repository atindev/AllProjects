using MediatR;
using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Report.GetAllSubscriptionCountPerDay
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Gets or sets the subscription mode count.
        /// </summary>
        /// <value>
        /// The subscription mode count.
        /// </value>
        public List<SubscriberAndUnsubscriberCountPerDay> subscriberAndUnsubscriberCountPerDays { get; set; }
            = new List<SubscriberAndUnsubscriberCountPerDay>();

        /// <summary>
        /// Gets or sets the location identifier.
        /// </summary>
        /// <value>
        /// The location identifier.
        /// </value>
        public int LocationId { get; set; } = new int();
    }
}
