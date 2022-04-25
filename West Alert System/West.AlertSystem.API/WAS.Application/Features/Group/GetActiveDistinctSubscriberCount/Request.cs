using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Group.GetActiveDistinctSubscriberCount
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// All selected Group Ids
        /// </summary>
        public List<int> Ids { get; set; }

        /// <summary>
        /// Subscription Ids
        /// </summary>
        public List<Guid> SubscriptionIds { get; set; }

        /// <summary>
        /// Notification created Date
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    }
}
