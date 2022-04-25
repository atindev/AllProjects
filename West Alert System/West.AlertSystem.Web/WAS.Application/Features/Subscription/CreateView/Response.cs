using System;
using System.Collections.Generic;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;

namespace WAS.Application.Features.Subscription.CreateView
{
    public class Response
    {
        /// <summary>
        /// Subscription
        /// </summary>
        public Common.Models.Subscription Subscription { get; set; }

        /// <summary>
        /// Locations list
        /// </summary>
        public List<Location> Locations { get; set; }

        /// <summary>
        /// Location Id
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Shift list
        /// </summary>
        public List<Shift> Shifts { get; set; }

        /// <summary>
        /// Shift Id
        /// </summary>
        public int? ShiftId { get; set; }

        /// <summary>
        /// AD User
        /// </summary>
        public ADUser ADUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Response Message
        /// </summary>
        public string ResponseMessage { get; set; }

        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Google Recaptcha properties
        /// </summary>
        public Recaptcha Recaptcha { get; set; }

        /// <summary>
        /// user blocked time interval
        /// </summary>
        public int UserBlockedInterval { get; set; }

        /// <summary>
        /// Gets or sets the languages.
        /// </summary>
        /// <value>
        /// The languages.
        /// </value>
        public List<Language> Languages { get; set; }
    }
}
