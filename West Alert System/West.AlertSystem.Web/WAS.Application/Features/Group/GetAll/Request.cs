using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Enum;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Groups.GetAll
{
    public class Request : PagedRequest, IRequest<Response>
    {
        /// <summary>
        /// Group filter
        /// </summary>
        public string GroupFilter { get; set; }

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
