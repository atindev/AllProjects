using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Notification.Create
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<Request, Features.Events.CreateUpdate.Request>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               .ForMember(
                   dest => dest.Id,
                   opt => opt.MapFrom(src => src.EventId))
               .ForMember(
                   dest => dest.Name,
                   opt => opt.MapFrom(src => src.MessageText))
               .ForMember(
                   dest => dest.CreatedBy,
                   opt => opt.MapFrom(src => src.CreatedBy))
               .ForMember(
                   dest => dest.ModifiedBy,
                   opt => opt.MapFrom(src => src.CreatedBy))
               .ForMember(
                   dest => dest.Status,
                   opt => opt.Ignore())
               ;
        }
    }
}
