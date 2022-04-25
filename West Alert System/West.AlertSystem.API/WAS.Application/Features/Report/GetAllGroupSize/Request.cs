using MediatR;
using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Report.GetAllGroupSize
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Gets or sets the notification per months.
        /// </summary>
        /// <value>
        /// The notification per months.
        /// </value>
        public List<GroupSize> AllGroups { get; set; } = new List<GroupSize>();

        /// <summary>
        /// Gets or sets the location identifier.
        /// </summary>
        /// <value>
        /// The location identifier.
        /// </value>
        public int LocationId { get; set; } = new int();
    }
}
