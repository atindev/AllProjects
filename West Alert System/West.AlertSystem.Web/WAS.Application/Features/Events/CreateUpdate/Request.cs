using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Events.CreateUpdate
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Event unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Event name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User who created the record
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the record
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Event type Id
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// Event urgency Id
        /// </summary>
        public int UrgencyId { get; set; }

        /// <summary>
        /// Event status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Event location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Event description
        /// </summary>
        public string Description { get; set; }
    }

}
