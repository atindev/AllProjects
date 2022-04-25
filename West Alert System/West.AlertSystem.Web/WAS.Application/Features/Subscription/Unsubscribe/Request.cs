using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.Unsubscribe
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Subscription Id
        /// </summary>
        public string OfficialEmail { get; set; }
    }
}
