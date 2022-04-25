using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WAS.Web.Models
{
    public class EventViewModel
    {
        /// <summary>
        /// Event unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Event name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User who created the record
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the record
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Event type Id
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// Event urgency Id
        /// </summary>
        public int UrgencyId { get; set; }

        /// <summary>
        /// Event status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Event location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Event description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// event view page details
        /// </summary>
        public string PageType { get; set; } 

    }
}
