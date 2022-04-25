using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class TwilioVerificationResource
    {
        /// <summary>
        /// To resource (ex: Mobile number or email id)
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// OTP sending channel (SMS,Email)
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// OTP to verify
        /// </summary>
        public string Otp { get; set; }
    }
}
