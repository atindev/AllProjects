using MediatR;
using System;
using System.Collections.Generic;
using WAS.Domain.Enum;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Survey.Broadcast
{
    public class Request : BroadcastSurveyRequest, IRequest<Response>
    {
        
    }
}
