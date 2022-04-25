using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Enum;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Notification.GetWhatsAppTemplate
{
    public class Request : PagedRequest, IRequest<Response>
    {
       /// <summary>
       /// Whatsapp template id
       /// </summary>
        public int Id { get; set; }

    }
}
