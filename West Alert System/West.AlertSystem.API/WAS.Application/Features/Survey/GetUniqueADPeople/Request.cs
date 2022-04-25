using MediatR;
using System;
using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Survey.GetUniqueADPeople
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Survey Braodcast Id
        /// </summary>
        public Guid SurveyBroadcastId { get; set; }

        /// <summary>
        /// Unique Subscribers
        /// </summary>
        public List<string> UniqueSubscribers { get; set; }

        /// <summary>
        /// Distribution Groups
        /// </summary>
        public List<DistributionGroup> DistributionGroups { get; set; }

        /// <summary>
        /// AD Users
        /// </summary>
        public List<ADPeople> ADPeople { get; set; }

        /// <summary>
        /// Whether to save Distribution Group Members to database or not
        /// </summary>
        public bool ShouldSaveToDB { get; set; }
    }
}
