using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Groups.GetByIds
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// list of group id
        /// </summary>
        public List<int> Ids { get; set; }

        /// <summary>
        /// need Archived groups also
        /// </summary>
        public bool IsArchiveGroupRequired { get; set; } = false;
    }
}
