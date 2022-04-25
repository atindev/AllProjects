using System;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class IncomingMessage : Entity
    {
        /// <summary>
        /// Primary key of IncomingMessage
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Subscriber Email
        /// </summary>
        public string SubscriberEmail { get; set; }

        /// <summary>
        /// Notification Id
        /// </summary>
        public Guid? NotificationId { get; set; }

        /// <summary>
        /// Incoming Message/Audio
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
        /// Incoming PhoneNumber
        /// </summary>
        public string FromPhone { get; set; }

        /// <summary>
        /// Is this Email
        /// </summary>
        public bool IsEmail { get; set; }

        /// <summary>
        /// Incoming Email Subject
        /// </summary>
        public string EmailSubject { get; set; }

        /// <summary>
        /// From Email
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets the twilio voice mail URL.
        /// </summary>
        /// <value>
        /// The twilio voice mail URL.
        /// </value>
        public string TwilioVoiceMailUrl { get; set; }

        /// <summary>
        /// Associated Notification for this IncomingMessage
        /// </summary>
        public virtual Notification Notification { get; set; }
    }
}
