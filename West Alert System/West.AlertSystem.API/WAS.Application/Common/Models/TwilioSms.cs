using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class TwilioSms
    {
        /// <summary>
        /// From number
        /// </summary>
        public string FromNumber { get; set; }
        /// <summary>
        /// To number
        /// </summary>
        public string ToNumber { get; set; }

        /// <summary>
        /// SMS body
        /// </summary>
        public string Body { get; set; }
    }
}
