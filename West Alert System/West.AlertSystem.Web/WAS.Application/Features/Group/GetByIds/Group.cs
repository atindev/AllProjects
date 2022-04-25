using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Groups.GetByIds
{
    public class Group
    {
        /// <summary>
        /// Primary key of group
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the group
        /// </summary>
        public string Name { get; set; }

        /// Subscription Id
        /// </summary>
        public Guid SubscriberId { get; set; }

        /// <summary>
        /// Subscriber name
        /// </summary>
        public string SubscriberName { get; set; }

        /// <summary>
        /// Location Id
        /// </summary>
        //[Required]
        public int LocationId { get; set; }

        /// <summary>
        /// Location name
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// shift Name
        /// </summary>
        public string ShiftName { get; set; }

        /// <summary>
        /// Subscriber official mobile number
        /// </summary>
        public string SubscriberOfficialMobile { get; set; }

        /// <summary>
        /// Subscriber personal mobile number
        /// </summary>
        public string SubscriberPersonalMobile { get; set; }

        /// <summary>
        /// Subscriber official email id
        /// </summary>
        public string SubscriberOfficialEmail { get; set; }

        /// <summary>
        /// Subscriber personal email id
        /// </summary>
        public string SubscriberPersonalEmail { get; set; }

        /// <summary>
        /// Is subscriber enabled text(sms) for office mobile
        /// </summary>
        public bool IsTextOfficeMobile { get; set; }

        /// <summary>
        /// Is subscriber enabled personal moblie for sms
        /// </summary>
        public bool IsTextPersonalMobile { get; set; }

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
        /// Subscription Added date
        /// </summary>
        public string SubcriptionAddedDate { get; set; }

        /// Subscription group row Id
        /// </summary>
        public Guid SubscriptionGroupId { get; set; }
    }
}
