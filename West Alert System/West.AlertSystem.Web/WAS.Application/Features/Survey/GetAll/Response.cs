using System.Collections.Generic;

namespace WAS.Application.Features.Survey.GetAll
{
    public class Response
    {
        /// <summary>
        /// List of survey
        /// </summary>
        public List<Common.Models.Survey> Surveys { get; set; }

        /// <summary>
        /// Total Number of Survey
        /// </summary>
        public int Count { get; set; }

    }
}
