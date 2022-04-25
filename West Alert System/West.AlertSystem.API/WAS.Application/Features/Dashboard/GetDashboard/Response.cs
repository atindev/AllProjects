using System.Collections.Generic;

namespace WAS.Application.Features.Dashboard.GetDashboard
{
    public class Response
    {
        /// <summary>
        /// Active Incoming Messages Count
        /// </summary>
        public int IncomingMessageCount { get; set; }

        /// <summary>
        /// Active Notification Count
        /// </summary>
        public int NotificationCount { get; set; }
       
        /// <summary>
        /// Active Event Count
        /// </summary>
       public int EventCount { get; set; }

        /// <summary>
        /// Group Count
        /// </summary>
        public int GroupCount { get; set; }

        /// <summary>
        /// People Count
        /// </summary>
        public int PeopleCount { get; set; }

        /// <summary>
        /// List of Recent Incoming Messages
        /// </summary>
        public List<Common.Models.IncomingMessage> IncomingMessages { get; set; }

        /// <summary>
        /// List of Recent notifications
        /// </summary>
        public List<Common.Models.Notification> RecentNotifications { get; set; }

    }
}
