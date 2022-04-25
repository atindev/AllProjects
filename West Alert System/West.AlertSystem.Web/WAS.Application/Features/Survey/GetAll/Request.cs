using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Enum;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Survey.GetAll
{
    public class Request : PagedRequest, IRequest<Response>
    {

        /// <summary>
        /// Survey name filter
        /// </summary>
        public string NameFilter { get; set; }

        /// <summary>
        /// Gets or sets the created by filter.
        /// </summary>
        /// <value>
        /// The created by filter.
        /// </value>
        public string CreatedByFilter { get; set; }
    }
}
