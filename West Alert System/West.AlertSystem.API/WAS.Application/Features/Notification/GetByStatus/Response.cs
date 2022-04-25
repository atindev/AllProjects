using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Notification.GetByStatus
{
    public class Response
    {
        /// <summary>
        /// List of notifications
        /// </summary>
        public List<Common.Models.Notification> Notifications { get; set; }
    }
}
