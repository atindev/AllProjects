using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.GetAllSubscribedOn
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the subscribed on.
        /// </summary>
        /// <value>
        /// The subscribed on.
        /// </value>
        public List<string> SubscribedOn { get; set; }
            = new List<string>();
    }
}
