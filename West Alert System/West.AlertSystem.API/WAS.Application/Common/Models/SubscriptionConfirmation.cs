using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class SubscriptionConfirmation
    {
        /// <summary>
        /// From number
        /// </summary>
        public string FromNumber { get; set; }

        /// <summary>
        /// To email/mobile
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Name of the receipient
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Channel like email/whatsapp/sms
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// Attempt ON
        /// </summary>
        public string AttemptON { get; set; }
    }
}
