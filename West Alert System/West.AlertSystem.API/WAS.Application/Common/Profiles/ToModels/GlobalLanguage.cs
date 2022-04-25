using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToModels
{
    public class GlobalLanguage: AutoMapper.Profile
    {
        public GlobalLanguage()
        {
            CreateMap<Domain.Entities.GlobalLanguage, Models.GlobalLanguage>()
            .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
            .IgnoreAllPropertiesWithAnInaccessibleSetter();

        }
    }
}
