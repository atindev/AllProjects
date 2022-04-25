using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class SubscriptionDetails
    {
        /// <summary>
        /// First name
        /// </summary>
        //[Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        //[Required]
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
        /// Consent
        /// </summary>
        public bool Consent { get; set; }

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

    }
}