namespace WAS.Application.Common.Profiles.ToEntities
{
    public class ConversationSubscription : AutoMapper.Profile
    {
        public ConversationSubscription()
        {
            CreateMap<Features.Subscription.ConversationSubscription.Request, Domain.Entities.Subscription>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                ;
        }
    }
}
