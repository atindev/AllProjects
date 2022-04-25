using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToModels
{
    public class EventType:AutoMapper.Profile
    {
        public EventType()
        {
            CreateMap<Domain.Entities.EventType, Common.Models.EventType>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               ;
        }
    }
}
