using System;

namespace WAS.Application.Common.Models
{
    public class SubscriptionReview
    {
        /// <summary>
        /// AD User
        /// </summary>
        public ADUser ADUser { get; set; }

        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid? SubscriptionId { get; set; }
    }
}
