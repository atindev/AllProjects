using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToEntities
{
    public class NotificationWhatsApp : AutoMapper.Profile
    {
        public NotificationWhatsApp()
        {
            CreateMap<Features.Notification.Create.Request, Domain.Entities.NotificationWhatsApp>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                    dest => dest.Message,
                    opt => opt.MapFrom(src => src.WhatsAppMessage))
                ;
        }
    }
}
