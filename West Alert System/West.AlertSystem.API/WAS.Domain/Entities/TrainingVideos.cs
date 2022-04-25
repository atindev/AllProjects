using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class TrainingVideos:Entity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the video category.
        /// </summary>
        /// <value>
        /// The video category.
        /// </value>
        public virtual VideoCategory VideoCategory { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string URL { get; set; }

        /// <summary>
        /// Gets or sets the video thumbnail.
        /// </summary>
        /// <value>
        /// The video thumbnail.
        /// </value>
        public string VideoThumbnail { get; set; }

        /// <summary>
        /// Gets or sets the video description.
        /// </summary>
        /// <value>
        /// The video description.
        /// </value>
        public string VideoDescription { get; set; }

        /// <summary>
        /// Gets or sets the duration of the video.
        /// </summary>
        /// <value>
        /// The duration of the video.
        /// </value>
        public string VideoDuration { get; set; }

        /// <summary>
        ///  Gets or sets the Language code.
        /// </summary>
        /// <value>
        /// The language code
        /// </value>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public virtual GlobalLanguage Language { get; set; }

    }
}
