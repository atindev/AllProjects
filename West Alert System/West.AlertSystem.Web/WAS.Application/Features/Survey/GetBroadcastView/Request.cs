using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Enum;

namespace WAS.Application.Features.Survey.GetBroadcastView
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Survey Id
        /// </summary>
        public Guid SurveyId { get; set; }

        /// <summary>
        /// user email
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// is having access to all groups
        /// </summary>
        public bool IsGlobalAdmin { get; set; }

        /// <summary>
        /// need to check access
        /// </summary>
        public bool IsAccessRequired { get; set; } = true;
    }
}
