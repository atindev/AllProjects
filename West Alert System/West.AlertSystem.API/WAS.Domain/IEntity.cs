using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Domain
{
    public interface IEntity
    {
        /// <summary>
        /// Record created date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Last modified date
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Deleted date
        /// </summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// User who created the record
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the record
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Is this active record
        /// </summary>
        public bool IsActive { get; set; }
    }
}
