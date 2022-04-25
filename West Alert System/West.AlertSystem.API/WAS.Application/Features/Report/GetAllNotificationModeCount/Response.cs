using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Report.GetAllNotificationModeCount
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the notification mode count.
        /// </summary>
        /// <value>
        /// The notification mode count.
        /// </value>
        public List<NotificationChannelCount> NotificationModeCount { get; set; } = new List<NotificationChannelCount>();
    }
}
