using System.Linq;
using TryInterview.DalLayer;
using TryInterview.Models.DbModels;

namespace TryInterview.BlLayer
{
    public class Companies : ICompanies
    {
        public readonly ICompaniesDa CompaniesDa;

        public Companies(ICompaniesDa CompaniesDa)
        {
            this.CompaniesDa = CompaniesDa;
        }

        public IQueryable<Company> GetCompanies()
        {
            return this.CompaniesDa.GetCompanies();
        }
    }
}
