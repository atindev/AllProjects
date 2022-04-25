using MediatR;
using System;

namespace WAS.Application.Features.Notification.GetById
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Notification Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
