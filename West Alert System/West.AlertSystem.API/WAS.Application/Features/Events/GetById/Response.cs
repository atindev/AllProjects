using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Models;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Events.GetById
{
    public class Response
    {
        /// <summary>
        /// Event details
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
