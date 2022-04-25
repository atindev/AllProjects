using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Enum;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Events.View
{
    public class Response
    {
        /// <summary>
        /// Event detils
        /// </summary>
        public Event Event { get; set; }

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
