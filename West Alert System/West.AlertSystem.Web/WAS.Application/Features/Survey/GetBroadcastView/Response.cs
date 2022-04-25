using System;
using System.Collections.Generic;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using GetAllSubscriptions = WAS.Application.Features.Subscription.GetAll;

namespace WAS.Application.Features.Survey.GetBroadcastView
{
    public class Response
    { 
        /// <summary>
        /// Subscription list
        /// </summary>
        public List<GetAllSubscriptions.Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Groups list
        /// </summary>
        public List<Group> Groups { get; set; }

        /// <summary>
        /// Survey Id
        /// </summary>
        public Guid SurveyId { get; set; }

        /// <summary>
        /// Survey Name
        /// </summary>
        public string SurveyName { get; set; }

        /// <summary>
        /// Broadcast Id
        /// </summary>
        public Guid BroadcastId { get; set; }

        /// <summary>
        /// for updating existing survey
        /// </summary>
        public bool IsUpdate { get; set; }
    }
}
