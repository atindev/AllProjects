using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Enum;

namespace WAS.Application.Common.Models
{
    public class BroadcastedSurvey : SurveyCompletionTime
    {
        /// <summary>
        /// Broadcast Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Survey Id
        /// </summary>
        public Guid SurveyId { get; set; }

        /// <summary>
        /// Survey name
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// User who Broadcasted
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// broadcasted time
        /// </summary>
        public string Updated { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Creater location
        /// </summary>
        public string CreaterLocation { get; set; }

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Timezone
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Sent date
        /// </summary>
        public DateTime? SentDate { get; set; }

        /// <summary>
        /// current status
        /// </summary>
        public SurveyStatus Status { get; set; }

        /// <summary>
        /// Group names
        /// </summary>
        public List<string> GroupNames { get; set; }

        /// <summary>
        /// Delivered Subscriber Names
        /// </summary>
        public List<string> SubscriberNames { get; set; } = new List<string>();

        /// <summary>
        /// for allowd action checking
        /// </summary>
        public string OwnerWithoutSpecialCharacter { get; set; }

        /// <summary>
        ///active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Survey description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Question Count
        /// </summary>
        public int NumberofQuestions { get; set; }

        /// <summary>
        /// Survey followupTime
        /// </summary>
        public DateTime? FollowUpTime { get; set; }

        /// <summary>
        /// Survey TimeZone Offset
        /// </summary>
        public int TimeZoneOffset { get; set; }

        /// <summary>
        /// Group Id
        /// </summary>
        public List<int> GroupId { get; set; }

        /// <summary>
        /// Subscription Id
        /// </summary>
        public List<Guid> SubscriptionId { get; set; }

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
        /// User who updated the survey
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// total unique subscribers
        /// </summary>
        public int AudienceCount { get; set; }

        /// <summary>
        /// total subscribers Submitted
        /// </summary>
        public int SubmittedCount { get; set; }

        /// <summary>
        /// Survey Created Timezone
        /// </summary>
        public string BroadcastedTimeZone { get; set; }

        /// <summary>
        /// Is wizard execution
        /// </summary>
        public bool IsWizard { get; set; }

        /// <summary>
        /// Distribution Group Ids
        /// </summary>
        public List<DistributionGroup> DistributionGroups { get; set; }

        /// <summary>
        /// AD User Emails
        /// </summary>
        public List<ADPeople> ADPeople { get; set; }
    }
}
