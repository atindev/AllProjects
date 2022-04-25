namespace WAS.Application.Common.Profiles.ToModels
{
    public class SubmissionReportAudience : AutoMapper.Profile
    {
        public SubmissionReportAudience()
        {

            CreateMap<Models.Audience, Models.SubmissionReportUniqueAudience>()
                  .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                  .IgnoreAllPropertiesWithAnInaccessibleSetter()
                  .ForMember(
                      dest => dest.OfficialEmail,
                      opt => opt.MapFrom(src => src.SubscriberOfficialEmail))
                   ;

            CreateMap<Domain.Entities.SurveyBroadcastADUser, Models.SubmissionReportUniqueAudience>()
                  .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
                  .IgnoreAllPropertiesWithAnInaccessibleSetter()
                  .ForMember(
                      dest => dest.OfficialEmail,
                      opt => opt.MapFrom(src => src.EmailId))
                   .ForMember(
                      dest => dest.LocationName,
                      opt => opt.MapFrom(src => src.Location.Name))
                   .ForMember(
                      dest => dest.DepartmentName,
                      opt => opt.MapFrom(src => src.Department.Name))
                   ;
        }
    }
}
