using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain;

namespace WAS.Application.Common.Profiles.ToEntities
{
    public class Subscription : AutoMapper.Profile
    {
        public Subscription()
        {
            CreateMap<Features.Subscription.Create.Request, Domain.Entities.Subscription>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                   dest => dest.Upn,
                   opt => opt.MapFrom(src => src.OfficialEmail))
                ;
        }
    }
}
