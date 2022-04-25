using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class CompletedSurveyByLocation
    {
        /// <summary>
        /// Gets or sets the name of the location.
        /// </summary>
        /// <value>
        /// The name of the location.
        /// </value>
        public string LocationName { get; set; }

        /// <summary>
        /// total people completed the survey
        /// </summary>
        public decimal SubmittedCount { get; set; }

        /// <summary>
        /// Submission percentage
        /// </summary>
        public decimal SubmissionPercentage { get; set; }
    }

    public class PendingSurveySubmissionByLocation
    {
        /// <summary>
        /// Gets or sets the name of the location.
        /// </summary>
        /// <value>
        /// The name of the location.
        /// </value>
        public string LocationName { get; set; }

        /// <summary>
        /// remaining people completed the survey
        /// </summary>
        public decimal PendingCount { get; set; }

        /// <summary>
        /// Pending percentage
        /// </summary>
        public decimal PendingPercentage { get; set; }
    }
}
