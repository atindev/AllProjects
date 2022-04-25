using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Survey.GetAllBroadcast
{
    public class Response
    {
        /// <summary>
        /// Get all broadcasted Surveys
        /// </summary>
        public List<Common.Models.BroadcastedSurvey> BroadcastedSurveys { get; set; }

        /// <summary>
        /// Total Number of Surveys
        /// </summary>
        public int Count { get; set; }
    }
}
