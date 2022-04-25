using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Subscription.GetAll
{
    public class Response
    {
        /// <summary>
        /// List of active subscriptions
        /// </summary>
        public List<Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Total Number of subscriptions
        /// </summary>
        public int Count { get; set; }

    }
}
