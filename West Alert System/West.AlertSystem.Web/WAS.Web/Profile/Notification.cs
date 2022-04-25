using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAS.Web.Models;
using CreateNotification = WAS.Application.Features.Notification.Create;

namespace WAS.Web.Profile
{
    public class Notification : AutoMapper.Profile
    {
        public Notification()
        {
            CreateMap<NotificationViewModel, CreateNotification.Request>()
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               .ForMember(
                   dest => dest.IsEmail,
                   opt => opt.MapFrom(src => src.IsEmail == "on"))
               .ForMember(
                   dest => dest.IsTeams,
                   opt => opt.MapFrom(src => src.IsTeams == "on"))
               .ForMember(
                   dest => dest.IsText,
                   opt => opt.MapFrom(src => src.IsText == "on"))
               .ForMember(
                   dest => dest.IsVoice,
                   opt => opt.MapFrom(src => src.IsVoice == "on"))
               .ForMember(
                   dest => dest.IsWhatsApp,
                   opt => opt.MapFrom(src => src.IsWhatsApp == "on"))
               ;
        }
    }
}
