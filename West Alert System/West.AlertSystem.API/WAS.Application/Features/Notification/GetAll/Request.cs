using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Notification.GetAll
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Current Page number
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Number Of rows in a page
        /// </summary>
        public int RowCount { get; set; }

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
        /// complete page or particular page
        /// </summary>
        public string PageType { get; set; } = "Complete";

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
