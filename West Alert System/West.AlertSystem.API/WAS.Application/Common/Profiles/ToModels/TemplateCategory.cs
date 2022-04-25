
namespace WAS.Application.Common.Profiles.ToModels
{
    public class TemplateCategory : AutoMapper.Profile
    {
        public TemplateCategory()
        {
            CreateMap<Domain.Entities.TemplateCategory, Models.TemplateCategory>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               ;
        }
    }
}
