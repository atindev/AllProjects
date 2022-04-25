using System;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class SurveyBroadcastDistributionGroup : Entity
    {
        /// <summary>
        /// Survey Subscription unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// SurveyBroadcast Id
        /// </summary>
        public Guid SurveyBroadcastId { get; set; }

        /// <summary>
        /// Distribution Group
        /// </summary>
        public string DistributionGroup { get; set; }

        /// <summary>
        /// Distribution Group Id
        /// </summary>
        public string DistributionGroupId { get; set; }

        /// <summary>
        /// Distribution Group Name
        /// </summary>
        public string DistributionGroupName { get; set; }

        /// <summary>
        /// Associated Survey for this Subscription
        /// </summary>
        public virtual SurveyBroadcast SurveyBroadcast { get; set; }
    }
}
