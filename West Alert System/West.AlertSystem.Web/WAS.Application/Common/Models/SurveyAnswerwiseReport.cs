using System;
using System.Collections.Generic;

namespace WAS.Application.Common.Models
{
    public class SurveyAnswerwiseReport
    {
        /// <summary>
        /// Questions
        /// </summary>
        public List<SurveyAnswer> Answers { get; set; }
 
        /// <summary>
        /// total subscribers Submitted
        /// </summary>
        public int SubmittedCount { get; set; }

        /// <summary>
        /// is wizard or single page
        /// </summary>
        public bool isWizard { get; set; }
    }

    public class SurveyAnswer
    {
        /// <summary>
        /// Question
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Question Number
        /// </summary>
        public string QuestionNumber { get; set; }

        /// <summary>
        /// Question type
        /// </summary>
        public string QuestionType { get; set; }

        /// <summary>
        /// elapsed time percentage
        /// </summary>
        public string ElapsedPercentage { get; set; }

        /// <summary>
        /// question option
        /// </summary>
        public List<AnswerOption> AnswerOptions { get; set; }
        
        /// <summary>
        /// other option values
        /// </summary>
        public IEnumerable<string> OtherOptions { get; set; }
        /// <summary>
        /// Formatted other option values for Tag cloud control
        /// </summary>
        public List<WordCloud> FormattedOtherOptions { get; set; }

    }

    public class AnswerOption
    {
        /// <summary>
        /// Option text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// sumbitted percentage
        /// </summary>
        public decimal SelectionPercentage { get; set; }

        /// <summary>
        /// sumbitted percentage text
        /// </summary>
        public string PercentageText { get; set; }

        /// <summary>
        /// color mapping
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// response count
        /// </summary>
        public int ResponseCount { get; set; }
    }
}
