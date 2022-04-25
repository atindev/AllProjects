using System;
using System.Collections.Generic;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class SurveyBroadcastText : Entity
    {
        /// <summary>
        /// SurveyBroadcastText unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// NotificationText Sent Date
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// SurveyBroadcast Id
        /// </summary>
        public Guid SurveyBroadcastId { get; set; }

        /// <summary>
        /// Associated SurveyBroadcast for this SMS
        /// </summary>
        public virtual SurveyBroadcast SurveyBroadcast { get; set; }
    }
}
