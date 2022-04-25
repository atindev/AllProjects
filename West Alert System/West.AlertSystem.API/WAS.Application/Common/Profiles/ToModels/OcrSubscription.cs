namespace WAS.Application.Common.Profiles.ToModels
{
    public class OcrSubscription : AutoMapper.Profile
    {
        public OcrSubscription()
        {
            CreateMap<Domain.Entities.OcrSubscription, Models.OcrSubscription>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
                ;
        }
    }
}
