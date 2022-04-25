using System.Linq;
using TryInterview.Models.DbModels;

namespace TryInterview.DalLayer
{
    public class CustomersDa : ICustomersDa
    {
        private readonly CompanyDbContext context;

        public CustomersDa(CompanyDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Customer> GetCustomers()
        {
            return this.context.Customer.AsQueryable();
        }
    }
}
