using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Notification.GetFailedDetails
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Notification ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Messsage,call,whatsapp
        /// </summary>
        public string PublishingType { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

    }
}
