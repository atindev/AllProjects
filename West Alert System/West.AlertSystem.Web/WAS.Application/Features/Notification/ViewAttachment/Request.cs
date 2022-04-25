using MediatR;
using System;

namespace WAS.Application.Features.Notification.ViewAttachment
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// NotificationEmailId
        /// </summary>
        public Guid NotificationEmailId { get; set; }

        /// <summary>
        /// Attachment Name
        /// </summary>
        public string FileName { get; set; }
    }
}
