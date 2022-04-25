using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Group.GetAll
{
    public class Group
    {
        /// <summary>
        /// Primary key of group
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the group
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Group created date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// User who created the record
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Last modified date
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// User who last updated the group
        /// </summary>
        public string ModifiedBy { get; set; }
 
        /// <summary>
        /// Number of subscriptions
        /// </summary>
        public int SubscriptionCount { get; set; }

        /// <summary>
        /// Number of email subscriptions
        /// </summary>
        public int EmailSubscriptionCount { get; set; }

        /// <summary>
        /// Number of voice subscriptions
        /// </summary>
        public int VoiceSubscriptionCount { get; set; }

        /// <summary>
        /// Number of text subscriptions
        /// </summary>
        public int TextSubscriptionCount { get; set; }

        /// <summary>
        /// Number of whatsapp subscriptions
        /// </summary>
        public int WhatsAppSubscriptionCount { get; set; }

        /// <summary>
        /// Record last updated
        /// </summary>
        public string Updated { get; set; }

        /// <summary>
        /// Group Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Is this active record
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Is this only accessable to owner
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Is this acccessable to members of the group
        /// </summary>
        public bool IsAccessToAdmins { get; set; }

        /// <summary>
        /// Creater location
        /// </summary>
        public string CreaterLocation { get; set; }

    }
}
