using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Subscription.TriangularValidation
{
    public class Response
    {
        /// <summary>
        /// List of locations
        /// </summary>
        public List<Location> Locations { get; set; }

        /// <summary>
        /// List of departments
        /// </summary>
        public List<Department> Departments { get; set; }

    }
}
