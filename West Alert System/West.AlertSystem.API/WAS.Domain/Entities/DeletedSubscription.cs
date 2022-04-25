using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{

    public class DeletedSubscription : Entity
    {
#pragma warning disable S4144 // Methods should not have identical implementations

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

#pragma warning disable S4144 // Methods should not have identical implementations
    }
}
