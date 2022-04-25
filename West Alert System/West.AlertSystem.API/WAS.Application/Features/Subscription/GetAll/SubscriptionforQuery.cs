using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.GetAll
{
    public class SubscriptionforQuery : Domain.Entities.Subscription
    {
         
        /// <summary>
        /// Location name
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// shift Name
        /// </summary>
        public string ShiftName { get; set; }

        /// <summary>
        /// city Id
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// State Id
        /// </summary>
        public int StateId { get; set; }

        /// <summary>
        /// Country Id
        /// </summary>
        public int CountryId { get; set; }
    }
}
