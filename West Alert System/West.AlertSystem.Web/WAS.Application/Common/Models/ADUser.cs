using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class ADUser
    {
        /// <summary>
        /// Employee full name
        /// </summary>
        public string FullName { get; set; }

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
        public string Email { get; set; }

        /// <summary>
        /// Employee designation
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// Employee department
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Employee picture in base64 string
        /// </summary>
        public string PictureBase64 { get; set; }

        /// <summary>
        /// Employee business phone
        /// </summary>
        public string BusinessPhones { get; set; }

        /// <summary>
        /// Employee Official Mobile phone
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// Employee location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Employee manager
        /// </summary>
        public ADUser Manager { get; set; }

        /// <summary>
        /// Employee's direct reportees count
        /// </summary>
        public int DirectReportsCount { get; set; }

        /// <summary>
        /// Employee's direct reportees
        /// </summary>
        public IEnumerable<ADUser> DirectReportees { get; set; }

        /// <summary>
        /// Employee EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }
        
        /// <summary>
        /// Employee  reporting manager
        /// </summary>
        public string ReportManager { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }

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

        /// <summary>
        /// Reporting Manager picture in base64 string
        /// </summary>
        public string ManagerPictureBase64 { get; set; }

        /// <summary>
        /// Employee UserId
        /// </summary>
        public string UserId { get; set; }
    }
}
