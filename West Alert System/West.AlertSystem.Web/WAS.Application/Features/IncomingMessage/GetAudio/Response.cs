using System.Net.Mime;

namespace WAS.Application.Features.IncomingMessage.GetAudio
{
    public class Response
    {
        /// <summary>
        /// Audio Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// ContentDisposition
        /// </summary>
        public ContentDisposition ContentDisposition { get; set; }
    }
}
