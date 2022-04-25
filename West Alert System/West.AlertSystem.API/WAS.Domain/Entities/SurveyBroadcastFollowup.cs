using System;
using System.Collections.Generic;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class SurveyBroadcastFollowup : Entity
    {
        /// <summary>
        /// followup Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Survey Id
        /// </summary>
        public Guid SurveyBroadcastId { get; set; }

        /// <summary>
        /// Survey status
        /// </summary>
        public Enum.SurveyStatus Status { get; set; }

        /// <summary>
        /// Survey followup
        /// </summary>
        public DateTime FollowUpDate { get; set; }

        /// <summary>
        /// Survey sent date
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// Surveybroadcast 
        /// </summary>
        public virtual SurveyBroadcast SurveyBroadcast { get; set; }
    }
}
