using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class AlertAdmin
    {
      public List<Domain.Entities.Subscription> AdminSubscription { get; set; }

      public Domain.Entities.IncomingMessage IncomingMessage { get; set; } 

      public string SenderFullName { get; set; }
    }
}
