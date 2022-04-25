﻿using MediatR;
using System;

namespace WAS.Application.Features.Survey.GetJsonByBroadcastId
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Survey broadcast Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
