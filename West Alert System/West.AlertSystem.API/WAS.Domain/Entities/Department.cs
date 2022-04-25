using System.Collections.Generic;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class Department : Entity
    {
        /// <summary>
        /// Primary key of Department
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the subscriptions.
        /// </summary>
        /// <value>
        /// The subscriptions.
        /// </value>
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Collection of SurveyBroadcast ADUsers
        /// </summary>
        public virtual ICollection<SurveyBroadcastADUser> SurveyBroadcastADUsers { get; set; }
    }
}
