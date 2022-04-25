using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class FeedbackChannelCount
    {
        /// <summary>
        /// Gets or sets the name of the feedback channel.
        /// </summary>
        /// <value>
        /// The name of the feedback channel.
        /// </value>
        public string FeedbackChannel { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public List<FeedbackRatings> FeedbackRatings {get;set;}

    }
}
