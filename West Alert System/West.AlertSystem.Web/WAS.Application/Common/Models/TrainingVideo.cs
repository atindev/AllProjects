namespace WAS.Application.Common.Models
{
    public class TrainingVideo
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string URL { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public string VideoDuration { get; set; }

        /// <summary>
        /// Gets or sets the video description.
        /// </summary>
        /// <value>
        /// The video description.
        /// </value>
        public string VideoDescription { get; set; }

        /// <summary>
        /// Gets or sets the video thumbnail.
        /// </summary>
        /// <value>
        /// The video thumbnail.
        /// </value>
        public string VideoThumbnail { get; set; }

        /// <summary>
        ///  Gets or sets the Language code.
        /// </summary>
        /// <value>
        /// The language code
        /// </value>
        public string LanguageCode { get; set; }
    }
}
