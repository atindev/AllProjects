using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class VoiceNotification
    {
        public Guid Id { get; set; }

        public List<int> GroupIds { get; set; }

        public List<Guid> SubscriptionIds { get; set; }
    }
}
