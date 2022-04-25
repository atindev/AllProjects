using MediatR;
using System;

namespace WAS.Application.Features.IncomingMessage.View
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Incoming Message id
        /// </summary>
        public Guid Id { get; set; }
    }
}
