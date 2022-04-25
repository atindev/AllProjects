using Microsoft.Graph;
namespace WAS.Application.Common.Models
{
    public class UserDataFromAd: Domain.Entities.Subscription
    {
        /// <summary>
        /// office location
        /// </summary>
        public string OfficeLocation { get; set; }

        /// <summary>
        /// Department
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// postal code
        /// </summary>
        public string PostalCodeFromAd { get; set; }

    }
}
