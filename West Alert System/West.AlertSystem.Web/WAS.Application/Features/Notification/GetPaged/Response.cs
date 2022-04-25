using System.Collections.Generic;

namespace WAS.Application.Features.Notification.GetPaged
{
    public class Response
    {
        /// <summary>
        /// List of notification
        /// </summary>
        public List<Common.Models.Notification> RecentNotifications { get; set; }

        /// <summary>
        /// Total Number of notifications
        /// </summary>
        public int Count { get; set; }

    }
}
