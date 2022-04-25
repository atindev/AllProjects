using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Survey.Delete
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Survey Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User who last modified the record
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// survey type
        /// </summary>
        public string SurveyType { get; set; }

    }

}
