using System.ComponentModel.DataAnnotations;

namespace WAS.Application.Common.Models
{
    public class ConversationSubscriptionData : ADUser
    {
        /// <summary>
        /// Upn
        /// </summary>
        public string Upn { get; set; }

        /// <summary>
        /// Mobile Number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Mobile Type
        /// </summary>
        public string PhoneType { get; set; }

        /// <summary>
        /// Is SMS enabled
        /// </summary>
        public string IsSMS { get; set; }

        /// <summary>
        /// Is Voice enabled
        /// </summary>
        public string IsVoice { get; set; }

        /// <summary>
        /// Is WhatsApp enabled
        /// </summary>
        public string IsWhatsApp { get; set; }

        /// <summary>
        /// Shift ID
        /// </summary>
        public string ShiftID { get; set; }
    }
}
