using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Enum;

namespace WAS.Application.Common.Models
{
    public class BroadcastSurveyResponse
    {
        /// <summary>
        /// broadcast survey unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Is broadcast success
        /// </summary>
        public bool Success { get; set; }
    }
}
