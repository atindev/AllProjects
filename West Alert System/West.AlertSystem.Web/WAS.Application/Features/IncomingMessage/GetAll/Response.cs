using System.Collections.Generic;

namespace WAS.Application.Features.IncomingMessage.GetAll
{
    public class Response
    {
        /// <summary>
        /// List of All Incoming Messages
        /// </summary>
        public List<Common.Models.IncomingMessage> IncomingMessages { get; set; }

        /// <summary>
        /// List of groups
        /// </summary>
        public Common.Models.GroupList GroupList { get; set; }
    }
}
