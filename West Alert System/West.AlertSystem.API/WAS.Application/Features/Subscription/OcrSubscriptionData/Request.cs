using MediatR;

namespace WAS.Application.Features.Subscription.OcrSubscriptionData
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// First Name Confidence
        /// </summary>
        public double FirstNameConfidence { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Last Name Confidence
        /// </summary>
        public double LastNameConfidence { get; set; }

        /// <summary>
        /// Employee Id
        /// </summary>
        public long? EmployeeId { get; set; }

        /// <summary>
        /// Employee Id Confidence
        /// </summary>
        public double EmployeeIdConfidence { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User Id Confidence
        /// </summary>
        public double? UserIdConfidence { get; set; }

        /// <summary>
        /// Employee Email
        /// </summary>
        public string EmployeeEmail { get; set; }

        /// <summary>
        /// Employee Email Confidence
        /// </summary>
        public double EmployeeEmailConfidence { get; set; }

        /// <summary>
        /// Personal Email
        /// </summary>
        public string PersonalEmail { get; set; }

        /// <summary>
        /// Personal Email Confidence
        /// </summary>
        public double PersonalEmailConfidence { get; set; }

        /// <summary>
        /// Personal Email Domain
        /// </summary>
        public string PersonalEmailDomain { get; set; }

        /// <summary>
        /// Personal Email Domain Confidence
        /// </summary>
        public double PersonalEmailDomainConfidence { get; set; }

        /// <summary>
        /// Home Phone
        /// </summary>
        public long? HomePhone { get; set; }

        /// <summary>
        /// Home Phone Confidence
        /// </summary>
        public double HomePhoneConfidence { get; set; }

        /// <summary>
        /// Cell Phone
        /// </summary>
        public long? CellPhone { get; set; }

        /// <summary>
        /// Cell Phone Confidence
        /// </summary>
        public double CellPhoneConfidence { get; set; }

        /// <summary>
        /// Is SMS enabled
        /// </summary>
        public string IsSMS { get; set; }

        /// <summary>
        /// Is Text preferred for SMS - Confidence
        /// </summary>
        public double IsSMSConfidence { get; set; }

        /// <summary>
        /// Is Voice enabled
        /// </summary>
        public string IsVoice { get; set; }

        /// <summary>
        /// Is Voice enabled - Confidence
        /// </summary>
        public double IsVoiceConfidence { get; set; }

        /// <summary>
        /// Is WhatsApp enabled
        /// </summary>
        public string IsWhatsApp { get; set; }

        /// <summary>
        /// Is WhatsApp enabled - Confidence
        /// </summary>
        public double IsWhatsAppConfidence { get; set; }

        /// <summary>
        /// Consent
        /// </summary>
        public string Consent { get; set; }

        /// <summary>
        /// Consent Confidence
        /// </summary>
        public double ConsentConfidence { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Date Confidence
        /// </summary>
        public double DateConfidence { get; set; }

        /// <summary>
        /// OCR FileName
        /// </summary>
        public string FileName { get; set; }


        /// <summary>
        /// OCR Location Name
        /// </summary>
        public string Location { get; set; }
    }
}
