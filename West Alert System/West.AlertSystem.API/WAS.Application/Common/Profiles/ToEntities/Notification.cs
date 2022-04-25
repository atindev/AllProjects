namespace WAS.Application.Common.Profiles.ToEntities
{
    public class Notification : AutoMapper.Profile
    {
        public Notification()
        {
            CreateMap<Features.Notification.Create.Request, Domain.Entities.Notification>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                ;
        }
    }
}
