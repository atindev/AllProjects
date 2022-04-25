using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Enum;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Notification.GetPaged
{
    public class Request : PagedRequest, IRequest<Response>
    {

        /// <summary>
        /// status filter
        /// </summary>
        public Status StatusFilter { get; set; }

        /// <summary>
        /// Event name filter
        /// </summary>
        public string EventFilter { get; set; }

        /// <summary>
        /// Message filter
        /// </summary>
        public string MessageFilter { get; set; }

        /// <summary>
        /// email id of the person
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// is having access to all private notification
        /// </summary>
        public bool IsGlobalAdmin { get; set; }

    }
}
