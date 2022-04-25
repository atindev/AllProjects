using System;
using System.Collections.Generic;
using WAS.Domain.Enum;

namespace WAS.Application.Common.Models
{
    public class FailedNotification
    {

        /// <summary>
        /// Person to whom notification send
        /// </summary>
        public Guid SubscriberId { get; set; }

        /// <summary>
        /// Subscriber Email
        /// </summary>
        public string SubscriberEmail { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        public string CreatedDate { get; set; }

        /// <summary>
        /// Subscriber Name
        /// </summary>
        public string SubscriberName { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Sent to number
        /// </summary>
        public string ToNumber { get; set; }

        /// <summary>
        /// Error code
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Error url
        /// </summary>
        public string ErrorURL { get; set; } = null;

        /// <summary>
        /// Subscriber location
        /// </summary>
        public string SubscriberLocation { get; set; }
    }
}
