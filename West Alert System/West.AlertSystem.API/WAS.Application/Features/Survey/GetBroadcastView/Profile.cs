namespace WAS.Application.Features.Survey.GetBroadcastView
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<Request,Subscription.GetAll.Request>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Request, Group.GetAll.Request>()
              .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
              .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
