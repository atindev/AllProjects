namespace ScaleTryConsole
{
    /// <summary>
    /// 
    /// </summary>
    public class ScaleObject
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the equipment.
        /// </summary>
        /// <value>
        /// The name of the equipment.
        /// </value>
        public string EquipmentName { get; set; }

        /// <summary>
        /// Gets or sets the MFG equipment identifier.
        /// </summary>
        /// <value>
        /// The MFG equipment identifier.
        /// </value>
        public string MfgEquipmentId { get; set; }

        /// <summary>
        /// Gets or sets the scale library identifier.
        /// </summary>
        /// <value>
        /// The scale library identifier.
        /// </value>
        public string ScaleLibraryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        public string ScalehostName { get; set; }

        /// <summary>
        /// Gets or sets the read port.
        /// </summary>
        /// <value>
        /// The read port.
        /// </value>
        public int ReadPort { get; set; }

        /// <summary>
        /// Gets or sets the write port.
        /// </summary>
        /// <value>
        /// The write port.
        /// </value>
        public int WritePort { get; set; }

        /// <summary>
        /// Gets or sets the decimal positions.
        /// </summary>
        /// <value>
        /// The decimal positions.
        /// </value>
        public int Precision { get; set; }

        /// <summary>
        /// Gets or sets the command zero.
        /// </summary>
        /// <value>
        /// The command zero.
        /// </value>
        public string cmdZero { get; set; } = "Z";

        /// <summary>
        /// Gets or sets the command reset clear.
        /// </summary>
        /// <value>
        /// The command reset clear.
        /// </value>
        public string cmdResetClear { get; set; } = "C";

        /// <summary>
        /// Gets or sets the command tare.
        /// </summary>
        /// <value>
        /// The command tare.
        /// </value>
        public string cmdTare { get; set; } = "T";

        /// <summary>
        /// Gets or sets the command read.
        /// </summary>
        /// <value>
        /// The command read.
        /// </value>
        public string cmdRead { get; set; } = "S";

        /// <summary>
        /// Gets or sets the scale uom.
        /// </summary>
        /// <value>
        /// The scale uom.
        /// </value>
        public string UOM { get; set; } = "KG";

        /// <summary>
        /// Gets or sets the scale library.
        /// </summary>
        /// <value>
        /// The scale library.
        /// </value>
        public string ScaleLibrary { get; set; } = "NodeSimulator";

        /// <summary>
        /// Gets or sets a value indicating whether [enable zero].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable zero]; otherwise, <c>false</c>.
        /// </value>
        public bool enableZero { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [enable tare].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable tare]; otherwise, <c>false</c>.
        /// </value>
        public bool enableTare { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether [enable reset clear].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable reset clear]; otherwise, <c>false</c>.
        /// </value>
        public bool enableResetClear { get; set; } = true;
        /// <summary>
        /// Gets or sets the scan frequency.
        /// </summary>
        /// <value>
        /// The scan frequency.
        /// </value>
        public decimal ScanFrequency { get; set; } = 1.0m;
        /// <summary>
        /// Gets or sets the tare frequency.
        /// </summary>
        /// <value>
        /// The tare frequency.
        /// </value>
        public decimal TareFrequency { get; set; } = 1.0m;
        /// <summary>
        /// Gets the scan timer.
        /// </summary>
        /// <value>
        /// The scan timer.
        /// </value>
        public int ScanTimer => (int)(ScanFrequency * 1000);
        /// <summary>
        /// Gets the tare timer.
        /// </summary>
        /// <value>
        /// The tare timer.
        /// </value>
        public int TareTimer => (int)(TareFrequency * 1000);
    }
}
