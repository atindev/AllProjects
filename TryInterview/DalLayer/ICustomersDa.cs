using System.Linq;
using TryInterview.Models.DbModels;

namespace TryInterview.DalLayer
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomersDa
    {
        /// <summary>
        /// Gets the customers.
        /// </summary>
        /// <returns></returns>
        IQueryable<Customer> GetCustomers();
    }
}
