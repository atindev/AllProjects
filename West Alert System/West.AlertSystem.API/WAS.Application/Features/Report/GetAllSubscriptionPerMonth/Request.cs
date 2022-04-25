using MediatR;
using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Report.GetAllSubscriptionPerMonth
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Gets or sets the subscription per months.
        /// </summary>
        /// <value>
        /// The subscription per months.
        /// </value>
        public List<SubscriptionPerMonth> SubscriptionPerMonths { get; set; } = new List<SubscriptionPerMonth>();

        /// <summary>
        /// Gets or sets the location identifier.
        /// </summary>
        /// <value>
        /// The location identifier.
        /// </value>
        public int LocationId { get; set; } = new int();
    }
}
