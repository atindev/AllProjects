using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Events.GetById
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Event Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// email id of the person
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// is having access to all private notification
        /// </summary>
        public bool IsGlobalAdmin { get; set; }
    }
}
