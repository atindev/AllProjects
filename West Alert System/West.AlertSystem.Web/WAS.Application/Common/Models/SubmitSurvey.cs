using System;
using System.Collections.Generic;
using WAS.Application.Common.Enum;

namespace WAS.Application.Common.Models
{
    public class SubmitSurvey
    {
        /// <summary>
        /// Survey broadcast id
        /// </summary>
        public Guid BroadcastId { get; set; }

        /// <summary>
        /// Subscriber Id
        /// </summary>
        public Guid SubscriberId { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Answers
        /// </summary>
        public List<Answers> Answers { get; set; }

        /// <summary>
        /// Survey Start Time
        /// </summary>
        public DateTime? SurveyStartTime { get; set; }

        /// <summary>
        /// Survey End Time
        /// </summary>
        public DateTime? SurveyEndTime { get; set; }
    }

    public class Answers
    {
        /// <summary>
        /// Question Number
        /// </summary>
        public string QuestionNumber { get; set; }

        /// <summary>
        /// Question type
        /// </summary>
        public string QuestionType { get; set; }

        /// <summary>
        /// Comments
        /// </summary> 
        public string Comments { get; set; }

        /// <summary>
        /// question option
        /// </summary>
        public List<string> Answer { get; set; }

        /// <summary>
        /// other option
        /// </summary>
        public string OtherOption { get; set; }

        /// <summary>
        /// Execution time
        /// </summary> 
        public int ElapsedTime { get; set; }
    }
}
