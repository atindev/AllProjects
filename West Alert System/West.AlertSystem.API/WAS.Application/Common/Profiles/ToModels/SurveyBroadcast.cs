using System.Collections.Generic;
using System.Linq;
using WAS.Application.Common.Models;

namespace WAS.Application.Common.Profiles.ToModels
{
    public class SurveyBroadcast : AutoMapper.Profile
    {
        public SurveyBroadcast()
        {
            CreateMap<Domain.Entities.SurveyBroadcast, Features.Survey.GetByBroadcastId.Response>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ForMember(
                    dest => dest.SurveyId,
                    opt => opt.MapFrom(src => src.SurveyId))
                  .ForMember(
                    dest => dest.GroupId,
                    opt => opt.MapFrom(src => src.SurveyBroadcastGroups.Select(x => x.GroupId).ToList()))
                    .ForMember(
                    dest => dest.SubscriptionId,
                    opt => opt.MapFrom(src => src.SurveyBroadcastSubscriptions.Select(x => x.SubscriptionId).ToList()))
                  .ForMember(
                    dest => dest.FollowUpTime,
                    opt => opt.MapFrom(src => src.SurveyBroadcastFollowup.FollowUpDate))
                     .ForMember(
                           dest => dest.GroupNames,
                           opt => opt.MapFrom(src => src.SurveyBroadcastGroups.Select(s => s.Group.Name).ToList()))
                    .ForMember(
                        dest => dest.SubscriberNames,
                        opt => opt.MapFrom(src => src.SurveyBroadcastSubscriptions.Select(s => s.Subscription.LastName + ", " + s.Subscription.FirstName).ToList()))
                    .ForMember(dest => dest.AudienceCount,
                    opt => opt.MapFrom(src => src.TotalAudienceCount))
                    .ForMember(
                    dest => dest.DistributionGroups,
                    opt => opt.MapFrom(src => src.SurveyBroadcastDistributionGroups.Select(s => new DistributionGroup { Id = s.DistributionGroupId, EmailId = s.DistributionGroup, Name = s.DistributionGroupName }).ToList()))
                   .ForMember(
                    dest => dest.ADPeople,
                    opt => opt.MapFrom(src => src.SurveyBroadcastADUsers.Where(s => s.SurveyBroadcastDistributionGroupId == null).Select(s => new ADPeople { EmailId = s.EmailId, FirstName = s.FirstName, LastName = s.LastName, Location = s.Location.Name, Department = s.Department.Name }).ToList()));

            CreateMap<Domain.Entities.SurveyBroadcast, Models.BroadcastedSurvey>()
               .AddTransform<string>(o => string.IsNullOrWhiteSpace(o) ? null : o)
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               .ForMember(
                    dest => dest.SurveyId,
                    opt => opt.MapFrom(src => src.SurveyId))
               .ForMember(
                    dest => dest.Subject,
                    opt => opt.MapFrom(src => src.Subject))
               .ForMember(
                    dest => dest.GroupId,
                    opt => opt.MapFrom(src => src.SurveyBroadcastGroups.Select(x => x.GroupId).ToList()))
               .ForMember(
                    dest => dest.SubscriptionId,
                    opt => opt.MapFrom(src => src.SurveyBroadcastSubscriptions.Select(x => x.SubscriptionId).ToList()))
               .ForMember(
                    dest => dest.GroupNames,
                    opt => opt.MapFrom(src => src.SurveyBroadcastGroups.Select(s => s.Group.Name).ToList()))
               .ForMember(
                    dest => dest.SubscriberNames,
                    opt => opt.MapFrom(src => src.SurveyBroadcastSubscriptions.Select(s => s.Subscription.LastName + ", " + s.Subscription.FirstName).ToList()))
               .ForMember(
                    dest => dest.DistributionGroups,
                    opt => opt.MapFrom(src => src.SurveyBroadcastDistributionGroups.Select(s => new DistributionGroup { Id = s.DistributionGroupId, EmailId = s.DistributionGroup, Name = s.DistributionGroupName }).ToList()))
               .ForMember(
                    dest => dest.ADPeople,
                    opt => opt.MapFrom(src => src.SurveyBroadcastADUsers.Where(s => s.SurveyBroadcastDistributionGroupId == null).Select(s => new ADPeople { EmailId = s.EmailId, FirstName = s.FirstName, LastName = s.LastName, Location = s.Location.Name, Department = s.Department.Name }).ToList()));
        }
    }
}
