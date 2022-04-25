using System;
using System.Collections.Generic;

namespace WAS.Web.Models
{
    public class GroupModel
    {
        public List<int> Ids { get; set; }

        public List<Guid> SubscriptionIds { get; set; }

        public bool IgnoreQueryFilters { get; set; }
    }
}
