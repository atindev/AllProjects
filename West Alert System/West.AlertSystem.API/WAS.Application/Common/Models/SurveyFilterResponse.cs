using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class SurveyFilterResponse
    {
        /// <summary>
        /// Get all Surveys
        /// </summary>
        public List<Domain.Entities.Survey> Surveys { get; set; }

        /// <summary>
        /// Total Number of Surveys
        /// </summary>
        public int Count { get; set; }
    }
}
