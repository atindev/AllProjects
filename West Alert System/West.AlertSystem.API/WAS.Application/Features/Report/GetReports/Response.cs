using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Report.GetReports
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the subscription mode count.
        /// </summary>
        /// <value>
        /// The subscription mode count.
        /// </value>
        public List<SubscriptionMode> SubscriptionModeCount { get; set; }

        /// <summary>
        /// Gets or sets the subscription mode count.
        /// </summary>
        /// <value>
        /// The subscription mode count.
        /// </value>
        public List<SubscriptionPerMonth> SubscriptionPerMonths { get; set; }

        /// <summary>
        /// Gets or sets the notification per months.
        /// </summary>
        /// <value>
        /// The notification per months.
        /// </value>
        public List<AllNotificationPerMonth> NotificationPerMonths { get; set; }

        /// <summary>
        /// Gets or sets all groups.
        /// </summary>
        /// <value>
        /// All groups.
        /// </value>
        public List<GroupSize> AllGroups { get; set; }

        /// <summary>
        /// Gets or sets all groups.
        /// </summary>
        /// <value>
        /// All groups.
        /// </value>
        public List<FeedbackChannelCount> FeedbackChannels { get; set; }

        /// <summary>
        /// Gets or sets the notification mode count.
        /// </summary>
        /// <value>
        /// The notification mode count.
        /// </value>
        public List<NotificationChannelCount> NotificationChannels { get; set; }

        /// <summary>
        /// Gets or sets the subscription location counts.
        /// </summary>
        /// <value>
        /// The subscription location counts.
        /// </value>
        public List<SubscriptionLocationCount> SubscriptionLocationCounts { get; set; }

        /// <summary>
        /// Gets or sets the subscriber and unsubscriber count per days.
        /// </summary>
        /// <value>
        /// The subscriber and unsubscriber count per days.
        /// </value>
        public List<SubscriberAndUnsubscriberCountPerDay> SubscriberAndUnsubscriberCountPerDays { get; set; }

        /// <summary>
        /// Gets or sets the subscription location percentage.
        /// </summary>
        /// <value>
        /// The subscription location percentage.
        /// </value>
        public LocationCountBySubscriptionPercentage SubscriptionLocationPercentage { get; set; }
    }
}
