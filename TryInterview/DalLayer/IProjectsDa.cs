using System.Linq;
using TryInterview.Models.DbModels;

namespace TryInterview.DalLayer
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProjectsDa
    {
        /// <summary>
        /// Gets the projects.
        /// </summary>
        /// <returns></returns>
        IQueryable<Project> GetProjects();

        /// <summary>
        /// Gets the projects billing data.
        /// </summary>
        /// <returns></returns>
        IQueryable<BillingData> BillingData();
    }
}
