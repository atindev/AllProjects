namespace WAS.Application.Common.Profiles.ToEntities
{
    public class SubscriptionFeedback : AutoMapper.Profile
    {
        public SubscriptionFeedback()
        {
            CreateMap<Features.Subscription.SubscriptionFeedback.Request, Domain.Entities.SubscriptionFeedback>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               .ForMember(
                    dest => dest.FeedbackRating,
                    opt => opt.MapFrom(src => src.Feedback))
               ;
        }

    }
}
