using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAS.Web.Models;
using SubscriptionCreate = WAS.Application.Features.Subscription.Create;
using GetAllSubscriptions = WAS.Application.Features.Subscription.GetAll;
using WAS.Application.Features.Subscription.GetAll;
using WAS.Application.Common.Models;

namespace WAS.Web.Profile
{
    public class Subscription : AutoMapper.Profile
    {
        public Subscription()
        {
            CreateMap<SubscriptionViewModel, SubscriptionCreate.Request>()
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               .ForMember(
                   dest => dest.FirstName,
                   opt => opt.MapFrom(src => src.FirstName))
               .ForMember(
                   dest => dest.LastName,
                   opt => opt.MapFrom(src => src.LastName))
               .ForMember(
                   dest => dest.LocationId,
                   opt => opt.MapFrom(src => src.LocationId))
                 .ForMember(
                   dest => dest.ShiftId,
                   opt => opt.MapFrom(src => src.ShiftId))
               .ForMember(
                   dest => dest.OfficeMobile,
                   opt => opt.MapFrom(src => src.OfficeMobile))
               .ForMember(
                   dest => dest.PersonalMobile,
                   opt => opt.MapFrom(src => src.PersonalMobile))
               .ForMember(
                   dest => dest.OfficialEmail,
                   opt => opt.MapFrom(src => src.OfficialEmail))
               .ForMember(
                   dest => dest.PersonalEmail,
                   opt => opt.MapFrom(src => src.PersonalEmail))
               .ForMember(
                   dest => dest.OfficePhone,
                   opt => opt.MapFrom(src => src.OfficePhone))
               .ForMember(
                   dest => dest.HomePhone,
                   opt => opt.MapFrom(src => src.HomePhone))
#pragma warning disable S1125 // Boolean literals should not be redundant
               .ForMember(
                   dest => dest.IsOfficialEmail,
                   opt => opt.MapFrom(src => src.IsOfficialEmail == "on" ? true : false))
               .ForMember(
                   dest => dest.IsPersonalEmail,
                   opt => opt.MapFrom(src => src.IsPersonalEmail == "on" ? true : false))
               .ForMember(
                   dest => dest.IsTeams,
                   opt => opt.MapFrom(src => src.IsTeams == "on" ? true : false))
               .ForMember(
                   dest => dest.IsTextOfficeMobile,
                   opt => opt.MapFrom(src => src.IsTextOfficeMobile == "on" ? true : false))
               .ForMember(
                   dest => dest.IsTextPersonalMobile,
                   opt => opt.MapFrom(src => src.IsTextPersonalMobile == "on" ? true : false))
               .ForMember(
                   dest => dest.IsVoiceHomePhone,
                   opt => opt.MapFrom(src => src.IsVoiceHomePhone == "on" ? true : false))
               .ForMember(
                   dest => dest.IsVoiceOfficeMobile,
                   opt => opt.MapFrom(src => src.IsVoiceOfficeMobile == "on" ? true : false))
               .ForMember(
                   dest => dest.IsVoiceOfficePhone,
                   opt => opt.MapFrom(src => src.IsVoiceOfficePhone == "on" ? true : false))
               .ForMember(
                   dest => dest.IsVoicePersonalMobile,
                   opt => opt.MapFrom(src => src.IsVoicePersonalMobile == "on" ? true : false))
               .ForMember(
                   dest => dest.IsWhatsAppPersonalMobile,
                   opt => opt.MapFrom(src => src.IsWhatsAppPersonalMobile == "on" ? true : false))
               .ForMember(
                   dest => dest.IsWhatsAppOfficeMobile,
                   opt => opt.MapFrom(src => src.IsWhatsAppOfficeMobile == "on" ? true : false))
#pragma warning restore S1125 // Boolean literals should not be redundant
               ;

            CreateMap<QueryBuilder, GetAllSubscriptions.Request>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
