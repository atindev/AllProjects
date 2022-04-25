using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToEntities
{
    public class SurveyBroadcastEmail : AutoMapper.Profile
    {
        public SurveyBroadcastEmail()
        {
            CreateMap<Features.Survey.Broadcast.Request, Domain.Entities.SurveyBroadcastEmail>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
