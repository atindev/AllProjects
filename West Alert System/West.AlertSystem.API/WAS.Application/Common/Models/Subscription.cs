using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class Subscription
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
        /// Location Id
        /// </summary>
        public int ShiftId { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        //[Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        //[Required]
        public string LastName { get; set; }

        /// <summary>
        /// Official email id
        /// </summary>
        public string OfficialEmail { get; set; }

        /// <summary>
        /// Personal email id
        /// </summary>
        public string PersonalEmail { get; set; }

        /// <summary>
        /// Official mobile number
        /// </summary>
        public string OfficeMobile { get; set; }

        /// <summary>
        /// Personal mobile number
        /// </summary>
        public string PersonalMobile { get; set; }

        /// <summary>
        /// Official phone number
        /// </summary>
        public string OfficePhone { get; set; }

        /// <summary>
        /// Personal phone number
        /// </summary>
        public string HomePhone { get; set; }
    }
}
