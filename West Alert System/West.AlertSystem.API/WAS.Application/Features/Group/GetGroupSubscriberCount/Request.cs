using MediatR;
using System;

namespace WAS.Application.Features.Group.GetGroupSubscriberCount
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Selected Group Id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Notification created Date
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    }
}
