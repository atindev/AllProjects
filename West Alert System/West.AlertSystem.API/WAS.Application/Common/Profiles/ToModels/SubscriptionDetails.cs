namespace WAS.Application.Common.Profiles.ToModels
{
    class SubscriptionDetails : AutoMapper.Profile
    {
        public SubscriptionDetails()
        {
            CreateMap<Domain.Entities.Subscription, Common.Models.SubscriptionDetails>()
            .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
            .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
