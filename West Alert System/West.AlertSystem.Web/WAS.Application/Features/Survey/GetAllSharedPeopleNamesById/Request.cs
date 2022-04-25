using MediatR;
using System;
using System.Collections.Generic;
using WAS.Application.Common.Enum;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Survey.GetAllSharedPeopleNamesById
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Gets or sets the broadcast identifier.
        /// </summary>
        /// <value>
        /// The broadcast identifier.
        /// </value>
        public Guid BroadcastId { get; set; }
    }
}
