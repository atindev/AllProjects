using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class BlockedUser : Entity
    {
        /// <summary>
        /// unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Email Address
        /// </summary>
        public string OfficialEmail { get; set; }

        /// <summary>
        /// Employee Id
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Attempt from
        /// </summary>
        public string AttemptFrom { get; set; }
         
    }
}
