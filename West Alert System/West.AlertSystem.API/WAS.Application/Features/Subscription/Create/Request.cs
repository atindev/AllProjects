using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Subscription.Create
{
    public class Request : Common.Models.SubscriptionDetails, IRequest<Response>
    {

        /// <summary>
        /// Shift Id
        /// </summary>
        public new int? ShiftId { get; set; }

        /// <summary>
        /// First name
        /// </summary>        
        public new string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>       
        public new string LastName { get; set; }

        /// <summary>
        /// Subscription Mode
        /// </summary>
        public string SubscriptionMode { get; set; }
    }
}
