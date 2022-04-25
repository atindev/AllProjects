using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Enum;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Survey.GetAllBroadcast
{
    public class Request : PagedRequest, IRequest<Response>
    {
        /// <summary>
        /// Survey name filter
        /// </summary>
        public string NameFilter { get; set; }

        /// <summary>
        /// Survey Status filter
        /// </summary>
        public SurveyStatus StatusFilter { get; set; }

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the created by filter.
        /// </summary>
        /// <value>
        /// The created by filter.
        /// </value>
        public string CreatedByFilter { get; set; }

        /// <summary>
        /// Gets or sets the user mail identifier.
        /// </summary>
        /// <value>
        /// The user mail identifier.
        /// </value>
        public string UserMailId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is global admin.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is global admin; otherwise, <c>false</c>.
        /// </value>
        public bool IsGlobalAdmin { get; set; }
    }
}
