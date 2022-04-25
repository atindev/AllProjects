namespace WAS.Application.Common.Models
{
    public class GroupSubscribers
    {
        /// <summary>
        /// GroupName
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Group Subscriber Count
        /// </summary>
        public int GroupSubscriberCount { get; set; }

        /// <summary>
        /// TextSubscribersCount for group
        /// </summary>
        public int TextSubscribersCount { get; set; }

        /// <summary>
        /// VoiceSubscribersCount for group
        /// </summary>
        public int VoiceSubscribersCount { get; set; }

        /// <summary>
        /// EmailSubscribersCount for group
        /// </summary>
        public int EmailSubscribersCount { get; set; }

        /// <summary>
        /// WhatsAppSubscribersCount for group
        /// </summary>
        public int WhatsAppSubscribersCount { get; set; }
    }
}
