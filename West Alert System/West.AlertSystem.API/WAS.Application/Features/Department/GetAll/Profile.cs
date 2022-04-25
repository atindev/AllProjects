namespace WAS.Application.Features.Department.GetAll
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<Domain.Entities.Department, WAS.Application.Common.Models.Department>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                ;
        }
    }
}
