using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class SubscriberAndUnsubscriberCountPerDay
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the subscriber count.
        /// </summary>
        /// <value>
        /// The subscriber count.
        /// </value>
        public int SubscriberCountPerDay { get; set; }

        /// <summary>
        /// Gets or sets the unsubscriber count per day.
        /// </summary>
        /// <value>
        /// The unsubscriber count per day.
        /// </value>
        public int UnsubscriberCountPerDay { get; set; }
    }
}
