using System;
using System.Collections.Generic;

namespace WAS.Application.Common.Models
{
    public class AnalysisReport
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
        /// key pharse extraction
        /// </summary>
        public List<KeyPhrasesCount> KeyPhrasesCounts { get; set; }

        /// <summary>
        /// Sentiment details
        /// </summary>
        public List<AnswerOption> SentimentsDetails { get; set; }
    }

    public class KeyPhrasesCount
    {
        public string Text { get; set; }

        public int Frequency { get; set; }
    }
   
}
