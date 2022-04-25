using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class NotificationChannelCount
    {
        /// <summary>
        /// Gets or sets the mode of notification.
        /// </summary>
        /// <value>
        /// The mode of notification.
        /// </value>
        public string NotificationChannel { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; set; }
    }
}
