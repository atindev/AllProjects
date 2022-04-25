using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.GetAllJobTitle
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the JobTitles.
        /// </summary>
        /// <value>
        /// The employee types.
        /// </value>
        public List<string> JobTitles { get; set; }
            = new List<string>();
    }
}
