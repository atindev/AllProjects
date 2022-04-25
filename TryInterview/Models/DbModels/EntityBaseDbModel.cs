namespace TryInterview.Models.DbModels
{
    public abstract class EntityBaseDbModel : BaseDbModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        #endregion Public Properties
    }
}