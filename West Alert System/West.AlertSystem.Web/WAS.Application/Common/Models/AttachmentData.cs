using System;

namespace WAS.Application.Common.Models
{
    public class AttachmentData
    {
        /// <summary>
        /// Attachment EmailId
        /// </summary>
        public Guid NotificationEmailId { get; set; }

        /// <summary>
        /// Attachment Name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Attachment Data
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Attachment ContentType
        /// </summary>
        public string ContentType { get; set; }
    }
}
