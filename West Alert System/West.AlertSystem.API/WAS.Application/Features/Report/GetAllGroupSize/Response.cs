using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Report.GetAllGroupSize
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the subscription mode count.
        /// </summary>
        /// <value>
        /// The subscription mode count.
        /// </value>
        public List<GroupSize> AllGroups { get; set; } = new List<GroupSize>();
    }
}
