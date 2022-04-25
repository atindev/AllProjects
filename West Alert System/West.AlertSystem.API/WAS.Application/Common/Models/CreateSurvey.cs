using System;
using System.Collections.Generic;

namespace WAS.Application.Common.Models
{
    public class CreateSurvey
    {
        /// <summary>
        /// Survey unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Survey Subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// User who created the record
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// User who modified the record
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Group Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Questions
        /// </summary>
        public List<SurveyQuestions> Questions { get; set; }
    }

    public class SurveyQuestions
    {
        /// <summary>
        /// Question Type
        /// </summary>
        public string QuestionType { get; set; }

        /// <summary>
        /// Question
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Question Number
        /// </summary>
        public string QuestionNumber { get; set; }

        /// <summary>
        /// Question Number
        /// </summary>
        public string ShortAnswerPlaceHolder { get; set; }

        /// <summary>
        /// Gets or sets the short length of the answer.
        /// </summary>
        /// <value>
        /// The short length of the answer.
        /// </value>
        public string ShortAnswerLength { get; set; }

        /// <summary>
        /// question option
        /// </summary>
        public List<Option> Options { get; set; }

        /// <summary>
        /// Is question required
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Rating Type
        /// </summary>
        public int RatingType { get; set; }
        /// <summary>
        /// Is question required
        /// </summary>
        public bool IsCommentsEnabled { get; set; }
        
        /// <summary>
        /// Is Other option enabled
        /// </summary>
        public bool IsOtherOptionEnabled { get; set; }

    }

    public class Option
    {
        /// <summary>
        /// Option Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Option text
        /// </summary>
        public string Text { get; set; }
    }
}
