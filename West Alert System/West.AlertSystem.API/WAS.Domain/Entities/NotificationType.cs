using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class NotificationType : Entity
    {
        /// <summary>
        /// NotificationType Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Noitifiction type 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Associated Notification for this Notification Type
        /// </summary>
        public virtual ICollection<Notification> Notification { get; set; }

    }
}
