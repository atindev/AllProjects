using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class Subscription : SubscriptionDetails
    {
        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Location Id
        /// </summary>
        //[Required]
        public int LocationId { get; set; }

        /// <summary>
        /// Shift Id
        /// </summary>
        public int ShiftId { get; set; }

        /// <summary>
        /// Location name
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// Shift name
        /// </summary>
        public string ShiftName { get; set; }

        /// <summary>
        /// Get all groups response object
        /// </summary>
        public List<Common.Models.Group> Groups { get; set; }
        = new List<Common.Models.Group>();
    }
}