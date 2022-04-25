using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain;

namespace WAS.Application.Features.Location.GetAll
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<Domain.Entities.Location, Location>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(
                    dest => dest.CityId,
                    opt => opt.MapFrom(src => src.CityId))
                .ForMember(
                    dest => dest.Address,
                    opt => opt.MapFrom(src => src.Address))
                ;
        }
    }
}
