using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class Audience : Common.Models.Group
    {
        /// <summary>
        /// EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Department Id
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Department Id
        /// </summary>
        public string DepartmentName { get; set; }
    }
}
