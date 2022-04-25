using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Enum;

namespace WAS.Application.Features.Notification.CreateView
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// user email
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// is having access to all groups
        /// </summary>
        public bool IsGlobalAdmin { get; set; }
    }
}
