using System;

namespace WAS.Application.Features.Survey.SubmitSurvey
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<Request, Common.Models.SubmitSurvey>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               .ForMember(
                   dest => dest.SurveyCompletionTime,
                   opt => opt.MapFrom(src => SurveyCompletionTime(src.SurveyStartTime, src.SurveyEndTime)))
               ;
        }

        private int SurveyCompletionTime(DateTime? SurveyStartTime, DateTime? SurveyEndTime) 
        {
            TimeSpan ts = (TimeSpan)(SurveyEndTime - SurveyStartTime);
            return (int)ts.TotalSeconds;
        }
    }
}
