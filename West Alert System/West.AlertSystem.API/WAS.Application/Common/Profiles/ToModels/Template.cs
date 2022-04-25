
namespace WAS.Application.Common.Profiles.ToModels
{
    public class Template : AutoMapper.Profile
    {
        public Template()
        {
            CreateMap<Domain.Entities.Template, Models.Template>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               ;
        }
    }
}
