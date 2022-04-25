namespace WAS.Application.Common.Models
{
    public class TwilioVoice
    {
        /// <summary>
        /// From number
        /// </summary>
        public string FromNumber { get; set; }

        /// <summary>
        /// To number
        /// </summary>
        public string ToNumber { get; set; }

        /// <summary>
        /// Voice body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Voice RepeatCount
        /// </summary>
        public int RepeatCount { get; set; }

        /// <summary>
        /// Voice Language
        /// </summary>
        public string Language { get; set; }
    }
}
