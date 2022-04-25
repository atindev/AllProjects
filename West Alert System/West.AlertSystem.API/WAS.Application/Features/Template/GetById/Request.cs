using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Template.GetById
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Template Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
