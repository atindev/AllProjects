using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class Location
    {
        /// <summary>
        /// Primary key of location
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the location
        /// </summary>
        public string Name { get; set; }
    }
}
