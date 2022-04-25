using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Survey.CreateUpdate
{
    public class Request : Common.Models.CreateSurvey,IRequest<Response>
    {

    }

}
