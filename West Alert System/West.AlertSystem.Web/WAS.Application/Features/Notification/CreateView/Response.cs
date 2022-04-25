using System;
using System.Collections.Generic;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using GetAllSubscriptions = WAS.Application.Features.Subscription.GetAll;

namespace WAS.Application.Features.Notification.CreateView
{
    public class Response:GroupList
    {
        /// <summary>
        /// Group Id
        /// </summary>
        public List<int> GroupId { get; set; }

        /// <summary>
        /// List of active events
        /// </summary>
        public List<Event> Events { get; set; }

        /// <summary>
        /// List of active Languages
        /// </summary>
        public List<Language> Languages { get; set; }

        /// <summary>
        /// Attachment File Properties
        /// </summary>
        public FileProperties FileProperties { get; set; }

        /// <summary>
        /// List of whatsapp template
        /// </summary>
        public List<WhatsAppTemplate> WhatsAppTemplates { get; set; }

        /// <summary>
        /// List of active Temlates
        /// </summary>
        public List<Common.Models.Template> Templates { get; set; }

        /// <summary>
        /// List of Template Categories
        /// </summary>
        public List<TemplateCategory> TemplateCategories { get; set; }

        /// <summary>
        /// Subscription list
        /// </summary>
        public List<GetAllSubscriptions.Subscription> Subscriptions { get; set; }

        /// <summary>
        /// Subscription Id
        /// </summary>
        public List<Guid> SubscriptionId { get; set; }

        /// <summary>
        /// Enable/Disable Approver question in Create Notification
        /// </summary>
        public bool EnableApprover { get; set; }

    }
}
