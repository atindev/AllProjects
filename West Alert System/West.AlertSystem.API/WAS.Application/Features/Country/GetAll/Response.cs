using System.Collections.Generic;

namespace WAS.Application.Features.Country.GetAll
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the countries.
        /// </summary>
        /// <value>
        /// The countries.
        /// </value>
        public List<WAS.Application.Common.Models.Country> Countries { get; set; }
            = new List<WAS.Application.Common.Models.Country>();
    }
}
