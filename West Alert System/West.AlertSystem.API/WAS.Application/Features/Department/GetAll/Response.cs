using System.Collections.Generic;

namespace WAS.Application.Features.Department.GetAll
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the departments.
        /// </summary>
        /// <value>
        /// The departments.
        /// </value>
        public List<WAS.Application.Common.Models.Department> Departments { get; set; }
            = new List<WAS.Application.Common.Models.Department>();
    }
}
