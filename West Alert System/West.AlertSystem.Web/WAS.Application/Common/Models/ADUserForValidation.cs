using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class ADUserForValidation
    {
        /// <summary>
        /// Employee email id
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Employee department
        /// </summary>
        public string Department { get; set; }
        
        /// <summary>
        /// Employee location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Employee manager
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// Employee id
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        public string Name { get; set; }

    }
}
