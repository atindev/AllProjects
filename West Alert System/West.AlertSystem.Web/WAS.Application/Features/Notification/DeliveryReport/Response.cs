using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Notification.DeliveryReport
{
    public class Response
    {
        /// <summary>
        /// Text Notification Delivery Status
        /// </summary>
        public List<DeliveryStatus> DeliveryStatusTexts { get; set; }

        /// <summary>
        /// Voice Notification Delivery Status
        /// </summary>
        public List<DeliveryStatus> DeliveryStatusVoices { get; set; }

        /// <summary>
        /// WhatsApp Notification Delivery Status
        /// </summary>
        public List<DeliveryStatus> DeliveryStatusWhatsApps { get; set; }
    }
}
