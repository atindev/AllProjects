namespace TryInterview.Models.DbModels
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="TryInterview.Models.DbModels.BaseDbModel" />
    public class Project : EntityBaseDbModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the billing mode.        
        /// </summary>
        /// <value>
        /// The billing mode.
        /// </value>
        /// 1 is for Debit, 2 is for Credit
        public int BillingMode { get; set; } = 1;

        /// <summary>
        /// Gets or sets the billed amout.
        /// </summary>
        /// <value>
        /// The billed amout.
        /// </value>
        public decimal BilledAmount { get; set; }

        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>
        /// The customer.
        /// </value>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        public int CustomerId { get; set; }

        #endregion Public Properties
    }
}