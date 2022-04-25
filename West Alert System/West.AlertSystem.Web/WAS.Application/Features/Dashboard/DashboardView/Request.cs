using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Dashboard.DashboardView
{
    public class Request : IRequest<Response>
    {
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
