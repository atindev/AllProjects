using System;
using SubscriptionCreate = WAS.Application.Features.Subscription.ConversationSubscription;
using WAS.Application.Common.Models;

namespace WAS.API.Profile
{
    public class ConversationSubscription : AutoMapper.Profile
    {
        public ConversationSubscription()
        {
            CreateMap<ConversationSubscriptionData, SubscriptionCreate.Request>()
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               .ForMember(
                   dest => dest.OfficialEmail,
                   opt => opt.MapFrom(src => src.WorkEmail))
               .ForMember(
                   dest => dest.IsOfficialEmail,
                   opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.WorkEmail)))
               .ForMember(
                   dest => dest.DepartmentName,
                   opt => opt.MapFrom(src => src.Department))
               .ForMember(
                   dest => dest.PostalCode,
                   opt => opt.MapFrom(src => Convert.ToInt32(src.PostalCode)))
               .ForMember(
                   dest => dest.IsVoiceOfficePhone,
                   opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.OfficePhone)))
               .ForMember(
                   dest => dest.ShiftId,
                   opt => opt.MapFrom(src => Convert.ToInt32(src.ShiftID)))
               .ForMember(
                   dest => dest.SubscriptionMode,
                   opt => opt.MapFrom(src => src.PhoneNumber.Contains("whatsapp:") ? "WhatsApp" : "SMS"))
                .ForMember(
                   dest => dest.OfficeMobile,
                   opt => opt.MapFrom(src => src.PhoneType == "WorkMobile" ? MobileNumberMapping(src) : src.OfficeMobile ))
                .ForMember(
                   dest => dest.PersonalMobile,
                   opt => opt.MapFrom(src => PersonalMobileMapping(src) ? MobileNumberMapping(src) : null))
               .ForMember(
                   dest => dest.IsTextOfficeMobile,
                   opt => opt.MapFrom(src => WorkMobileMapping(src)))
               .ForMember(
                   dest => dest.IsVoiceOfficeMobile,
                   opt => opt.MapFrom(src => WorkMobileMapping(src)))
               .ForMember(
                   dest => dest.IsWhatsAppOfficeMobile,
                   opt => opt.MapFrom(src => WorkMobileMapping(src)))
               .ForMember(
                   dest => dest.IsTextPersonalMobile,
                   opt => opt.MapFrom(src => PersonalMobileMapping(src) && src.IsSMS == "YES"))
               .ForMember(
                   dest => dest.IsVoicePersonalMobile,
                   opt => opt.MapFrom(src => PersonalMobileMapping(src) && src.IsVoice == "YES"))
               .ForMember(
                   dest => dest.IsWhatsAppPersonalMobile,
                   opt => opt.MapFrom(src => PersonalMobileMapping(src) && src.IsWhatsApp == "YES"))
               .ForMember(
                   dest => dest.Consent,
                   opt => opt.MapFrom(src => true))
               ;

            CreateMap<ADUser, ConversationSubscriptionData>()
            .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            ;
        }

        private static bool WorkMobileMapping(ConversationSubscriptionData src)
        {
            if(src.PhoneType == "WorkMobile" || !string.IsNullOrEmpty(src.OfficeMobile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool PersonalMobileMapping(ConversationSubscriptionData src)
        {
            if (src.PhoneType == "PersonalMobile")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string MobileNumberMapping(ConversationSubscriptionData src)
        {
            if (src.PhoneNumber.Contains("whatsapp:"))
            {
                int position = src.PhoneNumber.IndexOf(":");

                return src.PhoneNumber.Substring(position+1);
            }
            else
            {
                return src.PhoneNumber;
            }
        }

    }
}
