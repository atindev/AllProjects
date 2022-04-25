using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class VideoCategory:Entity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the training videos.
        /// </summary>
        /// <value>
        /// The training videos.
        /// </value>
        public virtual ICollection<TrainingVideos> TrainingVideos { get; set; }

        /// <summary>
        /// Gets or sets the category description.
        /// </summary>
        /// <value>
        /// The category description.
        /// </value>
        public string CategoryDescription { get; set; }

        /// <summary>
        /// Gets or sets the category thumbnail.
        /// </summary>
        /// <value>
        /// The category thumbnail.
        /// </value>
        public string CategoryThumbnail { get; set; }

        /// <summary>
        ///  Gets or sets the LanguageCode.
        /// </summary>
        /// <value>
        /// The LanguageCode
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
