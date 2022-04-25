using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAS.Application.Features.Survey.CloneSurvey
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Survey id to clone
        /// </summary>
        public Guid SurveyId { get; set; }

        /// <summary>
        /// Emailid of cloning person 
        /// </summary>
        public string EmailId { get; set; }

    }
}
