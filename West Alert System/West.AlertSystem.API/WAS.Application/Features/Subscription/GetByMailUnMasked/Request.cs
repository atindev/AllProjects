using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Subscription.GetByMailUnMasked
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
    }
}
