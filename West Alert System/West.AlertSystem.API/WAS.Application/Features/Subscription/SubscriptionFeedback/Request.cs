using MediatR;
using System;

namespace WAS.Application.Features.Subscription.SubscriptionFeedback
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid? SubscriptionId { get; set; }

        /// <summary>
        /// Upn
        /// </summary>
        public string Upn { get; set; }

        /// <summary>
        /// Feedback Rating
        /// </summary>
        public string Feedback { get; set; }

        /// <summary>
        /// Feedback Number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Feedback Channel
        /// </summary>
        public string FeedbackChannel { get; set; }
    }
}
