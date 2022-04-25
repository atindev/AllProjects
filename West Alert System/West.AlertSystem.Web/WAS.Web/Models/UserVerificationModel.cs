using System;

namespace WAS.Web.Models
{
    public class UserVerificationModel
    {
        /// <summary>
        ///survey broadcast Id
        /// </summary>
        public Guid BroadcastId { get; set; }

        /// <summary>
        /// user blocked time interval
        /// </summary>
        public int UserBlockedInterval { get; set; }

    }
}
