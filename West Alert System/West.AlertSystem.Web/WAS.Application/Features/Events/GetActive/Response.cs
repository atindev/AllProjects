using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Events.GetAtive
{
    public class Response
    {
        /// <summary>
        /// List of active events
        /// </summary>
        public List<Event> Events { get; set; }

        /// <summary>
        /// List of groups
        /// </summary>
        public Common.Models.GroupList GroupList { get; set; }
    }
}
