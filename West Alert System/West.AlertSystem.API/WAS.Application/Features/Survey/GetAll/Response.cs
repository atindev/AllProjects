using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Survey.GetAll
{
    public class Response
    {
        /// <summary>
        /// Get all Surveys
        /// </summary>
        public List<Common.Models.Survey> Surveys { get; set; }

        /// <summary>
        /// Total Number of Surveys
        /// </summary>
        public int Count { get; set; }
    }
}
