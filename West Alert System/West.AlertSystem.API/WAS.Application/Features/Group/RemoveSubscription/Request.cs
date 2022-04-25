using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Group.RemoveSubscription
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Subscription id
        /// </summary>
        public List<Guid> SubscriptionGroupId { get; set; }

        /// <summary>
        /// Group id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// User who modified the record
        /// </summary>
        public string ModifiedBy { get; set; }

    }

}
