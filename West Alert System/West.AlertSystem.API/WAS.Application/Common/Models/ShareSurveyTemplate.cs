using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class ShareSurveyTemplate
    {
        /// <summary>
        /// Gets or sets the first name of the employee.
        /// </summary>
        /// <value>
        /// The first name of the employee.
        /// </value>
        public string EmployeeFirstName { get; set; }

        /// <summary>
        /// Gets or sets the shared person.
        /// </summary>
        /// <value>
        /// The shared person.
        /// </value>
        public string SharedPerson { get; set; }

        /// <summary>
        /// Gets or sets the name of the survey.
        /// </summary>
        /// <value>
        /// The name of the survey.
        /// </value>
        public string SurveyName { get; set; }

        /// <summary>
        /// Gets or sets the email send grid template identifier.
        /// </summary>
        /// <value>
        /// The email send grid template identifier.
        /// </value>
        public string EmailSendGridTemplateID { get; set; }

    }
}
