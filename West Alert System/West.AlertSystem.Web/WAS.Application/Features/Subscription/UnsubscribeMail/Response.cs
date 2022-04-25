using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.UnsubscribeMail
{
    public class Response
    {
        /// <summary>
        /// Unsubscriber email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Verify Otp
        /// </summary>
        public string Otp { get; set; }

        /// <summary>
        /// Is verification success
        /// </summary>
        public bool Success { get; set; }
    }
}
