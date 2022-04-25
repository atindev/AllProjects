using System;

namespace WAS.Application.Features.Subscription.GetAll
{
    public class Subscription
    {
        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Location Id
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Official email id
        /// </summary>
        public string OfficialEmail { get; set; }

        /// <summary>
        /// Location name
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// shift Name
        /// </summary>
        public string Shift { get; set; }

        /// <summary>
        /// Gets or sets the subscribed on.
        /// </summary>
        /// <value>
        /// The subscribed on.
        /// </value>
        public string SubscribedOn { get; set; }

    }
}