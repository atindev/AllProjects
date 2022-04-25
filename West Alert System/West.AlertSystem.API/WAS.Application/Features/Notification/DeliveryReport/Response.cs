using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Notification.DeliveryReport
{
    public class Response
    {
        public Response()
        {
            DeliveryStatusTexts = new List<DeliveryStatus>();
            DeliveryStatusVoices = new List<DeliveryStatus>();
            DeliveryStatusWhatsApps = new List<DeliveryStatus>();
        }

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
