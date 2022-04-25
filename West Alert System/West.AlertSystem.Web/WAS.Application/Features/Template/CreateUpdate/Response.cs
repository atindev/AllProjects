using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WAS.Application.Features.Template.Create
{
    public class Response
    {
        /// <summary>
        /// newly created Category Id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Is Template Create/update success
        /// </summary>
        public bool Success { get; set; }
    }
}

