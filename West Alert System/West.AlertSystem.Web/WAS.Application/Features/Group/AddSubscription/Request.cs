using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Groups.AddSubscription
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Subscription id
        /// </summary>
        public List<Guid> SubscriptionId { get; set; }

        /// <summary>
        /// Group id
        /// </summary>
        public List<int> GroupId { get; set; }

        /// <summary>
        /// User who created the record
        /// </summary>
        public string CreatedBy { get; set; }
    }

}
