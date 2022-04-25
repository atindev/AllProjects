using GetAllSubscriptionPerMonth = WAS.Application.Features.Report.GetAllSubscriptionPerMonth;
using GetAllSubscriptionModeCount = WAS.Application.Features.Report.GetAllSubscriptionModeCount;
using GetAllNotificationPerMonth = WAS.Application.Features.Report.GetAllNotifiactionPerMonth;
namespace WAS.Application.Features.Report.GetReports
{
    public partial class Profile : ProfileExtension
    {
        public  Profile()
        {

            CreateMap<Request, GetAllSubscriptionPerMonth.Request>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Request, GetAllSubscriptionModeCount.Request>()
              .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
              .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Request, GetAllNotificationPerMonth.Request>()
              .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
              .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Request, GetAllGroupSize.Request>()
              .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
              .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Request, GetAllFeedbackChannelCount.Request>()
              .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
              .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Request, GetAllNotificationModeCount.Request>()
              .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
              .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Request, GetAllLocationCount.Request>()
              .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
              .IgnoreAllPropertiesWithAnInaccessibleSetter();

           
        }
    }

    public  class ProfileExtension : AutoMapper.Profile
    {
        public ProfileExtension()
        {
            CreateMap<Request, GetAllSubscriptionCountPerDay.Request>()
             .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
             .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Request, GetSubscriptionPercentageByLocation.Request>()
             .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
             .IgnoreAllPropertiesWithAnInaccessibleSetter();

        }
    }
}
