using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WAS.Application.Features.Events.CreateUpdate
{
    public class Response
    {
        /// <summary>
        /// Event unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Is Event Create/update success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Event Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is event name already exist
        /// </summary>
        public bool IsNameExist { get; set; }
    }
}

