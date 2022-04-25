using System.Linq;
using TryInterview.Models.DbModels;

namespace TryInterview.DalLayer
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICompaniesDa
    {
        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <returns></returns>
        IQueryable<Company> GetCompanies();
    }
}
