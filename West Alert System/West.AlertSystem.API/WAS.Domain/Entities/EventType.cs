using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class EventType : Entity
    {
        /// <summary>
        /// Event type Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Event type name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Collection of event
        /// </summary>
        public virtual ICollection<Event> Events { get; set; }
    }
}
