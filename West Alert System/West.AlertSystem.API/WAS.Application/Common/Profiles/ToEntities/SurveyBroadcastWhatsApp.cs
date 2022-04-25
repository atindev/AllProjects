using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToEntities
{
    public class SurveyBroadcastWhatsApp : AutoMapper.Profile
    {
        public SurveyBroadcastWhatsApp()
        {
            CreateMap<Features.Survey.Broadcast.Request, Domain.Entities.SurveyBroadcastWhatsApp>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
