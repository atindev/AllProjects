using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class NotificationApproval
    {
        /// <summary>
        /// Gets or sets the approver subscription.
        /// </summary>
        /// <value>
        /// The approver subscription.
        /// </value>
        public List<Domain.Entities.Subscription> ApproverSubscription { get; set; }

        /// <summary>
        /// Gets or sets the full name of the sender.
        /// </summary>
        /// <value>
        /// The full name of the sender.
        /// </value>
        public string SenderFullName { get; set; }
    }
}
