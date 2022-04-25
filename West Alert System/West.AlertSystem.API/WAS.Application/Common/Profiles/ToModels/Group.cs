using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToModels
{
    public class Group : AutoMapper.Profile
    {
        public Group()
        {
            CreateMap<Domain.Entities.Group, Features.Group.GetAll.Group>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                   dest => dest.SubscriptionCount,
                   opt => opt.MapFrom(src => src.SubscriptionGroups.Count))
                ;

            CreateMap<Domain.Entities.SubscriptionGroup, Common.Models.Group>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                ;
        }
    }
}
