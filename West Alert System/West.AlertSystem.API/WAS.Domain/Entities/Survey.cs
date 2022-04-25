using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class Survey : Entity
    {
        /// <summary>
        /// Primary key of Survey
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the Survey
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Survey Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Survey Path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Question Count
        /// </summary>
        public int? NumberofQuestions { get; set; }

        /// <summary>
        /// collection of SurveyBroadcast
        /// </summary>
        public virtual ICollection<SurveyBroadcast> SurveyBroadcasts { get; set; }
    }
}
