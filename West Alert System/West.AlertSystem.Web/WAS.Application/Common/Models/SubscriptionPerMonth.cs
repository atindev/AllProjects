using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class SubscriptionPerMonth
    {
        /// <summary>
        /// Gets or sets the stacked column chart datas.
        /// </summary>
        /// <value>
        /// The stacked column chart datas.
        /// </value>
        public int SubscriptionCount { get; set; }

        /// <summary>
        /// Gets or sets the name of the location.
        /// </summary>
        /// <value>
        /// The name of the location.
        /// </value>
        public string MonthName { get; set; }
    }
}
