using System;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class SurveyBroadcastADUser : Entity
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
        /// First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email Id
        /// </summary>
        public string EmailId { get; set; }

        /// <summary>
        /// Department Id
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Location Id
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Survey Broadcast Distribution Group Id
        /// </summary>
        public Guid? SurveyBroadcastDistributionGroupId { get; set; }

        /// <summary>
        /// Associated Survey
        /// </summary>
        public virtual SurveyBroadcast SurveyBroadcast { get; set; }

        /// <summary>
        /// Associated SurveyBroadcastDistributionGroup
        /// </summary>
        public virtual SurveyBroadcastDistributionGroup SurveyBroadcastDistributionGroup { get; set; }

        /// <summary>
        /// Gets or sets the departments.
        /// </summary>
        /// <value>
        /// The departments.
        /// </value>
        public virtual Department Department { get; set; }

        /// <summary>
        /// AD User location
        /// </summary>
        public virtual Location Location { get; set; }
    }
}
