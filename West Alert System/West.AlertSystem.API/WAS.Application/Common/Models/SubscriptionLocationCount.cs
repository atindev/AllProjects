using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class SubscriptionLocationCount
    {
        /// <summary>
        /// Gets or sets the name of the location.
        /// </summary>
        /// <value>
        /// The name of the location.
        /// </value>
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or sets the loactioncount.
        /// </summary>
        /// <value>
        /// The loactioncount.
        /// </value>
        public int LocationCount { get; set; }
    }
}
