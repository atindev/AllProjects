using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Template.Create
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<Request,  Common.Models.CreateTemplateCategory>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               .ForMember(
                   dest => dest.Id,
                   opt => opt.MapFrom(src => src.CategoryId))
               .ForMember(
                   dest => dest.Name,
                   opt => opt.MapFrom(src => src.CategoryName))
               .ForMember(
                   dest => dest.CreatedBy,
                   opt => opt.MapFrom(src => src.CreatedBy))
               .ForMember(
                   dest => dest.ModifiedBy,
                   opt => opt.MapFrom(src => src.CreatedBy))
               ;
        }
    }
}
