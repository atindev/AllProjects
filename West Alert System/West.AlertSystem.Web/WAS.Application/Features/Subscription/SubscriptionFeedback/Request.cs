using MediatR;
using System;

namespace WAS.Application.Features.Subscription.SubscriptionFeedback
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Upn
        /// </summary>
        public Guid? SubscriptionId { get; set; }

        /// <summary>
        /// Feedback Rating
        /// </summary>
        public string Feedback { get; set; }

        /// <summary>
        /// Feedback Channel
        /// </summary>
        public string FeedbackChannel { get; set; }
    }
}
