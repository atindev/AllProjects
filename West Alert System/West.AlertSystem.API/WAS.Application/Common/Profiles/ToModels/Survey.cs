
namespace WAS.Application.Common.Profiles.ToModels
{
    public class Survey : AutoMapper.Profile
    {
        public Survey()
        {
            CreateMap<Domain.Entities.Survey, Models.Survey>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                   dest => dest.BroadcastCount,
                   opt => opt.MapFrom(src => src.SurveyBroadcasts.Count))
               ;
        }
    }
}
