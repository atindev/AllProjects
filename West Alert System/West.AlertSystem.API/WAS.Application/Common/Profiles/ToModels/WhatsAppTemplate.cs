namespace WAS.Application.Common.Profiles.ToModels
{
    public class WhatsAppTemplate : AutoMapper.Profile
    {
        public WhatsAppTemplate()
        {
            CreateMap<Domain.Entities.WhatsAppTemplate, Features.Notification.GetWhatsAppTemplate.WhatsAppTemplate>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
