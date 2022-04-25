using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.UnsubscribeMobile
{
    public class Response 
    {
        /// <summary>
        /// is it personal or official number
        /// </summary>
        public bool IsPersonalMobile { get; set; } = false;

        /// <summary>
        /// subscriberId
        /// </summary>
        public Guid? SubscriberId { get; set; }

        /// <summary>
        /// Subscriber FirstName
        /// </summary>
        public string SubscriberFirstName { get; set; }
    }
}
