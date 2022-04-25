using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.GetById
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
