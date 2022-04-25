using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Enum;

namespace WAS.Application.Common.Models
{
    public class BroadcastSurveyRequest
    {
        /// <summary>
        /// Survey Id
        /// </summary>
        public Guid SurveyId { get; set; }

        /// <summary>
        /// Survey broadcast Id for update
        /// </summary>
        public Guid BroadcastId { get; set; }

        /// <summary>
        /// Survey StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Survey EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Survey followupTime
        /// </summary>
        public DateTime? FollowUpTime { get; set; }

        /// <summary>
        /// Survey TimeZone
        /// </summary>
        public string TimeZone { get; set; }

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
        /// Distribution Groups
        /// </summary>
        public List<DistributionGroup> DistributionGroups { get; set; }

        /// <summary>
        /// AD User Details
        /// </summary>
        public List<ADPeople> ADPeople { get; set; }

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
        public SurveyStatus Status { get; set; }

        /// <summary>
        /// Survey created by
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Survey Created Timezone
        /// </summary>
        public string BroadcastedTimeZone { get; set; }

        /// <summary>
        /// Is wizard execution
        /// </summary>
        public bool IsWizard { get; set; }
    }
}
