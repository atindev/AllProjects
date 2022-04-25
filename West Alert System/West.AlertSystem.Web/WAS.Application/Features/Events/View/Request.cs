using MediatR;
using System;

namespace WAS.Application.Features.Events.View
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Event id
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
