using System;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class SubscriptionGroup: Entity
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Subscription id
        /// </summary>
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Group id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Subscription
        /// </summary>
        public virtual Subscription Subscription { get; set; }
     
        /// <summary>
        /// Group
        /// </summary>
        public virtual Group Group{ get; set; }
    }
}
