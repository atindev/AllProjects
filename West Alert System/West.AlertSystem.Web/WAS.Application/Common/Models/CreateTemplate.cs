using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace WAS.Application.Common.Models
{
    public class CreateTemplate
    {
        /// <summary>
        /// Template unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Template Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User who created the record
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the record
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Group Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Category Id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Is text(sms) notification
        /// </summary>
        public bool IsText { get; set; } = false;

        /// <summary>
        /// Is email notification
        /// </summary>
        public bool IsEmail { get; set; } = false;

        /// <summary>
        /// Is voice notification
        /// </summary>
        public bool IsVoice { get; set; } = false;

        /// <summary>
        /// Is teams notification
        /// </summary>
        public bool IsTeams { get; set; } = false;

        /// <summary>
        /// Is whatsapp notification
        /// </summary>
        public bool IsWhatsApp { get; set; } = false;

        /// <summary>
        /// Notification message body
        /// </summary>
        public string MessageText { get; set; } = "";

        /// <summary>
        /// Notification voice body
        /// </summary>
        public string VoiceMessage { get; set; } = "";

        /// <summary>
        /// Notification Voice Language
        /// </summary>
        public string VoiceLanguage { get; set; } = "";

        /// <summary>
        /// Notification Voice Repeat count
        /// </summary>
        public int VoiceRepeatCount { get; set; }

        /// <summary>
        /// Email subject
        /// </summary>
        public string EmailSubject { get; set; } = "";

        /// <summary>
        /// Email body
        /// </summary>
        public string EmailBody { get; set; } = "";

        /// <summary>
        /// Notification whats app message body
        /// </summary>       
        public string WhatsAppMessage { get; set; } = "";

        /// <summary>
        /// whatsapp template Id 
        /// </summary>       
        public string WhatsAppTemplateId { get; set; } = "";

        /// <summary>
        /// Group Ids to send
        /// </summary>
        public List<int> GroupId { get; set; }

        /// <summary>
        /// Subscriber Ids to send
        /// </summary>
        public List<Guid> SubscriberId { get; set; }

        /// <summary>
        /// Event id
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Is for New Event
        /// </summary>
        public bool IsNewEvent { get; set; } = false;

        /// <summary>
        /// Is Approval Required
        /// </summary>
        public bool IsApprovalRequired { get; set; } = false;

        /// <summary>
        /// Category Name
        /// </summary>       
        public string CategoryName { get; set; } = "";

        /// <summary>
        /// private notification approver
        /// </summary>       
        public string ApproverForPrivate { get; set; }

        /// <summary>
        /// Is private notification
        /// </summary>
        public bool IsPrivateNotification { get; set; }

        ///// <summary>
        ///// Email attachments
        ///// </summary>
        public List<AttachmentData> EmailAttachments { get; set; }

        /// <summary>
        /// selected email attachment
        /// </summary>
        public List<string> ExistingEmailAttachments { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read confirmation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is read confirmation; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadConfirmation { get; set; }

        /// <summary>
        /// is this a notification
        /// </summary>
        public bool IsNotification { get; set; }

        /// <summary>
        /// Is creator Signature Required in Notification
        /// </summary>
        public bool IsSignatureRequired { get; set; }
    }
}
