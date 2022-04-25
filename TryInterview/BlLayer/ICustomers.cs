using System.Linq;
using TryInterview.Models.DbModels;

namespace TryInterview.BlLayer
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomers
    {
        /// <summary>
        /// Gets the customers.
        /// </summary>
        /// <returns></returns>
        IQueryable<Customer> GetCustomers();
    }
}
