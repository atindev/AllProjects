using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToEntities
{
    public class SurveyBroadcastTeams : AutoMapper.Profile
    {
        public SurveyBroadcastTeams()
        {
            CreateMap<Features.Survey.Broadcast.Request, Domain.Entities.SurveyBroadcastTeams>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
