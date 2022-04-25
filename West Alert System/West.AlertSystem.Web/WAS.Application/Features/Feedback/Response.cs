using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Feedback
{
    public class Response
    {
        /// <summary>
        /// To success message
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// To Status code
        /// </summary>
        public int StatusCode { get; set; }
       
        /// <summary>
        /// feed back count
        /// </summary>
        public int TotalCount { get; set; }
    }
}
