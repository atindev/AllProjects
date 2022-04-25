using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.GetAllEmployeeGroup
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the employee groups.
        /// </summary>
        /// <value>
        /// The employee groups.
        /// </value>
        public List<string> EmployeeGroups { get; set; }
            = new List<string>();
    }
}
