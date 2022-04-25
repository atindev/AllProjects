using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Group.Delete
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// group Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User who last modified the record
        /// </summary>
        public string ModifiedBy { get; set; }

    }

}
