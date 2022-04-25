using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Training.GetVideoById
{
    public class Response
    {
        /// <summary>
        /// Requested Training Video
        /// </summary>
        public TrainingVideo TrainingVideo { get; set; }

        /// <summary>
        /// Videos in same category
        /// </summary>
        public List<TrainingVideo> Videos { get; set; } = new List<TrainingVideo>();
    }
}
