using System;
using System.Collections.Generic;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class SurveyBroadcastTeams : Entity
    {
        /// <summary>
        /// SurveyBroadcastTeams unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// SurveyBroadcastTeams Sent Date
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// SurveyBroadcastId Id
        /// </summary>
        public Guid SurveyBroadcastId { get; set; }

        /// <summary>
        /// Associated SurveyBroadcast for this teams
        /// </summary>
        public virtual SurveyBroadcast SurveyBroadcast { get; set; }
    }
}
