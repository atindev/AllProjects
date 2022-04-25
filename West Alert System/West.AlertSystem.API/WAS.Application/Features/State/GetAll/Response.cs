using System.Collections.Generic;

namespace WAS.Application.Features.State.GetAll
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the states.
        /// </summary>
        /// <value>
        /// The states.
        /// </value>
        public List<WAS.Application.Common.Models.State> States { get; set; }
            = new List<WAS.Application.Common.Models.State>();
    }
}
