using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles
{
    public class TemplateCategory : AutoMapper.Profile
    {
        public TemplateCategory()
        {
            CreateMap<Features.Template.CreateCategory.Request, Domain.Entities.TemplateCategory>()
                 .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                 .IgnoreAllPropertiesWithAnInaccessibleSetter()
                 .ForAllMembers(opts => opts.Condition((src, dest, srcmember) => srcmember != null))
                ;
        }
    }
}
