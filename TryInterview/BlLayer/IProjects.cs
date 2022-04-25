using System.Linq;
using TryInterview.Models.DbModels;

namespace TryInterview.BlLayer
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProjects
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
        IQueryable<dynamic> GetProjectsBillingData();
    }
}
