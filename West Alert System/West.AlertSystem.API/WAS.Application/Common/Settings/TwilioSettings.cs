using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Settings
{
    public class TwilioSettings
    {
        /// <summary>
        /// Twilio AccountSid
        /// </summary>
        public string AccountSid { get; set; }

        /// <summary>
        /// Twilio ServiceSid
        /// </summary>
        public string ServiceSid { get; set; }

        /// <summary>
        /// Twilio AuthToken
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// Twilio ErrorUrl
        /// </summary>
        public string ErrorUrl { get; set; }
    }
}
