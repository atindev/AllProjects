using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Survey.SubmitSurvey
{
    public class Request : Common.Models.SubmitSurvey, IRequest<Response>
    {
        /// <summary>
        /// employee id
        /// </summary>
        public string EmployeeId { get; set; }
    }
}
