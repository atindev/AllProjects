using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Models;
using GetAllSubscriptions = WAS.Application.Features.Subscription.GetAll;

namespace WAS.Application.Features.Groups.GetByIds
{
    public class Response
    {
        /// <summary>
        /// Get all subscriptions belongs to a group
        /// </summary>
        public List<Group> Group { get; set; }

        /// <summary>
        /// Get all  groups
        /// </summary>
        public GroupList groupList { get; set; }

        /// <summary>
        /// Subscription list
        /// </summary>
        public List<GetAllSubscriptions.Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Subscription Id
        /// </summary>
        public List<Guid> SubscriptionId { get; set; }

        /// <summary>
        /// Details GroupID
        /// </summary>
        public int GroupId { get; set; }

    }
}
