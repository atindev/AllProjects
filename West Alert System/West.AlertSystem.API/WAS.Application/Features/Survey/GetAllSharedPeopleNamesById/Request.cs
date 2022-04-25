using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WAS.Application.Common.Models;
using WAS.Domain.Enum;

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
