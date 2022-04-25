using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAS.Web.Models;
using EventCreate = WAS.Application.Features.Events.CreateUpdate;
using Archive = WAS.Application.Features.Events.Archive;


namespace WAS.Web.Profile
{
    public class Event : AutoMapper.Profile
    {
        public Event()
        {
            CreateMap<EventViewModel, EventCreate.Request>()
                .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<EventArchiveModel, Archive.Request>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter();


        }
    }
}
