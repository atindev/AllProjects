using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class Department
    {
        /// <summary>
        /// Primary key of department
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the department
        /// </summary>
        public string Name { get; set; }
    }
}
