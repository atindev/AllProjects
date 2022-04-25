using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Template.Create
{
    public class Request : Common.Models.CreateTemplate,IRequest<Response>
    {

    }

}
