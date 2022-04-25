using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WAS.Application.Features.Survey.CreateUpdate
{
    public class Response
    {
        /// <summary>
        /// Survey unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Is Survey Create success
        /// </summary>
        public bool Success { get; set; }
    }
}

