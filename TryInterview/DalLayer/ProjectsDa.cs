using System.Linq;
using TryInterview.Models.DbModels;

namespace TryInterview.DalLayer
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TryInterview.DalLayer.IProjectsDa" />
    public class ProjectsDa : IProjectsDa
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly CompanyDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectsDa"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ProjectsDa(CompanyDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the projects.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Project> GetProjects()
        {
            return this.context.Project.AsQueryable();
        }

        /// <summary>
        /// Gets the projects billing data.
        /// </summary>
        /// <returns></returns>
        public IQueryable<BillingData> BillingData()
        {
            return this.context.BillingData.AsQueryable();
        }
    }
}
