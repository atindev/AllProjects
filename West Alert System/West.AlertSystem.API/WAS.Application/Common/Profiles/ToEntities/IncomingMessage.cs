namespace WAS.Application.Common.Profiles.ToEntities
{
    public class IncomingMessage : AutoMapper.Profile
    {
        public IncomingMessage()
        {
            CreateMap<Features.IncomingMessage.Add.Request, Domain.Entities.IncomingMessage>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               .ForMember(
                    dest => dest.IsText,
                    opt => opt.MapFrom(src => !src.FromPhone.Contains("whatsapp:") && src.IsVoice != "true"))
               .ForMember(
                    dest => dest.IsWhatsApp,
                    opt => opt.MapFrom(src => src.FromPhone.Contains("whatsapp:")))
               .ForMember(
                    dest => dest.IsVoice,
                    opt => opt.MapFrom(src => src.IsVoice == "true"))
               .ForMember(
                    dest => dest.FromPhone,
                    opt => opt.MapFrom(src => MobileNumberMapping(src)))
               ;
        }
        private static string MobileNumberMapping(Features.IncomingMessage.Add.Request src)
        {
            if (src.FromPhone.Contains("whatsapp:"))
            {
                int position = src.FromPhone.IndexOf(":");

                return src.FromPhone[(position + 1)..];
            }
            else
            {
                return src.FromPhone;
            }
        }
    }
}
