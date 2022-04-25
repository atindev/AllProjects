using MediatR;
using System;

namespace WAS.Application.Features.Notification.View
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Notification id
        /// </summary>
        public Guid Id { get; set; }
    }
}
