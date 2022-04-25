using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Training.GetVideoById
{
    public class Request : IRequest<Response>
    {
        public int videoId { get; set; }
    }
}
