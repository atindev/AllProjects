using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain;

namespace WAS.Application.Features.Subscription.GetAll
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<SubscriptionforQuery, Subscription>()
            .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .ForMember(
                dest => dest.Location,
                opt => opt.MapFrom(src => src.LocationName))
             .ForMember(
                dest => dest.Shift,
                opt => opt.MapFrom(src => src.ShiftName))
             .ForMember(
                dest => dest.SubscribedOn,
                opt => opt.MapFrom(src => src.CreatedDate.ToString("MMM dd, yyyy hh:mm:ss tt")))
            ;
        }
    }
}
