using System;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class NotificationGroup : Entity
    {
        /// <summary>
        /// Notification group unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Notification Id
        /// </summary>
        public Guid NotificationId { get; set; }

        /// <summary>
        /// Group Id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Associated Notification for this group
        /// </summary>
        public virtual Notification Notification { get; set; }

        /// <summary>
        /// Associated Notification for this group
        /// </summary>
        public virtual Group Group { get; set; }
    }
}
