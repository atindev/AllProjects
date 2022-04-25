using MediatR;
using System;

namespace WAS.Application.Features.Survey.GetAllSubmitted
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Survey broadcast Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// only subscriber ids
        /// </summary>
        public bool IsOnlySubscriberIds { get; set; }
    }
}
