using System;

namespace TryInterview.Models.DbModels
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="TryInterview.Models.DbModels.BaseDbModel" />
    public class BillingData : BaseDbModel
    {
        /// <summary>
        /// Gets or sets the type of the transaction.
        /// </summary>
        /// <value>
        /// The type of the transaction.
        /// </value>
        public int TransactionType { get; set; } = 0;
        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        /// <value>
        /// The transaction date.
        /// </value>
        public DateTime TransactionDate { get; set; }
        /// <summary>
        /// Gets or sets the transaction amount.
        /// </summary>
        /// <value>
        /// The transaction amount.
        /// </value>
        public decimal TransactionAmount { get; set; } = 0m;
    }
}
