using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToEntities
{
    public class OcrSubscriptionData : AutoMapper.Profile
    {
        public OcrSubscriptionData()
        {
            CreateMap<Features.Subscription.OcrSubscriptionData.Request, Domain.Entities.OcrSubscription>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               .ForMember(
                   dest => dest.FirstNameConfidence,
                   opt => opt.MapFrom(src => Math.Round(src.FirstNameConfidence * 100, 2)))
               .ForMember(
                   dest => dest.LastNameConfidence,
                   opt => opt.MapFrom(src => Math.Round(src.LastNameConfidence * 100, 2)))
               .ForMember(
                   dest => dest.EmployeeId,
                   opt => opt.MapFrom(src => src.EmployeeId.ToString()))
               .ForMember(
                   dest => dest.EmployeeIdConfidence,
                   opt => opt.MapFrom(src => Math.Round(src.EmployeeIdConfidence * 100, 2)))
               .ForMember(
                   dest => dest.OfficialEmail,
                   opt => opt.MapFrom(src => src.EmployeeEmail.Contains("@") ? src.EmployeeEmail : src.EmployeeEmail + "@WESTPHARMA.COM"))
               .ForMember(
                   dest => dest.OfficialEmailConfidence,
                   opt => opt.MapFrom(src => Math.Round(src.EmployeeEmailConfidence * 100, 2)))
               .ForMember(
                   dest => dest.PersonalEmail,
                   opt => opt.MapFrom(src => src.PersonalEmail + "@" + src.PersonalEmailDomain))
               .ForMember(
                   dest => dest.PersonalEmailConfidence,
                   opt => opt.MapFrom(src => Math.Round(src.PersonalEmailConfidence * 100, 2)))
               .ForMember(
                   dest => dest.HomePhone,
                   opt => opt.MapFrom(src => src.HomePhone != null ? "+" + src.HomePhone.ToString() : null))
               .ForMember(
                   dest => dest.HomePhoneConfidence,
                   opt => opt.MapFrom(src => Math.Round(src.HomePhoneConfidence * 100, 2)))
               .ForMember(
                   dest => dest.PersonalMobile,
                   opt => opt.MapFrom(src => src.CellPhone != null ? "+" + src.CellPhone.ToString() : null))
               .ForMember(
                   dest => dest.PersonalMobileConfidence,
                   opt => opt.MapFrom(src => Math.Round(src.CellPhoneConfidence * 100, 2)))
               .ForMember(
                   dest => dest.IsOfficialEmail,
                   opt => opt.MapFrom(src => src.EmployeeEmail != null))
               .ForMember(
                   dest => dest.IsPersonalEmail,
                   opt => opt.MapFrom(src => src.PersonalEmail != null))
               .ForMember(
                   dest => dest.IsTextPersonalMobile,
                   opt => opt.MapFrom(src => src.CellPhone != null && src.IsSMS == "selected"))
               .ForMember(
                   dest => dest.IsTextPersonalMobileConfidence,
                   opt => opt.MapFrom(src => Math.Round(src.IsSMSConfidence * 100, 2)))
               .ForMember(
                   dest => dest.IsVoicePersonalMobile,
                   opt => opt.MapFrom(src => src.CellPhone != null && src.IsVoice == "selected"))
               .ForMember(
                   dest => dest.IsVoicePersonalMobileConfidence,
                   opt => opt.MapFrom(src => Math.Round(src.IsVoiceConfidence * 100, 2)))
               .ForMember(
                   dest => dest.IsWhatsAppPersonalMobile,
                   opt => opt.MapFrom(src => src.CellPhone != null && src.IsWhatsApp == "selected"))
               .ForMember(
                   dest => dest.IsWhatsAppPersonalMobileConfidence,
                   opt => opt.MapFrom(src => Math.Round(src.IsWhatsAppConfidence * 100, 2)))
               .ForMember(
                   dest => dest.IsVoiceHomePhone,
                   opt => opt.MapFrom(src => src.HomePhone != null && src.IsVoice == "selected"))
               .ForMember(
                   dest => dest.Consent,
                   opt => opt.MapFrom(src => src.Consent == "selected"))
               .ForMember(
                   dest => dest.ConsentConfidence,
                   opt => opt.MapFrom(src => Math.Round(src.ConsentConfidence * 100, 2)))
               ;
        }
    }
}
