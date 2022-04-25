using System;
using System.Collections.Generic;
using CreateUpdateGroups = WAS.Application.Features.Groups.CreateUpdate;


namespace WAS.Web.Models
{
    public class AddSubscriptionToGroupModel: CreateUpdateGroups.Request
    {
        /// <summary>
        /// Subscription id
        /// </summary>
        public List<Guid> SubscriptionId { get; set; }
    }
}
