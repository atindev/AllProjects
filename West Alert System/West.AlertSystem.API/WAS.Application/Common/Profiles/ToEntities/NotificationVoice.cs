using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToEntities
{
    public class NotificationVoice : AutoMapper.Profile
    {
        public NotificationVoice()
        {
            CreateMap<Features.Notification.Create.Request, Domain.Entities.NotificationVoice>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                    dest => dest.Message,
                    opt => opt.MapFrom(src => src.VoiceMessage))
                .ForMember(
                    dest => dest.RepeatCount,
                    opt => opt.MapFrom(src => src.VoiceRepeatCount))
                ;
        }
    }
}
