using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Enum;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Subscription.GetAll
{
    public class Request : PagedRequest,IRequest<Response>
    { 
             /// <summary>
             /// query Condition
             /// </summary>
            public string condition { get; set; }

            /// <summary>
            /// Query rule from query builder
            /// </summary>
            public List<Rule> rules { get; set; }
         
    }
}
