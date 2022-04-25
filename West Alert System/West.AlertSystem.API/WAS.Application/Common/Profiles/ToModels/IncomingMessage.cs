namespace WAS.Application.Common.Profiles.ToModels
{
    public class IncomingMessage : AutoMapper.Profile
    {
        public IncomingMessage()
        {
            CreateMap<Domain.Entities.IncomingMessage, Common.Models.IncomingMessage>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
                   ;
        }
    }
}
