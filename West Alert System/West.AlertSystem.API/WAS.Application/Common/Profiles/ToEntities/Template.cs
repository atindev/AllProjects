using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToEntities
{
    public class Template : AutoMapper.Profile
    {
        public Template()
        {
            CreateMap<Features.Template.Create.Request, Domain.Entities.Template>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForAllMembers(opts => opts.Condition((src, dest, srcmember) => srcmember != null))
                ;
        }
    }
}
