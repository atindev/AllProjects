using System.Linq;

namespace WAS.Application.Common.Profiles.ToModels
{
    public class DeliveryReportText : AutoMapper.Profile
    {
        public DeliveryReportText()
        {
            CreateMap<Domain.Entities.DeliveryReportText, Models.FailedNotification>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                    dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => src.CreatedDate.ToString("MMM dd, yyyy hh:mm:ss tt")))
                .ForMember(
                    dest => dest.SubscriberName,
                    opt => opt.MapFrom(src => $"{src.Subscription.FirstName} {src.Subscription.LastName}"))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status))
                .ForMember(
                    dest => dest.ErrorMessage,
                    opt => opt.MapFrom(src => src.ErrorMessage))
                .ForMember(
                    dest => dest.SubscriberEmail,
                    opt => opt.MapFrom(src => src.Subscription.OfficialEmail))
                .ForMember(
                    dest => dest.SubscriberId,
                    opt => opt.MapFrom(src => src.Subscription.Id))
                 .ForMember(
                    dest => dest.ToNumber,
                    opt => opt.MapFrom(src => src.ToNumber))
                  .ForMember(
                    dest => dest.ErrorCode,
                    opt => opt.MapFrom(src => (src.ErrorCode != null && src.ErrorCode != 0) ? src.ErrorCode.ToString() : null))
                ;
        }
    }
}
