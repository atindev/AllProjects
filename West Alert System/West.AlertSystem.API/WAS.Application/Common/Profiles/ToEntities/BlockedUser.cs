using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Profiles
{
    public class BlockedUser : AutoMapper.Profile
    {
        public BlockedUser()
        {
            CreateMap<Features.Subscription.BlockUser.Request, Domain.Entities.BlockedUser>()
                 .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                 .IgnoreAllPropertiesWithAnInaccessibleSetter()
                 .ForAllMembers(opts => opts.Condition((src, dest, srcmember) => srcmember != null))
                ;
        }
    }
}
