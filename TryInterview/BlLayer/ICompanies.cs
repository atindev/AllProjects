using System.Linq;
using TryInterview.Models.DbModels;

namespace TryInterview.BlLayer
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICompanies
    {
        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <returns></returns>
        IQueryable<Company> GetCompanies();
    }
}
