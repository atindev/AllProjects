using Microsoft.Graph;
namespace WAS.Application.Common.Models
{
    public class DeliveryReportStatus
    {
        /// <summary>
        /// Error message
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Sending Number
        /// </summary>
        public string ToNumber { get; set; }

        /// <summary>
        /// Error Code
        /// </summary>
        public int? ErrorCode { get; set; }

    }
}
