using MediatR;
using System;

namespace WAS.Application.Features.Notification.DeliveryReport
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Notification Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
