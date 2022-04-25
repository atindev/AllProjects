using System;
using System.ComponentModel.DataAnnotations;

namespace TryInterview.Models.DbModels
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseDbModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        public DateTime CreatedDateTime { get; set; }
    }
}
