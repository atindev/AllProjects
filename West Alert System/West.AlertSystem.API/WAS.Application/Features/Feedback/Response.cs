using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Feedback
{
   public class Response
    {
        /// <summary>
        /// Feed back status code 
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Feed back status message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Response"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }

    }
}
