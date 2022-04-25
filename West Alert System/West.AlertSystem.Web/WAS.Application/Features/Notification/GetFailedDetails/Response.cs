using System.Collections.Generic;

namespace WAS.Application.Features.Notification.GetFailedDetails
{
    public class Response
    {
        /// <summary>
        /// List of All failed notifications
        /// </summary>
        public List<Common.Models.FailedNotification> FailedNotifications { get; set; }
    }
}
