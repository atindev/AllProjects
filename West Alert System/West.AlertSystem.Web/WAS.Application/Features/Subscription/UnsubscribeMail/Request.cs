using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Subscription.UnsubscribeMail
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Email Id
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Otp received through email
        /// </summary>
        public string Otp { get; set; }

        /// <summary>
        /// need to remove personal email and its preference also
        /// </summary>
        public string RemovePersonalEmail { get; set; }
    }
}
