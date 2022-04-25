using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Survey.ExtractKeyPhrasesFromShortAnswer
{
    public class Response
    {
        /// <summary>
        /// total subscribers Submitted
        /// </summary>
        public int SubmittedCount { get; set; }

        /// <summary>
        /// is wizard or single page
        /// </summary>
        public bool isWizard { get; set; }

        /// <summary>
        /// list of answers
        /// </summary>
        public List<AnalysisReport> AnalysisReports { get; set; } 
    }
}
