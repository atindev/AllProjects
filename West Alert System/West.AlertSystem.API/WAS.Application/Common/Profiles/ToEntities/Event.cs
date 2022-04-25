using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToEntities
{
    public class Event : AutoMapper.Profile
    {
        public Event()
        {
            CreateMap<Features.Events.CreateUpdate.Request, Domain.Entities.Event>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForAllMembers(opts => opts.Condition((src, dest, srcmember) => srcmember != null))
                ;
        }
    }
}
