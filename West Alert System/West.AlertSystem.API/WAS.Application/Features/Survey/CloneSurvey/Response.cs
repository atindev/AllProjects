using System;

namespace WAS.Application.Features.Survey.CloneSurvey
{
    public class Response
    {
        /// <summary>
        /// new cloned survey id
        /// </summary>
        public Guid SurveyId { get; set; }
        
        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }

    }
}
