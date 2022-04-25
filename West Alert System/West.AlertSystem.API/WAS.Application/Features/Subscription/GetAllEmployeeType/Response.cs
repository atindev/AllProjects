using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.GetAllEmployeeType
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the employee types.
        /// </summary>
        /// <value>
        /// The employee types.
        /// </value>
        public List<string> EmployeeTypes { get; set; }
            = new List<string>();
    }
}
