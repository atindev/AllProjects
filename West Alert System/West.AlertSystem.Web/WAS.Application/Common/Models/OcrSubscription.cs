using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class OcrSubscription : SubscriptionDetails
    {
        /// <summary>
        /// OcrSubscription Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// First Name Confidence
        /// </summary>
        public double? FirstNameConfidence { get; set; }

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
        /// Officia Email Confidence
        /// </summary>
        public double? OfficialEmailConfidence { get; set; }

        /// <summary>
        /// Personal Email Confidence
        /// </summary>
        public double? PersonalEmailConfidence { get; set; }

        /// <summary>
        /// Home Phone Confidence
        /// </summary>
        public double? HomePhoneConfidence { get; set; }

        /// <summary>
        /// Personal Mobile Confidence
        /// </summary>
        public double? PersonalMobileConfidence { get; set; }

        /// <summary>
        /// Is SMS selected for PersonalMobile Confidence
        /// </summary>
        public double? IsTextPersonalMobileConfidence { get; set; }

        /// <summary>
        /// Is Voice selected for PersonalMobile Confidence
        /// </summary>
        public double? IsVoicePersonalMobileConfidence { get; set; }

        /// <summary>
        /// Is WhatsApp selected for PersonalMobile Confidence
        /// </summary>
        public double? IsWhatsAppPersonalMobileConfidence { get; set; }

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
