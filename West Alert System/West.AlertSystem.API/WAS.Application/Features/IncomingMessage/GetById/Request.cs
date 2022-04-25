using MediatR;
using System;

namespace WAS.Application.Features.IncomingMessage.GetById
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// IncomingMessage Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
