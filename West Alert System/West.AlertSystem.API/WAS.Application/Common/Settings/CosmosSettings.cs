using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Settings
{
    public class CosmosSettings
    {
        /// <summary>
        /// EndpointUri
        /// </summary>
        public string EndpointUri { get; set; }

        /// <summary>
        /// PrimaryKey
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// Survey database 
        /// </summary>
        public string SurveyDatabaseId { get; set; }

        /// <summary>
        /// Survey Submission container
        /// </summary>
        public string SurveySubmissionContainer { get; set; }

        /// <summary>
        /// ApplicationName
        /// </summary>
        public string ApplicationName { get; set; }
    }
}
