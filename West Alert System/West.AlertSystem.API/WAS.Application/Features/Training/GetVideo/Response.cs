using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Training.GetVideo
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the training videos.
        /// </summary>
        /// <value>
        /// The training videos.
        /// </value>
        public List<VideoCategory> VideoCategories { get; set; }
            = new List<VideoCategory>();

        /// <summary>
        /// Gets or sets the languages.
        /// </summary>
        /// <value>
        /// The languages available in the videocategory
        /// </value>
        public IEnumerable<GlobalLanguage> AllLanguages { get; set; }
    }
}
