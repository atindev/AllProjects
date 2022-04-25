using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class GroupChangeNotification
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the subscription.
        /// </summary>
        /// <value>
        /// The subscription.
        /// </value>
        public Domain.Entities.Subscription Subscription { get; set; }
    }
}
