using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Survey.GetUniqueSubscribers
{
    public class Response
    {
        /// <summary>
        /// Get all unique subscriptions
        /// </summary>
        public List<Common.Models.Audience> Audience { get; set; }
    }
}
