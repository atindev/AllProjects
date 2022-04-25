using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToModels
{
    public class EventUrgency:AutoMapper.Profile
    {
        public EventUrgency()
        {
            CreateMap<Domain.Entities.EventUrgency, Common.Models.EventUrgency>()
              .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
              .IgnoreAllPropertiesWithAnInaccessibleSetter()
              ;
        }
    }
}
