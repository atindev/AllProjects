using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Events.GetAll
{
    public class Response
    {
        /// <summary>
        /// List of events
        /// </summary>
        public List<Event> Events { get; set; }

        /// <summary>
        /// List of event types
        /// </summary>
        public List<EventType> EventTypes { get; set; }

        /// <summary>
        /// List of event urgencies
        /// </summary>
        public List<EventUrgency> EventUrgencies { get; set; }

        /// <summary>
        /// List of groups
        /// </summary>
        public Common.Models.GroupList GroupList { get; set; }
        
    }
}
