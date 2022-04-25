using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.Create
{
    public class Response
    {
        /// <summary>
        /// Is subscription success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Subscription id
        /// </summary>
        public Guid SubscriptionId { get; set; }
    }
}
