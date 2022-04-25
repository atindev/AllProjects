using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.Delete
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Subscriber officialemail
        /// </summary>
        public string OfficialEmail {  get; set;}

        /// <summary>
        /// need to hard delete the user record
        /// </summary>
        public bool IsDeleteRequestFromSubscriber { get; set; }
    }
}
