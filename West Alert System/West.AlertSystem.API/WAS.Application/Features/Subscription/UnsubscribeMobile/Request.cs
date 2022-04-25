using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Subscription.UnsubscribeMobile
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// mobile number
        /// </summary>
        public string MobileNumber { get; set; }
    }
}
