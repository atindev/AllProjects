
namespace WAS.Application.Common.Profiles.ToModels
{
    public class Event : AutoMapper.Profile
    {
        public Event()
        {
            CreateMap<Domain.Entities.Event, Common.Models.Event>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               .ForMember(
                   dest => dest.TypeName,
                   opt => opt.MapFrom(src => src.EventType.Name))
               .ForMember(
                   dest => dest.UrgencyName,
                   opt => opt.MapFrom(src => src.EventUrgency.Name))
               .ForMember(
                   dest => dest.NotificationCount,
                   opt => opt.MapFrom(src => src.Notifications.Count))
               ;
        }
    }
}
