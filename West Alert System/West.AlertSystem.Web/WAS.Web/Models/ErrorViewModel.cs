namespace WAS.Web.Models
{
    public class ErrorViewModel
    {
        /// <summary>
        /// RequestId
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Show RequestId?
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// <summary>
        /// Error StatusCode
        /// </summary>
        public int? ErrorStatusCode { get; set; }

        /// <summary>
        /// Error Type
        /// </summary>
        public string ErrorType { get; set; }

        /// <summary>
        /// Error Message
        /// </summary>
        public string ErrorMessage { get; set; }

    }
}
