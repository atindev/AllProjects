using System.Linq;

namespace WAS.Application.Common.Profiles.ToModels
{
    public class Notification: AutoMapper.Profile
    {
        public Notification()
        {
            CreateMap<Domain.Entities.Notification, Models.Notification>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                    dest => dest.EventName,
                    opt => opt.MapFrom(src => src.Event.Name))
                .ForMember(
                    dest => dest.EventStatus,
                    opt => opt.MapFrom(src => src.Event.Status))
                .ForMember(
                    dest => dest.EventType,
                    opt => opt.MapFrom(src => src.Event.EventType.Name))
                .ForMember(
                    dest => dest.EventUrgency,
                    opt => opt.MapFrom(src => src.Event.EventUrgency.Name))
                .ForMember(
                    dest => dest.EventDescription,
                    opt => opt.MapFrom(src => src.Event.Description))
                 .ForMember(
                    dest => dest.EventCreatedDate,
                    opt => opt.MapFrom(src => src.Event.CreatedDate))
                .ForMember(
                    dest => dest.TextMessage,
                    opt => opt.MapFrom(src => src.NotificationText.Message))
                .ForMember(
                    dest => dest.WhatsAppMessage,
                    opt => opt.MapFrom(src => src.NotificationWhatsApp.Message))
                .ForMember(
                    dest => dest.VoiceMessage,
                    opt => opt.MapFrom(src => src.NotificationVoice.Message))
                .ForMember(
                    dest => dest.VoiceRepeatCount,
                    opt => opt.MapFrom(src => src.NotificationVoice.RepeatCount))
                .ForMember(
                    dest => dest.EmailSubject,
                    opt => opt.MapFrom(src => src.NotificationEmail.Subject))
                .ForMember(
                    dest => dest.EmailMessage,
                    opt => opt.MapFrom(src => src.NotificationEmail.Body))
                .ForMember(
                    dest => dest.GroupId,
                    opt => opt.MapFrom(src => src.NotificationGroups.Select(s => s.Id)))
                .ForMember(
                    dest => dest.GroupNames,
                    opt => opt.MapFrom(src => src.NotificationGroups.Select(s => s.Group.Name).ToList()))
                .ForMember(
                    dest => dest.IncomingMessages,
                    opt => opt.MapFrom(src => src.IncomingMessages))
                .ForMember(
                    dest => dest.ResponseCount,
                    opt => opt.MapFrom(src => src.IncomingMessages.Count))
                .ForMember(
                    dest => dest.SubscriberNames,
                    opt => opt.MapFrom(src => src.NotificationSubscriptions.Select(s => s.Subscription.LastName + ", " + s.Subscription.FirstName).ToList()))
                ;
        }
    }
}
