using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain;

namespace WAS.Application.Features.Shift.GetAll
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<Domain.Entities.Shift, Shift>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.ShiftName))
                ;
        }
    }
}
