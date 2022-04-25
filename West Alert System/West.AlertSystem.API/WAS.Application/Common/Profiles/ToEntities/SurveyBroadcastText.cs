using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToEntities
{
    public class SurveyBroadcastText : AutoMapper.Profile
    {
        public SurveyBroadcastText()
        {
            CreateMap<Features.Survey.Broadcast.Request, Domain.Entities.NotificationText>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
