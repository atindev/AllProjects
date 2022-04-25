using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Group.GetAll
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
        /// Group filter
        /// </summary>
        public string GroupFilter { get; set; }

        /// <summary>
        /// complete page or particular page
        /// </summary>
        public string PageType { get; set; } = "Complete";

        /// <summary>
        /// need to apply the group access condition
        /// </summary>
        public bool IsAccessRequired { get; set; }

        /// <summary>
        /// user email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// need Archived groups also
        /// </summary>
        public bool IsArchiveGroupRequired { get; set; } = false;

        /// <summary>
        /// is having access to all groups
        /// </summary>
        public bool IsGlobalAdmin { get; set; }

    }
}
