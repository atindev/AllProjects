using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class IncomingMessage
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Corresponding SubscriptionId
        /// </summary>
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Corresponding NotificationId
        /// </summary>
        public Guid NotificationId { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Incoming PhoneNumber
        /// </summary>
        public string FromPhone { get; set; }

        /// <summary>
        /// Incoming Audio/Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Is this Text
        /// </summary>
        public bool IsText { get; set; }

        /// <summary>
        /// Is this WhatsApp
        /// </summary>
        public bool IsWhatsApp { get; set; }

        /// <summary>
        /// Is this Audio
        /// </summary>
        public bool IsVoice { get; set; }

        /// <summary>
        /// Is email notification
        /// </summary>
        public bool IsEmail { get; set; }

        /// <summary>
        /// Record created date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Message Date
        /// </summary>
        public string MessageDate { get; set; }

        /// <summary>
        /// Subscriber Email
        /// </summary>
        public string SubscriberEmail { get; set; }

        /// <summary>
        /// Creater location
        /// </summary>
        public string SubscriberLocation { get; set; }
    }
}
