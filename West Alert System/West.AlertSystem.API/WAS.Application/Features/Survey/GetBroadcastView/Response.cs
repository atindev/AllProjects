using System;
using System.Collections.Generic;

namespace WAS.Application.Features.Survey.GetBroadcastView
{
    public class Response 
    {
        /// <summary>
        /// Subscription list
        /// </summary>
        public List<Subscription.GetAll.Subscription> Subscriptions { get; set; }

        /// <summary>
        /// all groups
        /// </summary>
        public List<Group.GetAll.Group> Groups { get; set; }
         
        /// <summary>
        /// Survey Id
        /// </summary>
        public Guid SurveyId { get; set; }

        /// <summary>
        /// Survey Name
        /// </summary>
        public string SurveyName { get; set; }
    }
}
