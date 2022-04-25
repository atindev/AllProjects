using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WAS.Application.Features.Survey.SubmitSurvey
{
    public class Response
    {
        /// <summary>
        /// Is submit success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Is Survey error message
        /// </summary>
        public bool IsAlreadySubmitted { get; set; }
    }
}

