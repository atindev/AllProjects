using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Location.GetAll
{
    public class Location
    {
        /// <summary>
        /// Primary key of location
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// City Id
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Name of the location
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Location address
        /// </summary>
        public string Address { get; set; }
    }
}
