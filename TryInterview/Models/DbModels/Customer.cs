using System.Collections.Generic;

namespace TryInterview.Models.DbModels
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="TryInterview.Models.DbModels.BaseDbModel" />
    public class Customer : EntityBaseDbModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the comapany.
        /// </summary>
        /// <value>
        /// The comapany.
        /// </value>
        public virtual Company Comapany { get; set; }

        /// <summary>
        /// Gets or sets the comapany identifier.
        /// </summary>
        /// <value>
        /// The comapany identifier.
        /// </value>
        public int ComapanyId { get; set; }

        /// <summary>
        /// Gets or sets the projects.
        /// </summary>
        /// <value>
        /// The projects.
        /// </value>
        public ICollection<Project> Projects { get; set; }

        #endregion Public Properties
    }
}