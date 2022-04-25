using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class EmailNotification
    {
        public Guid Id { get; set; }

        public List<int> GroupIds { get; set; }

        public List<Guid> SubscriptionIds { get; set; }

        public List<DistributionGroup> DistributionGroups { get; set; }

        public List<ADPeople> ADPeople { get; set; }

        public string EmailSendGridTemplateID { get; set; }

        public bool IsFollowUpEmail { get; set; }
    }
}
