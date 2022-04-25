using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Events.GetActiveEvents
{
    public class Response
    {
        /// <summary>
        /// List of active events
        /// </summary>
        public List<Event> Events { get; set; }
         = new List<Event>();
    }
}
