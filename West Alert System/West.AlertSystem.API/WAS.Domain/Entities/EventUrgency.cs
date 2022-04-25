using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{ 

    public class EventUrgency : Entity
    {
        /// <summary>
        /// Event urgency Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Event name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Collection of events
        /// </summary>
        public virtual ICollection<Event> Events { get; set; }

    }
}
