using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles
{
    public class Group : AutoMapper.Profile
    {
        public Group()
        {
            CreateMap<Features.Group.CreateUpdate.Request, Domain.Entities.Group>()
                 .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                 .IgnoreAllPropertiesWithAnInaccessibleSetter()
                 .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id[0]))
                 .ForAllMembers(opts => opts.Condition((src, dest, srcmember) => srcmember != null))
                 ;
        }
    }
}
