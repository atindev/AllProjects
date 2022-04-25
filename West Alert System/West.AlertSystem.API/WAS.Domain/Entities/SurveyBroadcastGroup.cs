using System;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class SurveyBroadcastGroup : Entity
    {
        /// <summary>
        /// unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// SurveyBroadcast Id
        /// </summary>
        public Guid SurveyBroadcastId { get; set; }

        /// <summary>
        /// Group Id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Associated Survey for this Subscription
        /// </summary>
        public virtual SurveyBroadcast SurveyBroadcast { get; set; }

        /// <summary>
        /// Associated group for this survey
        /// </summary>
        public virtual Group Group { get; set; }
    }
}
