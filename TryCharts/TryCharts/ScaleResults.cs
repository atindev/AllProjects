namespace ScaleTryConsole
{
    /// <summary>
    /// Scale Results
    /// </summary>
    public class ScaleResults
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ScaleResults"/> is success.
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
        public string ErrorText { get; set; }
        /// <summary>
        /// Gets or sets the scale gross value.
        /// </summary>
        /// <value>
        /// The scale gross value.
        /// </value>        
        public decimal ScaleGrossValue { get; set; } = 0.0m;
        /// <summary>
        /// Gets or sets the scale net value.
        /// </summary>
        /// <value>
        /// The scale net value.
        /// </value>
        public decimal ScaleNetValue { get; set; } = 0.0m;
        /// <summary>
        /// Gets or sets the scale tare value.
        /// </summary>
        /// <value>
        /// The scale tare value.
        /// </value>
        public decimal ScaleTareValue { get; set; } = 0.0m;
        /// <summary>
        /// Gets or sets the scale uom.
        /// </summary>
        /// <value>
        /// The scale uom.
        /// </value>
        public string ScaleUOM { get; set; }
        /// <summary>
        /// Gets or sets the scale gross text.
        /// </summary>
        /// <value>
        /// The scale gross text.
        /// </value>
        public string ScaleGrossText { get; set; }
        /// <summary>
        /// Gets or sets the scale net text.
        /// </summary>
        /// <value>
        /// The scale net text.
        /// </value>
        public string ScaleNetText { get; set; }
        /// <summary>
        /// Gets or sets the scale tare text.
        /// </summary>
        /// <value>
        /// The scale tare text.
        /// </value>
        public string ScaleTareText { get; set; }
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
