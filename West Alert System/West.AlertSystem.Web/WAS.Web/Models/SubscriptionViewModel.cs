using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WAS.Web.Models
{
    public class SubscriptionViewModel
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
        /// Shift Id
        /// </summary>
        public int? ShiftId { get; set; }

        /// <summary>
        /// Subscription Mode
        /// </summary>
        public string SubscriptionMode { get; set; }

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
        public string IsOfficialEmail { get; set; }

        /// <summary>
        /// Is personal email id preferred for mail
        /// </summary>
        public string IsPersonalEmail { get; set; }

        /// <summary>
        /// Is office mobile preferred for voice
        /// </summary>
        public string IsVoiceOfficeMobile { get; set; }

        /// <summary>
        /// Is personal mobile for voice preferred
        /// </summary>
        public string IsVoicePersonalMobile { get; set; }

        /// <summary>
        /// Is office phone for voice preferred
        /// </summary>
        public string IsVoiceOfficePhone { get; set; }

        /// <summary>
        /// Is home phone for voice preferred
        /// </summary>
        public string IsVoiceHomePhone { get; set; }

        /// <summary>
        /// Is office mobile for sms preferred
        /// </summary>
        public string IsTextOfficeMobile { get; set; }

        /// <summary>
        /// Is personal mobile preferred for text
        /// </summary>
        public string IsTextPersonalMobile { get; set; }

        /// <summary>
        /// Is office mobile preferred for whatsapp
        /// </summary>
        public string IsWhatsAppOfficeMobile { get; set; }

        /// <summary>
        /// Is personal mobile preferred for whatsapp
        /// </summary>
        public string IsWhatsAppPersonalMobile { get; set; }

        /// <summary>
        /// Is teams preferred
        /// </summary>
        public string IsTeams { get; set; }

        /// <summary>
        /// Consent
        /// </summary>
        public bool Consent { get; set; }

        /// <summary>
        /// Emplaoyee Id
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Location name
        /// </summary>
        public string OfficeLocation { get; set; }

        /// <summary>
        /// Department
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// JobTitle
        /// </summary>
        public string JobTitle { get; set; }

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
        /// Upn
        /// </summary>
        public string Upn { get; set; }

        /// <summary>
        /// Gets or sets the preferred channel.
        /// </summary>
        /// <value>
        /// The preferred channel.
        /// </value>
        public string PreferredChannel  { get; set; }

        /// <summary>
        /// Gets or sets the preferred channel.
        /// </summary>
        /// <value>
        /// The preferred channel.
        /// </value>
        public string PreferredLanguage { get; set; }

        /// <summary>
        /// SubscriptionReviewId for OCR Subscription
        /// </summary>
        public Guid? SubscriptionReviewId { get; set; }
    }
}
