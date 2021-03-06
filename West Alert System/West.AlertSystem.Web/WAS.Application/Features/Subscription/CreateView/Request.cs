using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.CreateView
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Official email id
        /// </summary>
        public string OfficialEmail { get; set; }
    }
}
