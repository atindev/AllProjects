using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Events.GetTypeAndUrgency
{
    public class Response
    {
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
