using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.BlockUser
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
        /// User Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Attempted ON
        /// </summary>
        public string AttemptON { get; set; }

        /// <summary>
        /// Attempted from
        /// </summary>
        public string AttemptFrom { get; set; }
    }
}
