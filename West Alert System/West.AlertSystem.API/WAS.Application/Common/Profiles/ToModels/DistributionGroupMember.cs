using WAS.Application.Common.Models;

namespace WAS.Application.Common.Profiles.ToModels
{
    public class DistributionGroupMember : AutoMapper.Profile
    {
        public DistributionGroupMember()
        {
            CreateMap<ADPeople, Models.DistributionGroupMember>()
             .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
             .IgnoreAllPropertiesWithAnInaccessibleSetter()
             .ForMember(
                 dest => dest.EmailId,
                 opt => opt.MapFrom(src => src.EmailId))
              .ForMember(
                 dest => dest.FirstName,
                 opt => opt.MapFrom(src => src.FirstName));
        }
    }
}
