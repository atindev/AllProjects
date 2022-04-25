using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Groups.QueryView
{
    public class Response
    {
        /// <summary>
        /// Filter type selected
        /// </summary>
        public Common.Enum.FilterType FilterType { get; set; }

        /// <summary>
        /// List of locations
        /// </summary>
        public List<Location> Locations { get; set; }

        /// <summary>
        /// List of departments
        /// </summary>
        public List<Department> Departments { get; set; }

        /// <summary>
        /// Gets or sets the cities.
        /// </summary>
        /// <value>
        /// The cities.
        /// </value>
       public List<City> Cities { get; set; }

        /// <summary>
        /// List of locations
        /// </summary>
        public List<Shift> Shifts { get; set; }

        /// <summary>
        /// List of email ids
        /// </summary>
        public List<string> Emails { get; set; }

        /// <summary>
        /// Gets or sets the states.
        /// </summary>
        /// <value>
        /// The states.
        /// </value>
        public List<State> States { get; set; }

        /// <summary>
        /// Gets or sets the countries.
        /// </summary>
        /// <value>
        /// The countries.
        /// </value>
        public List<Country> Countries { get; set; }

        /// <summary>
        /// Gets or sets the employee types.
        /// </summary>
        /// <value>
        /// The employee types.
        /// </value>
        public List<String> EmployeeTypes { get; set; }

        /// <summary>
        /// Gets or sets the employee groups.
        /// </summary>
        /// <value>
        /// The employee groups.
        /// </value>
        public List<String> EmployeeGroups { get; set; }  
        
        /// <summary>
        /// Gets or sets the job titles.
        /// </summary>
        /// <value>
        /// The job titles.
        /// </value>
        public List<String> JobTitles { get; set; }

    }
}
