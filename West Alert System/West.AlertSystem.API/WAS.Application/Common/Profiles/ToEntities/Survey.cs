using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToEntities
{
    public class Survey : AutoMapper.Profile
    {
        public Survey()
        {
            CreateMap<Features.Survey.CreateUpdate.Request, Domain.Entities.Survey>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForAllMembers(opts => opts.Condition((src, dest, srcmember) => srcmember != null))
                ;
        }
    }
}
