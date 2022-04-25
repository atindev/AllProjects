using System;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class OcrSubscription : Entity
    {
        /// <summary>
        /// OcrSubscription Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// First Name Confidence
        /// </summary>
        public double? FirstNameConfidence { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Last Name Confidence
        /// </summary>
        public double? LastNameConfidence { get; set; }

        /// <summary>
        /// Employee Id
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Employee Id Confidence
        /// </summary>
        public double? EmployeeIdConfidence { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User Id Confidence
        /// </summary>
        public double? UserIdConfidence { get; set; }

        /// <summary>
        /// Officia Email
        /// </summary>
        public string OfficialEmail { get; set; }

        /// <summary>
        /// Officia Email Confidence
        /// </summary>
        public double? OfficialEmailConfidence { get; set; }

        /// <summary>
        /// Personal Email
        /// </summary>
        public string PersonalEmail { get; set; }

        /// <summary>
        /// Personal Email Confidence
        /// </summary>
        public double? PersonalEmailConfidence { get; set; }

        /// <summary>
        /// Home Phone
        /// </summary>
        public string HomePhone { get; set; }

        /// <summary>
        /// Home Phone Confidence
        /// </summary>
        public double? HomePhoneConfidence { get; set; }

        /// <summary>
        /// Personal Mobile
        /// </summary>
        public string PersonalMobile { get; set; }

        /// <summary>
        /// Personal Mobile Confidence
        /// </summary>
        public double? PersonalMobileConfidence { get; set; }

        /// <summary>
        /// Is official email id preferred for mail
        /// </summary>
        public bool IsOfficialEmail { get; set; }

        /// <summary>
        /// Is personal email id preferred for mail
        /// </summary>
        public bool IsPersonalEmail { get; set; }

        /// <summary>
        /// Is SMS selected for PersonalMobile
        /// </summary>
        public bool IsTextPersonalMobile { get; set; }

        /// <summary>
        /// Is SMS selected for PersonalMobile Confidence
        /// </summary>
        public double? IsTextPersonalMobileConfidence { get; set; }

        /// <summary>
        /// Is Voice selected for PersonalMobile
        /// </summary>
        public bool IsVoicePersonalMobile { get; set; }

        /// <summary>
        /// Is Voice selected for PersonalMobile Confidence
        /// </summary>
        public double? IsVoicePersonalMobileConfidence { get; set; }

        /// <summary>
        /// Is WhatsApp selected for PersonalMobile
        /// </summary>
        public bool IsWhatsAppPersonalMobile { get; set; }

        /// <summary>
        /// Is WhatsApp selected for PersonalMobile Confidence
        /// </summary>
        public double? IsWhatsAppPersonalMobileConfidence { get; set; }

        /// <summary>
        /// Is Voice selected for Home Phone
        /// </summary>
        public bool IsVoiceHomePhone { get; set; }

        /// <summary>
        /// Consent
        /// </summary>
        public bool Consent { get; set; }

        /// <summary>
        /// Consent Confidence
        /// </summary>
        public double? ConsentConfidence { get; set; }

        /// <summary>
        /// OCR FileName
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// OCR FileName
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Location ID
        /// </summary>
        public int LocationId { get; set; }
    }
}
