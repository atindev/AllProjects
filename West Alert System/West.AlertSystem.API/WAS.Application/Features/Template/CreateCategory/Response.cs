using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Template.CreateCategory
{
    public class Response
    {
        /// <summary>
        /// Category unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Is Category Create/update success
        /// </summary>
        public bool Success { get; set; }

    }
}
