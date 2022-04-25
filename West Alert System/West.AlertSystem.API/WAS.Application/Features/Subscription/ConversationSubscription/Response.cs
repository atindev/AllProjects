using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.ConversationSubscription
{
    public class Response
    {
        /// <summary>
        /// Response Message
        /// </summary>
        public string ResponseMessage { get; set; }

        /// <summary>
        /// Success Message
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid SubscriptionId { get; set; }
    }
}
