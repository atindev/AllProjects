using System.Net.Mime;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Notification.ViewAttachment
{
    public class Response : AttachmentData
    {
        /// <summary>
        /// ContentDisposition
        /// </summary>
        public ContentDisposition ContentDisposition { get; set; }
    }
}
