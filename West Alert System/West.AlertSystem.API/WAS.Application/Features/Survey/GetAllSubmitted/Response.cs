using System.Collections.Generic;

namespace WAS.Application.Features.Survey.GetAllSubmitted
{
    public class Response
    {
        /// <summary>
        /// each survey response
        /// </summary>
        public List<Common.Models.SubmitSurvey> Answers { get; set; }
    }
}
