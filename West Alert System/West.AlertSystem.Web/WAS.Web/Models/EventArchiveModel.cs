using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WAS.Web.Models
{
    public class EventArchiveModel
    {

        /// <summary>
        /// Event unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User who last modified the record
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
