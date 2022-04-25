using Microsoft.Graph;
namespace WAS.Application.Common.Models
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
        /// Token Authority
        /// </summary>
        public string TokenAuthority { get; set; }

        /// <summary>
        /// Graph Resource URL
        /// </summary>
        public string GraphResourceURL { get; set; }

        /// <summary>
        /// Gets or sets the application resource identifier.
        /// </summary>
        /// <value>
        /// The application resource identifier.
        /// </value>
        public string AppResourceId { get; set; }

    }
}
