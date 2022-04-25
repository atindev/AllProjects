using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Notification.GetWhatsAppTemplate
{
    public class Response
    {
        /// <summary>
        /// List of whatsapp template
        /// </summary>
        public List<WhatsAppTemplate> WhatsAppTemplates { get; set; }
    }
}
