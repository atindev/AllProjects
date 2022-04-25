using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Report.GetAllNotifiactionPerMonth
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the subscription mode count.
        /// </summary>
        /// <value>
        /// The subscription mode count.
        /// </value>
        public List<AllNotificationPerMonth> NotificationPerMonths { get; set; } = new List<AllNotificationPerMonth>();
    }
}
