using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Survey.GetAllSharedSurvey
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the broadcasted survey ids.
        /// </summary>
        /// <value>
        /// The broadcasted survey ids.
        /// </value>
        public List<Guid> BroadcastedSurveyIds { get; set; }
    }
}
