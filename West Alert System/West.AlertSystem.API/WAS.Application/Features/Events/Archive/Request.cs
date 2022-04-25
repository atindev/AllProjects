using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Events.Archive
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Event unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User who last modified the record
        /// </summary>
        public string ModifiedBy { get; set; }

    }

}
