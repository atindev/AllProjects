using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class Group : Entity
    {
        /// <summary>
        /// Primary key of location
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the location
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Location Id
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Group Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Is this only accessable to owner
        /// </summary>
        public bool? IsPrivate { get; set; }

        /// <summary>
        /// Is this acccessable to members of the group
        /// </summary>
        public bool? IsAccessToAdmins { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        public virtual Location Location { get; set; }

        /// <summary>
        /// Subscription groups mapped to this group
        /// </summary>
        public virtual ICollection<SubscriptionGroup> SubscriptionGroups { get; set; }
        
        /// <summary>
        /// Notification groups mapped to this group
        /// </summary>
        public virtual ICollection<NotificationGroup> NotificationGroups { get; set; }

        /// <summary>
        /// collection of SurveyBroadcastGroup
        /// </summary>
        public virtual ICollection<SurveyBroadcastGroup> SurveyBroadcastGroups { get; set; }

    }
}
