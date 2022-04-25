namespace WAS.Application.Common.Profiles.ToModels
{
    public class VideoCategory : AutoMapper.Profile
    {
        public VideoCategory()
        {
            CreateMap<Domain.Entities.VideoCategory, Models.VideoCategory>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               ;

            CreateMap<Domain.Entities.TrainingVideos, Models.TrainingVideo>()
              .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
              .IgnoreAllPropertiesWithAnInaccessibleSetter()
              ;
        }
    }
}
