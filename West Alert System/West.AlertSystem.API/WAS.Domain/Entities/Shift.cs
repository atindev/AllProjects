using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class Shift : Entity
    {  
        /// <summary>
        /// Primary key of Shift
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Shift Name
        /// </summary>
        public string ShiftName { get; set; }

        /// <summary>
        /// Collection of subscriptions
        /// </summary>
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
