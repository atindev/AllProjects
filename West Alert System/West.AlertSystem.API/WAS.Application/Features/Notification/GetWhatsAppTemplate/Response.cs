using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Notification.GetWhatsAppTemplate
{
    public class Response
    {
        /// <summary>
        /// Get all WhatappTemplate response object
        /// </summary>
        public List<WhatsAppTemplate> WhatsAppTemplates { get; set; }
            = new List<WhatsAppTemplate>();
    }
}
