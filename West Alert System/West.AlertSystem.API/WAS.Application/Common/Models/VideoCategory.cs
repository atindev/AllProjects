using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class VideoCategory
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
        public List<TrainingVideo> TrainingVideos { get; set; } = new List<TrainingVideo>();

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
        ///  Gets or sets the Language code.
        /// </summary>
        /// <value>
        /// The language code
        /// </value>
        public string LanguageCode { get; set; }
    }
}
