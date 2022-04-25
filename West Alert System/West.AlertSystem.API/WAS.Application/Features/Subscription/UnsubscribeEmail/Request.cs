using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Subscription.UnsubscribeEmail
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Email id which needs to be unsubscribed
        /// </summary>
        [Required]
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
