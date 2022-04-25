using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Models;
using WAS.Domain;

namespace WAS.Functions.Profiles.ToEntities
{
    public class Subscription : AutoMapper.Profile
    {
        public Subscription()
        {
            
            CreateMap<UserDataFromAd, Domain.Entities.Subscription>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                   dest => dest.PostalCode,
                   opt => opt.MapFrom(src => Convert.ToInt32(src.PostalCodeFromAd)))
                ;
        }
    }
}
