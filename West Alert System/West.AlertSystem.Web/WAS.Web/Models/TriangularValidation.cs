using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Web.Models
{
    public class TriangularValidation
    {
        /// <summary>
        /// list of questions
        /// </summary>
        public List<TriangularValidationQuestion> Questions { get; set; } = new List<TriangularValidationQuestion>();

        /// <summary>
        /// user blocked or not
        /// </summary>
        public bool IsUserBlocked { get; set; }

        /// <summary>
        /// Email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Employee Id of the user
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Attempted ON
        /// </summary>
        public string AttemptON { get; set; }

        /// <summary>
        /// Attempted from
        /// </summary>
        public string AttemptFrom { get; set; }
    }

    public class TriangularValidationQuestion
    {
        /// <summary>
        /// Question
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Current Answer
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// current random options
        /// </summary>
        public List<string> Options { get; set; }

        /// <summary>
        /// Question type
        /// </summary>
        public string Type { get; set; }

    }
}
