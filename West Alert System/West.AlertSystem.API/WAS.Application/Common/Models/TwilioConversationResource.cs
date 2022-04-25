using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class TwilioConversationResource
    {
        /// <summary>
        /// To resource (ex: Mobile number or email id)
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Conversation SID
        /// </summary>
        public string ConversationSid { get; set; }
    }
}
