using MediatR;
using System;

namespace WAS.Application.Features.Subscription.GetOcrSubscriptionById
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// OCR Subscription id
        /// </summary>
        public Guid Id { get; set; }
    }
}
