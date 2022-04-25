using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class SubscriptionDetails : Subscription
    {
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
        /// Location name
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// Shift name
        /// </summary>
        public string ShiftName { get; set; }

        /// <summary>
        /// Get all groups response object
        /// </summary>
        public List<Common.Models.Group> Groups { get; set; }
            = new List<Common.Models.Group>();

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
