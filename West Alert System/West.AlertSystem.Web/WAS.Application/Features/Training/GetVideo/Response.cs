using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Training.GetVideo
{
    public class Response
    {
        /// <summary>
        /// Gets or sets the video categories.
        /// </summary>
        /// <value>
        /// The video categories.
        /// </value>
        public List<VideoCategory> VideoCategories { get; set; }
             = new List<VideoCategory>();

        /// <summary>
        /// Gets or sets the languages.
        /// </summary>
        /// <value>
        /// The languages.
        /// </value>
        public List<GlobalLanguage> AllLanguages { get; set; }
    }
}
