using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class CompletedSurveyReportByDepartment
    {
        /// <summary>
        /// Gets or sets the name of the department.
        /// </summary>
        /// <value>
        public string DepartmentName { get; set; }

        /// <summary>
        /// total people completed the survey
        /// </summary>
        public decimal SubmittedCount { get; set; }

        /// <summary>
        /// Submission percentage
        /// </summary>
        public decimal SubmissionPercentage { get; set; }
    }

    public class PendingSurveySubmissionByDepartment
    {
        /// <summary>
        /// Gets or sets the name of the department.
        /// </summary>
        /// <value>
        public string DepartmentName { get; set; }

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
