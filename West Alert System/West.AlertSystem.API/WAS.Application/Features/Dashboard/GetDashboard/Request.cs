using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Dashboard.GetDashboard
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
        /// complete page or particular page
        /// </summary>
        public string PageType { get; set; }

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
