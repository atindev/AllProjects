using System;
using System.Collections.Generic;
using GetAllSubscriptions = WAS.Application.Features.Subscription.GetAll;

namespace WAS.Application.Features.Survey.ShareSurvey
{
    public class Response
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Response"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }
    }
}
