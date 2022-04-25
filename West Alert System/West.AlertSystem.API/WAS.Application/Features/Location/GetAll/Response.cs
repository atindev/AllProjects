using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Location.GetAll
{
    public class Response
    {
        /// <summary>
        /// Get all locations response object
        /// </summary>
        public List<Location> Locations { get; set; }
            = new List<Location>();
    }
}
