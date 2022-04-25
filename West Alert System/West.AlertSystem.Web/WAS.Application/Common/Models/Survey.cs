using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class Survey
    {
        /// <summary>
        /// Survey unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Survey name
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// User who created the record
        /// </summary>
        public string CreatedBy { get; set; }
         
        /// <summary>
        /// Survey description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// When this survey is last updated
        /// </summary>
        public string Updated { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Modified date
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Creater location
        /// </summary>
        public string CreaterLocation { get; set; }

        /// <summary>
        /// Question Count
        /// </summary>
        public int NumberofQuestions { get; set; }

        /// <summary>
        /// for Owner comparison
        /// </summary>
        public string OwnerWithoutSpecialCharacter { get; set; }

        /// <summary>
        /// broadcast Count
        /// </summary>
        public int BroadcastCount { get; set; }
    }
}
