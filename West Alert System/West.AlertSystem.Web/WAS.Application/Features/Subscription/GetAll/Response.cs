using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Subscription.GetAll
{
    public class Response 
    {
        /// <summary>
        /// Subscription list
        /// </summary>
        public List<Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Total Number of Subscription
        /// </summary>
        public int Count { get; set; }

    }
}
