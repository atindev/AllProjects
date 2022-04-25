using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Subscription.IsBlockedUser
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// User Email or employee Id
        /// </summary>
        public string EmailorEmployeeId { get; set; }
       
    }
}
