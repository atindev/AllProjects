using recentNotification = WAS.Application.Features.Notification.GetAll;
using incomingMessage = WAS.Application.Features.IncomingMessage.GetAll;

namespace WAS.Application.Features.Dashboard.GetDashboard
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<Request,recentNotification.Request>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Request, incomingMessage.Request>()
              .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
              .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
