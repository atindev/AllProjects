using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class Event : Entity
    {
        /// <summary>
        /// Event unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Event name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Event type Id
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// Event urgency Id
        /// </summary>
        public int UrgencyId { get; set; }

        /// <summary>
        /// Event status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Event location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Event description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Event type
        /// </summary>
        public virtual EventType EventType { get; set; }

        /// <summary>
        /// Event urgency
        /// </summary>
        public virtual  EventUrgency EventUrgency { get; set; }

        /// <summary>
        /// collection of notification
        /// </summary>
        public virtual ICollection<Notification> Notifications { get; set; }

    }
}
