using System;

namespace WAS.Application.Common.Models
{
    public class AttachmentDetails
    {
        
        /// <summary>
        /// Attachment Name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Attachment url
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Attachment content type
        /// </summary>
        public string ContentType { get; set; }

    }
}
