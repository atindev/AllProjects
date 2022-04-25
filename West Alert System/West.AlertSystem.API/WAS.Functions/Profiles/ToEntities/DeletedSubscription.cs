using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain;

namespace WAS.Functions.Profiles.ToEntities
{
    public class DeletedSubscription : AutoMapper.Profile
    {
        public DeletedSubscription()
        { 
            CreateMap<Domain.Entities.Subscription, Domain.Entities.DeletedSubscription>()
             .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
             .IgnoreAllPropertiesWithAnInaccessibleSetter()
             ;
        }
    }
}
