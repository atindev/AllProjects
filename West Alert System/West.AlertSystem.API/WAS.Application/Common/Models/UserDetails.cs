using Microsoft.Graph;
namespace WAS.Application.Common.Models
{
    public class UserDetails
    {
        /// <summary>
        /// user detail from AD
        /// </summary>
        public UserDataFromAd User { get; set; } = new UserDataFromAd();

        /// <summary>
        /// user is present in AD or not
        /// </summary>
        public bool IsSubscriberPresent { get; set; } = false;
    }
}
