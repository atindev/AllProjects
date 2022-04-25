using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class Event
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
        /// User who created the record
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Event type Id
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// Event type name
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Event urgency Id
        /// </summary>
        public int UrgencyId { get; set; }

        /// <summary>
        /// Urgency name
        /// </summary>
        public string UrgencyName { get; set; }

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
        /// Notification count of this event
        /// </summary>
        public int NotificationCount { get; set; }

        /// <summary>
        /// When this event is last updated
        /// </summary>
        public string Updated { get; set; }

        /// <summary>
        /// Last modified date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Last modified date
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Is this record active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Creater location
        /// </summary>
        public string CreaterLocation { get; set; }

        /// <summary>
        /// List of notifications for this event
        /// </summary>
        public List<Notification> Notifications { get; set; }
    }
}
