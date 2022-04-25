using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Settings
{
    public class GraphSettings
    {
        /// <summary>
        /// client Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client Secret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// TenantId
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// graph url
        /// </summary>
        public string GraphBaseURL { get; set; }

        /// <summary>
        /// AppResourceId
        /// </summary>
        public string AppResourceId { get; set; }

    }
}
