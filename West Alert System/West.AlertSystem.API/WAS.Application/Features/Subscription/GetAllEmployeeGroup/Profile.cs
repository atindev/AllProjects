using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain;

namespace WAS.Application.Features.Subscription.GetAllEmployeeGroup
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<Domain.Entities.Subscription, WAS.Application.Common.Models.EmployeeGroup>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.EmployeeGroup));
                
        }
    }
}
