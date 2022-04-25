using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles.ToEntities
{
    public class NotificationEmail : AutoMapper.Profile
    {
        public NotificationEmail()
        {
            CreateMap<Features.Notification.Create.Request, Domain.Entities.NotificationEmail>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                    dest => dest.Body,
                    opt => opt.MapFrom(src => src.EmailBody))
                .ForMember(
                    dest => dest.Subject,
                    opt => opt.MapFrom(src => src.EmailSubject))
                ;
        }
    }
}
