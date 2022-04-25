using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class Location : Entity
    {
        /// <summary>
        /// Primary key of location
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// City Id
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Name of the location
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Location address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Twilio Sender Phone Number based on Location
        /// </summary>
        public string CountryPhoneNumber { get; set; }

        /// <summary>
        /// Collection of group
        /// </summary>
        public virtual ICollection<Group> Groups { get; set; }


        /// <summary>
        /// Collection of subscriptions
        /// </summary>
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Collection of SurveyBroadcast ADUsers
        /// </summary>
        public virtual ICollection<SurveyBroadcastADUser> SurveyBroadcastADUsers { get; set; }

        /// <summary>
        /// Gets or sets the cities.
        /// </summary>
        /// <value>
        /// The cities.
        /// </value>
        public virtual City City{ get; set; }

    }
}
