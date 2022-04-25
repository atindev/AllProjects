using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Group.GetActiveDistinctSubscriberCount
{
    public class Response
    {
        /// <summary>
        /// total unique subscriber count
        /// </summary>
        public int TotalSubscribers { get; set; }

        /// <summary>
        /// Total sms count
        /// </summary>
        public int SMSCount { get; set; }

        /// <summary>
        /// Total voice count
        /// </summary>
        public int VoiceCount { get; set; }

        /// <summary>
        /// Total Email count
        /// </summary>
        public int EmailCount { get; set; }

        /// <summary>
        /// Total Whatspp count
        /// </summary>
        public int WhatsappCount { get; set; }

        /// <summary>
        /// Gets or sets the people identifier.
        /// </summary>
        /// <value>
        /// The people identifier.
        /// </value>
        public List<Guid> PeopleId { get; set; }

    }
}
