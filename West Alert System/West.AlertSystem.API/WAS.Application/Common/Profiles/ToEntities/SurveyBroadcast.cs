namespace WAS.Application.Common.Profiles.ToEntities
{
    public class SurveyBroadcast : AutoMapper.Profile
    {
        public SurveyBroadcast()
        {
            CreateMap<Features.Survey.Broadcast.Request, Domain.Entities.SurveyBroadcast>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                ;

            CreateMap<Features.Survey.UpdateBroadcast.Request, Domain.Entities.SurveyBroadcast>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               ;
        }
    }
}
