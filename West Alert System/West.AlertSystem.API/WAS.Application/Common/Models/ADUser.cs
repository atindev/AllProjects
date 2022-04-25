using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class ADUser
    {
        /// <summary>
        /// Employee first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Employee last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Employee email id
        /// </summary>
        public string WorkEmail { get; set; }

        /// <summary>
        /// Employee designation
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// Employee department
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Employee business phone
        /// </summary>
        public string OfficePhone { get; set; }

        /// <summary>
        /// Employee Official Mobile phone
        /// </summary>
        public string OfficeMobile { get; set; }

        /// <summary>
        /// Employee location
        /// </summary>
        public string OfficeLocation { get; set; }

        /// <summary>
        /// Employee EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// PostalCode
        /// </summary>
        public int? PostalCode { get; set; }

        /// <summary>
        /// PostalCode
        /// </summary>
        public string EmployeeType { get; set; }

        /// <summary>
        /// PostalCode
        /// </summary>

        public string EmployeeGroup { get; set; }

        /// <summary>
        /// PostalCode
        /// </summary>
        public string CostCenter { get; set; }

        /// <summary>
        /// User principal name
        /// </summary>
        public string UserPrincipalName { get; set; }
    }
}
