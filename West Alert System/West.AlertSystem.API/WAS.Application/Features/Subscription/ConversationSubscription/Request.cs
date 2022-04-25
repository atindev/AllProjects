using MediatR;
using System;

namespace WAS.Application.Features.Subscription.ConversationSubscription
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// OfficeLocation
        /// </summary>
        public string OfficeLocation { get; set; }

        /// <summary>
        /// Shift Id
        /// </summary>
        public int? ShiftId { get; set; }

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
        /// Is official email id preferred for mail
        /// </summary>
        public bool IsOfficialEmail { get; set; }

        /// <summary>
        /// Official mobile number
        /// </summary>
        public string OfficeMobile { get; set; }

        /// <summary>
        /// Personal phone number
        /// </summary>
        public string HomePhone { get; set; }

        /// <summary>
        /// Personal mobile number
        /// </summary>
        public string PersonalMobile { get; set; }

        /// <summary>
        /// Is office mobile preferred for voice
        /// </summary>
        public bool IsVoiceOfficeMobile { get; set; }

        /// <summary>
        /// Is personal mobile for voice preferred
        /// </summary>
        public bool IsVoicePersonalMobile { get; set; }

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
        /// Is home phone for voice preferred
        /// </summary>
        public bool IsVoiceHomePhone { get; set; }

        /// <summary>
        /// Mode of Subscription
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
        /// WorkPhone
        /// </summary>
        public string OfficePhone { get; set; }

        /// <summary>
        /// Is office phone for voice preferred
        /// </summary>
        public bool IsVoiceOfficePhone { get; set; }

        /// <summary>
        /// Department
        /// </summary>
        public string DepartmentName { get; set; }

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
        /// Personal email id
        /// </summary>
        public string PersonalEmail { get; set; }

        /// <summary>
        /// Is personal email id preferred for mail
        /// </summary>
        public bool IsPersonalEmail { get; set; }

        /// <summary>
        /// Consent of subscriber 
        /// </summary>
        public bool Consent { get; set; }

        /// <summary>
        /// Gets or sets the preferred channel.
        /// </summary>
        /// <value>
        /// The preferred channel.
        /// </value>
        public string PreferredChannel  { get; set; }

        /// <summary>
        /// Gets or sets the preferred language.
        /// </summary>
        /// <value>
        /// The preferred language.
        /// </value>
        public string PreferredLanguage { get; set; }

        /// <summary>
        /// SubscriptionReviewId for OCR Subscription
        /// </summary>
        public Guid? SubscriptionReviewId { get; set; }
    }
}
