using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class SurveyDetailShare : Entity
    {
        /// <summary>
        /// The identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the broadcast identifier.
        /// </summary>
        /// <value>
        /// The broadcast identifier.
        /// </value>
        public Guid BroadcastId { get; set; }

        /// <summary>
        /// Gets or sets the subscription identifier.
        /// </summary>
        /// <value>
        /// The subscription identifier.
        /// </value>
        public string OfficialMail { get; set; }

        /// <summary>
        /// Gets or sets the survey broadcast.
        /// </summary>
        /// <value>
        /// The survey broadcast.
        /// </value>
        public virtual SurveyBroadcast SurveyBroadcast { get; set;}

    }
}
