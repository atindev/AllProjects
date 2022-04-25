using System.Collections.Generic;

namespace WAS.Application.Features.IncomingMessage.GetAll
{
    public class Response
    {
        /// <summary>
        /// List of All IncomingMessage
        /// </summary>
        public List<Common.Models.IncomingMessage> IncomingMessages { get; set; }

        /// <summary>
        /// Total Number of IncomingMessage
        /// </summary>
        public int Count { get; set; }
    }
}
