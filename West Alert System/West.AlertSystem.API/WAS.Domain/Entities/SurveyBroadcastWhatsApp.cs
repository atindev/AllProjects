using System;
using System.Collections.Generic;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class SurveyBroadcastWhatsApp : Entity
    {
        /// <summary>
        /// SurveyBroadcastWhatsApp unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// SurveyBroadcastWhatsApp Sent Date
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// SurveyBroadcastId Id
        /// </summary>
        public Guid SurveyBroadcastId { get; set; }

        /// <summary>
        /// Associated SurveyBroadcast for this whatsapp
        /// </summary>
        public virtual SurveyBroadcast SurveyBroadcast { get; set; }

    }
}
