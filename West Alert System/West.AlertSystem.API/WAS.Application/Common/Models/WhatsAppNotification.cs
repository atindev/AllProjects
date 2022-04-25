using System;
using System.Collections.Generic;

namespace WAS.Application.Common.Models
{
    public class WhatsAppNotification
    {
        public Guid Id { get; set; }

        public List<int> GroupIds { get; set; }

        public List<Guid> SubscriptionIds { get; set; }
    }
}
