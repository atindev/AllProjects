using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Survey.GetSubjectByBroadcastId
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Survey broadcast Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
