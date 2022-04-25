using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Group.GetByIds
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Group Ids
        /// </summary>
        public List<int> Ids { get; set; }

        /// <summary>
        /// Subscription Ids
        /// </summary>
        public List<Guid> SubscriptionIds { get; set; }

        /// <summary>
        /// need Archived groups also
        /// </summary>
        public bool IsArchiveGroupRequired { get; set; } = false;
    }
}
