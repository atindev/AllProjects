using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class AlertAdminWithMessage
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
        public string FirstName { get; set; }

        /// <summary>
        /// Channel like email/whatsapp/sms
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// Attempt ON
        /// </summary>
        public string AttemptON { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the full name of the employee.
        /// </summary>
        /// <value>
        /// The full name of the employee.
        /// </value>
        public string SenderEmployeeFullName { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

    }
}
