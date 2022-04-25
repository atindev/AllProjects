
namespace WAS.Application.Common.Profiles.ToModels
{
    public class Language : AutoMapper.Profile
    {
        public Language()
        {
            CreateMap<Domain.Entities.Language, Common.Models.Language>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               .ForMember(
                   dest => dest.Id,
                   opt => opt.MapFrom(src => src.Id))
               .ForMember(
                   dest => dest.Name,
                   opt => opt.MapFrom(src => src.Name))
               .ForMember(
                   dest => dest.Code,
                   opt => opt.MapFrom(src => src.Code))
               ;
        }
    }
}
