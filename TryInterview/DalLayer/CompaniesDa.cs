using System.Linq;
using TryInterview.Models.DbModels;

namespace TryInterview.DalLayer
{
    public class CompaniesDa : ICompaniesDa
    {
        private readonly CompanyDbContext context;

        public CompaniesDa(CompanyDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Company> GetCompanies()
        {
            return this.context.Company.AsQueryable();
        }
    }
}
