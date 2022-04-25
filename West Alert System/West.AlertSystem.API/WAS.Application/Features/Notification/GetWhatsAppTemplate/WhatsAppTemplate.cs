using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Notification.GetWhatsAppTemplate
{
   public class WhatsAppTemplate
    {
        /// <summary>
        /// Whats app template id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Whatsapp template name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whatsapp template description
        /// </summary>
        public string Description { get; set; }
    }
}
