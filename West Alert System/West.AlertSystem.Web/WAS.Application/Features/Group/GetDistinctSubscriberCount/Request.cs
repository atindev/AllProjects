using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Groups.GetDistinctSubscriberCount
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// All selected Group Ids
        /// </summary>
        public List<int> Ids { get; set; }
        /// <summary>
        /// Gets or sets the subscription ids.
        /// </summary>
        /// <value>
        /// The subscription ids.
        /// </value>
        public List<Guid> SubscriptionIds { get; set; }

    }
}
