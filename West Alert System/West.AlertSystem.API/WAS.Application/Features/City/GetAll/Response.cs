using System.Collections.Generic;

namespace WAS.Application.Features.City.GetAll
{
    public class Response
    {
        /// <summary>
        /// Get all shift response object
        /// </summary>
        /// <value>
        /// The cities.
        /// </value>
        public List<WAS.Application.Common.Models.City> Cities { get; set; }
            = new List<WAS.Application.Common.Models.City>();
    }
}
