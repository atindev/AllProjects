using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WAS.Application.Common.Models;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Survey.GetAllSharedSurvey
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Gets or sets the official mail.
        /// </summary>
        /// <value>
        /// The official mail.
        /// </value>
        public string OfficialMail { get; set; }
    }
}
