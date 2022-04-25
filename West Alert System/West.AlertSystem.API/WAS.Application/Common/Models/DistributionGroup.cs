using System;

namespace WAS.Application.Common.Models
{
    public class DistributionGroup
    {
        /// <summary>
        /// Survey Broadcast Distribution Group Id
        /// </summary>
        public Guid? SurveyBroadcastDistributionGroupId { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Distribution Group Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Distribution Group Mail
        /// </summary>
        public string EmailId { get; set; }

        /// <summary>
        /// Created By
        /// </summary>
        public string CreatedBy { get; set; }
    }
}
