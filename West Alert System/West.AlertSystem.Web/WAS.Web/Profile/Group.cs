using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAS.Web.Models;
using CreateUpdate = WAS.Application.Features.Groups.CreateUpdate;
using AddSubscription = WAS.Application.Features.Groups.AddSubscription;
using GetDistinctSubscribers = WAS.Application.Features.Groups.GetDistinctSubscriberCount;


namespace WAS.Web.Profile
{
    public class Group : AutoMapper.Profile
    {
        public Group()
        {
            
            CreateMap<AddSubscriptionToGroupModel, CreateUpdate.Request>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<AddSubscriptionToGroupModel, AddSubscription.Request>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                   dest => dest.GroupId,
                   opt => opt.MapFrom(src => src.Id));

            CreateMap<GroupModel, GetDistinctSubscribers.Request>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
