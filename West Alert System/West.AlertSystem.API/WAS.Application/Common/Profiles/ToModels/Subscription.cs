using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain;

namespace WAS.Application.Common.Profiles.ToModels
{
    public class Subscription : AutoMapper.Profile
    {
        public Subscription()
        {
            CreateMap<Domain.Entities.Subscription, Features.Subscription.GetAll.SubscriptionforQuery>()
              .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
              .IgnoreAllPropertiesWithAnInaccessibleSetter()
              .ForMember(
                  dest => dest.LocationName,
                  opt => opt.MapFrom(src => src.Location.Name))
               .ForMember(
                  dest => dest.ShiftName,
                  opt => opt.MapFrom(src => src.Shift.ShiftName))
               .ForMember(
                dest => dest.CityId,
                opt => opt.MapFrom(src => src.Location.City.Id))
               .ForMember(
                dest => dest.StateId,
                opt => opt.MapFrom(src => src.Location.City.State.Id))
                .ForMember(
                dest => dest.CountryId,
                opt => opt.MapFrom(src => src.Location.City.State.Country.Id))
              ;

            CreateMap<Domain.Entities.Subscription, Features.Subscription.GetAll.Subscription>()
             .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
             .IgnoreAllPropertiesWithAnInaccessibleSetter()
             .ForMember(
                 dest => dest.Location,
                 opt => opt.MapFrom(src => src.Location.Name))
              .ForMember(
                 dest => dest.Shift,
                 opt => opt.MapFrom(src => src.Shift.ShiftName))
               .ForMember(
                dest => dest.SubscribedOn,
                opt => opt.MapFrom(src => src.CreatedDate.ToString("MMM dd, yyyy hh:mm:ss tt")))
             ;

            CreateMap<Domain.Entities.Subscription, Features.Subscription.GetByMail.Response>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                  dest => dest.LocationName,
                  opt => opt.MapFrom(src => src.Location.Name))
               .ForMember(
                  dest => dest.ShiftName,
                  opt => opt.MapFrom(src => src.Shift.ShiftName))
                ;
            CreateMap<Domain.Entities.Subscription, Features.Subscription.GetByMailUnMasked.Response>()
              .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
              .IgnoreAllPropertiesWithAnInaccessibleSetter()
              .ForMember(
                dest => dest.LocationName,
                opt => opt.MapFrom(src => src.Location.Name))
             .ForMember(
                dest => dest.ShiftName,
                opt => opt.MapFrom(src => src.Shift.ShiftName))
              ;

            CreateMap<Domain.Entities.Subscription, Features.Subscription.GetById.Response>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                  dest => dest.LocationName,
                  opt => opt.MapFrom(src => src.Location.Name))
               .ForMember(
                  dest => dest.ShiftName,
                  opt => opt.MapFrom(src => src.Shift.ShiftName))
                ;

            CreateMap<Domain.Entities.SubscriptionGroup, Common.Models.SubscriptionDetails>()
           .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
           .IgnoreAllPropertiesWithAnInaccessibleSetter()
           .ForMember(
               dest => dest.Id,
               opt => opt.MapFrom(src => src.Subscription.Id))
           .ForMember(
               dest => dest.IsTextOfficeMobile,
               opt => opt.MapFrom(src => src.Subscription.IsTextOfficeMobile))
           .ForMember(
               dest => dest.IsTextPersonalMobile,
               opt => opt.MapFrom(src => src.Subscription.IsTextPersonalMobile))
            .ForMember(
               dest => dest.IsOfficialEmail,
               opt => opt.MapFrom(src => src.Subscription.IsOfficialEmail))
            .ForMember(
               dest => dest.IsPersonalEmail,
               opt => opt.MapFrom(src => src.Subscription.IsPersonalEmail))
            .ForMember(
               dest => dest.IsVoiceHomePhone,
               opt => opt.MapFrom(src => src.Subscription.IsVoiceHomePhone))
            .ForMember(
               dest => dest.IsVoiceOfficeMobile,
               opt => opt.MapFrom(src => src.Subscription.IsVoiceOfficeMobile))
            .ForMember(
               dest => dest.IsVoiceOfficePhone,
               opt => opt.MapFrom(src => src.Subscription.IsVoiceOfficePhone))
            .ForMember(
               dest => dest.IsVoicePersonalMobile,
               opt => opt.MapFrom(src => src.Subscription.IsVoicePersonalMobile))
            .ForMember(
               dest => dest.IsWhatsAppOfficeMobile,
               opt => opt.MapFrom(src => src.Subscription.IsWhatsAppOfficeMobile))
            .ForMember(
               dest => dest.IsWhatsAppPersonalMobile,
               opt => opt.MapFrom(src => src.Subscription.IsWhatsAppPersonalMobile))
           ;

            CreateMap<Domain.Entities.NotificationSubscription, Common.Models.SubscriptionDetails>()
           .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
           .IgnoreAllPropertiesWithAnInaccessibleSetter()
           .ForMember(
               dest => dest.Id,
               opt => opt.MapFrom(src => src.Subscription.Id))
           .ForMember(
               dest => dest.IsTextOfficeMobile,
               opt => opt.MapFrom(src => src.Subscription.IsTextOfficeMobile))
           .ForMember(
               dest => dest.IsTextPersonalMobile,
               opt => opt.MapFrom(src => src.Subscription.IsTextPersonalMobile))
            .ForMember(
               dest => dest.IsOfficialEmail,
               opt => opt.MapFrom(src => src.Subscription.IsOfficialEmail))
            .ForMember(
               dest => dest.IsPersonalEmail,
               opt => opt.MapFrom(src => src.Subscription.IsPersonalEmail))
            .ForMember(
               dest => dest.IsVoiceHomePhone,
               opt => opt.MapFrom(src => src.Subscription.IsVoiceHomePhone))
            .ForMember(
               dest => dest.IsVoiceOfficeMobile,
               opt => opt.MapFrom(src => src.Subscription.IsVoiceOfficeMobile))
            .ForMember(
               dest => dest.IsVoiceOfficePhone,
               opt => opt.MapFrom(src => src.Subscription.IsVoiceOfficePhone))
            .ForMember(
               dest => dest.IsVoicePersonalMobile,
               opt => opt.MapFrom(src => src.Subscription.IsVoicePersonalMobile))
            .ForMember(
               dest => dest.IsWhatsAppOfficeMobile,
               opt => opt.MapFrom(src => src.Subscription.IsWhatsAppOfficeMobile))
            .ForMember(
               dest => dest.IsWhatsAppPersonalMobile,
               opt => opt.MapFrom(src => src.Subscription.IsWhatsAppPersonalMobile))
           ;
        }
    }
}
