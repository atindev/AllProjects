using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.GetAll
{
    public class Subscription:Common.Models.Subscription
    {
         
        /// <summary>
        /// Location name
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// shift Name
        /// </summary>
        public string Shift { get; set; }

        /// <summary>
        /// Gets or sets the subscribed on.
        /// </summary>
        /// <value>
        /// The subscribed on.
        /// </value>
        public string SubscribedOn { get; set; }
    }
}
