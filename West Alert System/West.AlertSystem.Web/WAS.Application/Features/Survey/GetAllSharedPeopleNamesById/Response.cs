using System;
using System.Collections.Generic;
using GetAllSubscriptions = WAS.Application.Features.Subscription.GetAll;

namespace WAS.Application.Features.Survey.GetAllSharedPeopleNamesById
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the name of the people.
        /// </summary>
        /// <value>
        /// The name of the people.
        /// </value>
        public List<string> PeopleName { get; set; }
    }
}
