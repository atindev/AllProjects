using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.DiscardOcrSubscription
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// OCR Subscription id
        /// </summary>
        public Guid Id { get; set; }
    }
}
