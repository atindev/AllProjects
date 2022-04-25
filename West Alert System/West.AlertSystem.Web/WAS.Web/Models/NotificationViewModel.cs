using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CreateNotification = WAS.Application.Features.Notification.Create;

namespace WAS.Web.Models
{
    public class NotificationViewModel : CreateNotification.Request
    {
        public NotificationViewModel()
        {
            EmailAttachment = new List<IFormFile>();
        }
        /// <summary>
        /// Event type
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// Is text(sms) notification
        /// </summary>
        public new string IsText { get; set; }

        /// <summary>
        /// Is email notification
        /// </summary>
        public new string IsEmail { get; set; }

        /// <summary>
        /// Is voice notification
        /// </summary>
        public new string IsVoice { get; set; }

        /// <summary>
        /// Is teams notification
        /// </summary>
        public new string IsTeams { get; set; }

        /// <summary>
        /// Is whatsapp notification
        /// </summary>
        public new string IsWhatsApp { get; set; }

        ///// <summary>
        ///// Email attachments
        ///// </summary>
        public List<IFormFile> EmailAttachment { get; set; }

        /// <summary>
        /// selected email attachment
        /// </summary>
        public new List<string> ExistingEmailAttachments { get; set; }

        /// <summary>
        /// EventName with timezone
        /// </summary>       
        public new string NewEventName { get; set; }
    }
}
