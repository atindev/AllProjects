using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToEntities
{
    public class NotificationText : AutoMapper.Profile
    {
        public NotificationText()
        {
            CreateMap<Features.Notification.Create.Request, Domain.Entities.NotificationText>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                    dest => dest.Message,
                    opt => opt.MapFrom(src => src.MessageText))
                ;
        }
    }
}
