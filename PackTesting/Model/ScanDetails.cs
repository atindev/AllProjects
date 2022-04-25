using System.Drawing;

namespace PackTesting.Model
{
    public class ScanDetails
    {
        /// <summary>
        /// Gets or sets the bag bundle identifier.
        /// </summary>
        /// <value>
        /// The bag bundle identifier.
        /// </value>
        public string BagBundleId { get; set; } = null;

        /// <summary>
        /// Gets or sets the bag text.
        /// </summary>
        /// <value>
        /// The bag text.
        /// </value>
        public string BagText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is scanned.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is scanned; otherwise, <c>false</c>.
        /// </value>
        public bool isScanned { get; set; } = false;

        /// <summary>
        /// Gets or sets the weight gross.
        /// </summary>
        /// <value>
        /// The weight gross.
        /// </value>
        public decimal WeightGross { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets the weight net.
        /// </summary>
        /// <value>
        /// The weight net.
        /// </value>
        public decimal? WeightNet { get; set; }

        /// <summary>
        /// Gets or sets the weight tare.
        /// </summary>
        /// <value>
        /// The weight tare.
        /// </value>
        public decimal? WeightTare { get; set; }

        /// <summary>
        /// Gets or sets the uom.
        /// </summary>
        /// <value>
        /// The uom.
        /// </value>
        public string UOM { get; set; }


        /// <summary>
        /// Gets or sets the color of the bg.
        /// </summary>
        /// <value>
        /// The color of the bg.
        /// </value>
        private Color bgColor;

        /// <summary>
        /// Gets or sets the color of the bg.
        /// </summary>
        /// <value>
        /// The color of the bg.
        /// </value>
        public Color BgColor
        {
            get
            {
                return bgColor;
            }
            set
            {
                bgColor = value;
                ////OnPropertyChanged(nameof(BgColor));
            }
        }
    }
}