using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Verification.Mail
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
    }
}
