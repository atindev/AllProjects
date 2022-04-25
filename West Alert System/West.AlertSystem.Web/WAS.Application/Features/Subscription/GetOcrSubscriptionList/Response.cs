using System.Collections.Generic;
using WAS.Application.Common.Models;
using Locations = WAS.Application.Features.Subscription.CreateView;

namespace WAS.Application.Features.Subscription.GetOcrSubscriptionList
{
    public class Response 
    {
        /// <summary>
        /// OcrSubscriptionList
        /// </summary>
        public List<OcrSubscription> OcrSubscriptionList { get; set; }

        /// <summary>
        /// Get all locations response object
        /// </summary>
        public List<Locations.Location> Locations { get; set; }

        /// <summary>
        /// Admin Location ID
        /// </summary>
        public int AdminLocationId { get; set; }
    }
}
