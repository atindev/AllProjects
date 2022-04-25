using System;
using System.Collections.Generic;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class SurveyBroadcastEmail : Entity
    {
        /// <summary>
        /// SurveyBroadcast email unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// SurveyEmail Sent Date
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// Survey Id
        /// </summary>
        public Guid SurveyBroadcastId { get; set; }

        /// <summary>
        /// Associated SurveyBroadcast for this mail
        /// </summary>
        public virtual SurveyBroadcast SurveyBroadcast { get; set; }
    }
}
