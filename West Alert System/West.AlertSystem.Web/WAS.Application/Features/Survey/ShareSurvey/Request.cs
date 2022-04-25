using MediatR;
using System;
using System.Collections.Generic;
using WAS.Application.Common.Enum;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Survey.ShareSurvey
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Survey Id
        /// </summary>
        public Guid BroadcastId { get; set; }

        /// <summary>
        /// Gets or sets the people mail.
        /// </summary>
        /// <value>
        /// The people mail.
        /// </value>
        public List<string> PeopleMail { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }
    }
}
