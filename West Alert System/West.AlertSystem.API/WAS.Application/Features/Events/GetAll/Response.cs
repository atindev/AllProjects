using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Events.GetAll
{
    public class Response
    {
        /// <summary>
        /// List of active events
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
    }
}
