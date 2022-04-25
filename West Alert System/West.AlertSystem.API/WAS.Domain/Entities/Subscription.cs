using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class Subscription : Entity
    {
        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Location Id
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Official email id
        /// </summary>
        public string OfficialEmail { get; set; }

        /// <summary>
        /// Personal email id
        /// </summary>
        public string PersonalEmail { get; set; }

        /// <summary>
        /// Official mobile number
        /// </summary>
        public string OfficeMobile { get; set; }

        /// <summary>
        /// Personal mobile number
        /// </summary>
        public string PersonalMobile { get; set; }

        /// <summary>
        /// Official phone number
        /// </summary>
        public string OfficePhone { get; set; }

        /// <summary>
        /// Personal phone number
        /// </summary>
        public string HomePhone { get; set; }

        /// <summary>
        /// Is official email id preferred for mail
        /// </summary>
        public bool IsOfficialEmail { get; set; }

        /// <summary>
        /// Is personal email id preferred for mail
        /// </summary>
        public bool IsPersonalEmail { get; set; }

        /// <summary>
        /// Is office mobile preferred for voice
        /// </summary>
        public bool IsVoiceOfficeMobile { get; set; }

        /// <summary>
        /// Is personal mobile for voice preferred
        /// </summary>
        public bool IsVoicePersonalMobile { get; set; }

        /// <summary>
        /// Is office phone for voice preferred
        /// </summary>
        public bool IsVoiceOfficePhone { get; set; }

        /// <summary>
        /// Is home phone for voice preferred
        /// </summary>
        public bool IsVoiceHomePhone { get; set; }

        /// <summary>
        /// Is office mobile for sms preferred
        /// </summary>
        public bool IsTextOfficeMobile { get; set; }

        /// <summary>
        /// Is personal mobile preferred for text
        /// </summary>
        public bool IsTextPersonalMobile { get; set; }

        /// <summary>
        /// Is office mobile preferred for whatsapp
        /// </summary>
        public bool IsWhatsAppOfficeMobile { get; set; }

        /// <summary>
        /// Is personal mobile preferred for whatsapp
        /// </summary>
        public bool IsWhatsAppPersonalMobile { get; set; }

        /// <summary>
        /// Is teams preferred
        /// </summary>
        public bool IsTeams { get; set; }

        /// <summary>
        /// Consent of subscriber 
        /// </summary>
        public bool Consent { get; set; }

        /// <summary>
        /// Shift id
        /// </summary>
        public int? ShiftId { get; set; }

        /// <summary>
        /// Subscription Mode
        /// </summary>
        public string SubscriptionMode { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Upn
        /// </summary>
        public string Upn { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// PostalCode
        /// </summary>
        public int? PostalCode { get; set; }

        /// <summary>
        /// Department
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// JobTitle
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// EmployeeType
        /// </summary>
        public string EmployeeType { get; set; }

        /// <summary>
        /// EmployeeGroup
        /// </summary>
        public string EmployeeGroup { get; set; }

        /// <summary>
        /// CostCenter
        /// </summary>
        public string CostCenter { get; set; }

        /// <summary>
        /// Record modified by function app
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Updated TimeZone from functio app
        /// </summary>
        public string UpdatedTimeZone { get; set; }

        /// <summary>
        /// Subscription assigned to group
        /// </summary>
        public virtual ICollection<SubscriptionGroup> SubscriptionGroups { get; set; }

        /// <summary>
        /// subscription for which location
        /// </summary>
        public virtual Location Location { get; set; }

        /// <summary>
        /// shift of particular subscription
        /// </summary>
        public virtual Shift Shift { get; set; }

        /// <summary>
        /// Gets or sets the departments.
        /// </summary>
        /// <value>
        /// The departments.
        /// </value>
        public virtual Department Department{ get; set; }

        /// <summary>
        /// Collection of DeliveryReportText
        /// </summary>
        public virtual ICollection<DeliveryReportText> DeliveryReportTexts { get; set; }

        /// <summary>
        /// Collection of DeliveryReportWhatsap
        /// </summary>
        public virtual ICollection<DeliveryReportWhatsApp> DeliveryReportWhatsApps { get; set; }

        /// <summary>
        /// Collection of DeliveryReportVoice
        /// </summary>
        public virtual ICollection<DeliveryReportVoice> DeliveryReportVoices { get; set; }

        /// <summary>
        /// Collection of Feedback
        /// </summary>
        public virtual ICollection<SubscriptionFeedback> SubscriptionFeedbacks { get; set; }

        /// <summary>
        /// Gets or sets the preferred channel.
        /// </summary>
        /// <value>
        /// The preferred channel.
        /// </value>
        public string PreferredChannel { get; set; }

        /// <summary>
        /// Gets or sets the preferred language.
        /// </summary>
        /// <value>
        /// The preferred language.
        /// </value>
        public string PreferredLanguage { get; set; }

        /// <summary>
        /// collection of SurveyBroadcastSubscription
        /// </summary>
        public virtual ICollection<SurveyBroadcastSubscription> SurveyBroadcastSubscriptions { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public string Role { get; set; }

    }
}
