using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Notification.GetByStatus
{
    public class Response
    {
        /// <summary>
        /// List of notifications
        /// </summary>
        public List<Common.Models.Notification> Notifications { get; set; }

        /// <summary>
        /// List of groups
        /// </summary>
        public Common.Models.GroupList GroupList { get; set; }
    }
}
