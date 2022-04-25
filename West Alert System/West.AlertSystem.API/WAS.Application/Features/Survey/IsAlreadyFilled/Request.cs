using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Survey.IsAlreadyFilled
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// employee Id
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Survey broadcast id
        /// </summary>
        public Guid BroadcastId { get; set; }
    }
}
