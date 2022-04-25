using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Survey.CheckAudience
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// User Email
        /// </summary>
        public string OfficialEmail { get; set; }

        /// <summary>
        /// Employee Id
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Survey Broadcast Id
        /// </summary>
        public Guid BroadcastId { get; set; }
    }
}
