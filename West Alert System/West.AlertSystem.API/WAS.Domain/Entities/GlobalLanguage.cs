using System.Collections.Generic;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class GlobalLanguage : Entity
    {
        /// <summary>
        /// Primary key of language
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// language code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// video categories
        /// </summary>
        public virtual ICollection<VideoCategory> VideoCategories { get; set; }

        /// <summary>
        /// Gets or sets the training videos.
        /// </summary>
        public virtual ICollection<TrainingVideos> TrainingVideos { get; set; }
    }
}
