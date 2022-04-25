using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class FeedbackResource
    {
        /// <summary>
        /// Which user is submited by feedback
        /// </summary>
        public string SubmittedBy { get; set; }

        /// <summary>
        /// Which user is submited by feedback
        /// </summary>
        public string SubmittedByMail { get; set; }

        /// <summary>
        /// Feed back title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Feedback description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// user feed back screen shot Uri
        /// </summary>
        public Uri ScreenshotUri { get; set; }
        
        /// <summary>
        /// Feed back submit date
        /// </summary>
        public DateTime SubmittedOn { get; set; }
    }
}
