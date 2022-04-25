using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToModels
{
    public class SubscriptionGroup : AutoMapper.Profile
    {
        public SubscriptionGroup()
        {
            CreateMap<Domain.Entities.SubscriptionGroup, Models.Group>()
              .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
              .IgnoreAllPropertiesWithAnInaccessibleSetter()
              .ForMember(
                  dest => dest.Id,
                  opt => opt.MapFrom(src => src.GroupId))
              .ForMember(
                  dest => dest.PreferredLanguage,
                  opt => opt.MapFrom(src => src.Subscription.PreferredLanguage))
              .ForMember(
                  dest => dest.Name,
                  opt => opt.MapFrom(src => src.Group.Name))
              .ForMember(
                  dest => dest.SubscriberId,
                  opt => opt.MapFrom(src => src.Subscription.Id))
               .ForMember(
                  dest => dest.SubscriberFirstName,
                  opt => opt.MapFrom(src => src.Subscription.FirstName))
              .ForMember(
                  dest => dest.SubscriberName,
                  opt => opt.MapFrom(src => $"{src.Subscription.FirstName} {src.Subscription.LastName}"))
              .ForMember(
                  dest => dest.LocationId,
                  opt => opt.MapFrom(src => src.Subscription.Location.Id))
              .ForMember(
                  dest => dest.LocationName,
                  opt => opt.MapFrom(src => src.Subscription.Location.Name))
              .ForMember(
                  dest => dest.CountryPhoneNumber,
                  opt => opt.MapFrom(src => src.Subscription.Location.CountryPhoneNumber))
              .ForMember(
                  dest => dest.ShiftName,
                  opt => opt.MapFrom(src => src.Subscription.Shift.ShiftName))
              .ForMember(
                  dest => dest.SubscriberOfficialMobile,
                  opt => opt.MapFrom(src => src.Subscription.OfficeMobile))
              .ForMember(
                  dest => dest.SubscriberPersonalMobile,
                  opt => opt.MapFrom(src => src.Subscription.PersonalMobile))
              .ForMember(
                  dest => dest.SubscriberOfficePhone,
                  opt => opt.MapFrom(src => src.Subscription.OfficePhone))
              .ForMember(
                  dest => dest.SubscriberHomePhone,
                  opt => opt.MapFrom(src => src.Subscription.HomePhone))
              .ForMember(
                  dest => dest.SubscriberOfficialEmail,
                  opt => opt.MapFrom(src => src.Subscription.OfficialEmail))
              .ForMember(
                  dest => dest.SubscriberPersonalEmail,
                  opt => opt.MapFrom(src => src.Subscription.PersonalEmail))
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
              .ForMember(
                  dest => dest.SubcriptionAddedDate,
                  opt => opt.MapFrom(src => src.CreatedDate.ToString("MMM dd, yyyy hh:mm:ss tt")))
               .ForMember(
                  dest => dest.SubscriptionGroupId,
                  opt => opt.MapFrom(src => src.Id))
              ;

            CreateMap<Domain.Entities.NotificationSubscription, Models.Group>()
              .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
              .IgnoreAllPropertiesWithAnInaccessibleSetter()
              .ForMember(
                  dest => dest.Id,
                  opt => opt.MapFrom(src => 0))
              .ForMember(
                  dest => dest.SubscriberId,
                  opt => opt.MapFrom(src => src.Subscription.Id))
               .ForMember(
                  dest => dest.SubscriberFirstName,
                  opt => opt.MapFrom(src => src.Subscription.FirstName))
              .ForMember(
                  dest => dest.SubscriberName,
                  opt => opt.MapFrom(src => $"{src.Subscription.FirstName} {src.Subscription.LastName}"))
              .ForMember(
                  dest => dest.LocationId,
                  opt => opt.MapFrom(src => src.Subscription.Location.Id))
              .ForMember(
                  dest => dest.LocationName,
                  opt => opt.MapFrom(src => src.Subscription.Location.Name))
              .ForMember(
                  dest => dest.CountryPhoneNumber,
                  opt => opt.MapFrom(src => src.Subscription.Location.CountryPhoneNumber))
              .ForMember(
                  dest => dest.ShiftName,
                  opt => opt.MapFrom(src => src.Subscription.Shift.ShiftName))
              .ForMember(
                  dest => dest.SubscriberOfficialMobile,
                  opt => opt.MapFrom(src => src.Subscription.OfficeMobile))
              .ForMember(
                  dest => dest.SubscriberPersonalMobile,
                  opt => opt.MapFrom(src => src.Subscription.PersonalMobile))
              .ForMember(
                  dest => dest.SubscriberOfficePhone,
                  opt => opt.MapFrom(src => src.Subscription.OfficePhone))
              .ForMember(
                  dest => dest.SubscriberHomePhone,
                  opt => opt.MapFrom(src => src.Subscription.HomePhone))
              .ForMember(
                  dest => dest.SubscriberOfficialEmail,
                  opt => opt.MapFrom(src => src.Subscription.OfficialEmail))
              .ForMember(
                  dest => dest.SubscriberPersonalEmail,
                  opt => opt.MapFrom(src => src.Subscription.PersonalEmail))
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
              .ForMember(
                  dest => dest.PreferredLanguage,
                  opt => opt.MapFrom(src => src.Subscription.PreferredLanguage))
              .ForMember(
                  dest => dest.SubcriptionAddedDate,
                  opt => opt.MapFrom(src => src.CreatedDate.ToString("MMM dd, yyyy hh:mm:ss tt")))
              ;

        }
    }
}
