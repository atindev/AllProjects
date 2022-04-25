using System;
using System.Collections.Generic;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class SurveyBroadcast : Entity
    {
        /// <summary>
        /// broadcast Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Survey Id
        /// </summary>
        public Guid SurveyId { get; set; }

        /// <summary>
        /// Survey start time
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Survey end time
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Survey TimeZone
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Survey TimeZone Offset
        /// </summary>
        public int TimeZoneOffset { get; set; }

        /// <summary>
        /// Is text(sms) Survey
        /// </summary>
        public bool IsText { get; set; }

        /// <summary>
        /// Is email Survey
        /// </summary>
        public bool IsEmail { get; set; }

        /// <summary>
        /// Is teams Survey
        /// </summary>
        public bool IsTeams { get; set; }

        /// <summary>
        /// Is whatsapp Survey
        /// </summary>
        public bool IsWhatsApp { get; set; }

        /// <summary>
        /// Survey status
        /// </summary>
        public Enum.SurveyStatus Status { get; set; }

        /// <summary>
        /// Survey sent date
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// Survey Created Timezone
        /// </summary>
        public string BroadcastedTimeZone { get; set; }

        /// <summary>
        /// Name of the Survey
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Survey Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Survey Path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Question Count
        /// </summary>
        public int? NumberofQuestions { get; set; }

        /// <summary>
        /// Is wizard execution
        /// </summary>
        public bool? IsWizard { get; set; }

        /// <summary>
        /// Total Audience Count
        /// </summary>
        public int TotalAudienceCount { get; set; }

        /// <summary>
        /// Survey 
        /// </summary>
        public virtual Survey Survey { get; set; }

        /// <summary>
        /// collection of SurveyBroadcastSubscription
        /// </summary>
        public virtual ICollection<SurveyBroadcastSubscription> SurveyBroadcastSubscriptions { get; set; }

        /// <summary>
        /// collection of SurveyBroadcastGroup
        /// </summary>
        public virtual ICollection<SurveyBroadcastGroup> SurveyBroadcastGroups { get; set; }

        /// <summary>
        /// collection of SurveyBroadcastEmail
        /// </summary>
        public virtual SurveyBroadcastEmail SurveyBroadcastEmail { get; set; }

        /// <summary>
        /// collection of SurveyBroadcastText
        /// </summary>
        public virtual  SurveyBroadcastText SurveyBroadcastText { get; set; }

        /// <summary>
        /// collection of SurveyBroadcastWhatsapp
        /// </summary>
        public virtual SurveyBroadcastWhatsApp SurveyBroadcastWhatsApp { get; set; }

        /// <summary>
        /// collection of SurveyBroadcastTeams
        /// </summary>
        public virtual SurveyBroadcastTeams SurveyBroadcastTeams { get; set; }

        /// <summary>
        /// collection of SurveyBroadcastFollowup
        /// </summary>
        public virtual SurveyBroadcastFollowup SurveyBroadcastFollowup { get; set; }

        /// <summary>
        /// Gets or sets the share broadcasted surveys.
        /// </summary>
        /// <value>
        /// The share broadcasted surveys.
        /// </value>
        public virtual ICollection<SurveyDetailShare> SurveyDetailShare { get; set; }

        /// <summary>
        /// collection of SurveyBroadcastADUser
        /// </summary>
        public virtual ICollection<SurveyBroadcastADUser> SurveyBroadcastADUsers { get; set; }

        /// <summary>
        /// collection of SurveyBroadcastDistributionGroup
        /// </summary>
        public virtual ICollection<SurveyBroadcastDistributionGroup> SurveyBroadcastDistributionGroups { get; set; }
    }
}
