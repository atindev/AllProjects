using System.Linq;
using TryInterview.DalLayer;
using TryInterview.Models.DbModels;

namespace TryInterview.BlLayer
{
    public class Customers : ICustomers
    {
        public readonly ICustomersDa CustomerDa;

        public Customers(ICustomersDa CustomerDa)
        {
            this.CustomerDa = CustomerDa;
        }

        public IQueryable<Customer> GetCustomers()
        {
            return this.CustomerDa.GetCustomers();
        }
    }
}
