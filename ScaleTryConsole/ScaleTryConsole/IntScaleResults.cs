using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScaleTryConsole
{
    /// <summary>
    /// IntScaleResults
    /// </summary>
    public class IntScaleResults
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IntScaleResults"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool success { get; set; } = false;
        /// <summary>
        /// Gets or sets the error text.
        /// </summary>
        /// <value>
        /// The error text.
        /// </value>
        [StringLength(100)]
        public string ErrorText { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        [Column(TypeName = "decimal(20,4)")]
        public decimal Weight { get; set; } = 0.0m;
        /// <summary>
        /// Gets or sets the uom.
        /// </summary>
        /// <value>
        /// The uom.
        /// </value>
        [StringLength(5)]
        public string UOM { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is scale stable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is scale stable; otherwise, <c>false</c>.
        /// </value>
        public bool isScaleStable { get; set; } = false;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is scale out of range.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is scale out of range; otherwise, <c>false</c>.
        /// </value>
        public bool isScaleOutOfRange { get; set; } = false;
    }
}
