using System;
using System.Collections.Generic;

namespace PackTesting.Model
{
    public class PackOrderDetails
    {
        /// <summary>
        /// Gets or sets the pack operation process stage identifier.
        /// </summary>
        /// <value>
        /// The pack operation process stage identifier.
        /// </value>
        public string packOperationProcessStageId { get; set; }
        /// <summary>
        /// Gets or sets the MFG order operation identifier.
        /// </summary>
        /// <value>
        /// The MFG order operation identifier.
        /// </value>
        public string mfgOrderOperationId { get; set; }
        /// <summary>
        /// Gets or sets the order packaging structure data.
        /// </summary>
        /// <value>
        /// The order packaging structure data.
        /// </value>
        public string orderPackagingStructureData { get; set; }
        /// <summary>
        /// Gets or sets the MFG process execution detail identifier.
        /// </summary>
        /// <value>
        /// The MFG process execution detail identifier.
        /// </value>
        public string MfgProcessExecutionDetailId { get; set; }
        /// <summary>
        /// Gets or sets the MFG packed lot identifier.
        /// </summary>
        /// <value>
        /// The MFG packed lot identifier.
        /// </value>
        public string mfgPackedLotId { get; set; }
        /// <summary>
        /// Gets or sets the MFG order identifier.
        /// </summary>
        /// <value>
        /// The MFG order identifier.
        /// </value>
        public string mfgOrderId { get; set; }
        /// <summary>
        /// Gets or sets the MFG production unit identifier.
        /// </summary>
        /// <value>
        /// The MFG production unit identifier.
        /// </value>
        public string MfgProductionUnitId { get; set; }
        /// <summary>
        /// Gets or sets the source batch identifier.
        /// </summary>
        /// <value>
        /// The source batch identifier.
        /// </value>
        public string sourceBatchId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [use scale].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use scale]; otherwise, <c>false</c>.
        /// </value>
        public bool useScale { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [pre pack].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [pre pack]; otherwise, <c>false</c>.
        /// </value>
        public bool prePack { get; set; }
        /// <summary>
        /// Gets or sets the MFG order packaging information data identifier.
        /// </summary>
        /// <value>
        /// The MFG order packaging information data identifier.
        /// </value>
        public string mfgOrderPackagingInfoDataId { get; set; }
        /// <summary>
        /// Gets or sets the instruction set.
        /// </summary>
        /// <value>
        /// The instruction set.
        /// </value>
        public List<Instruction> instructionSet { get; set; }
        /// <summary>
        /// Gets or sets the scanner list.
        /// </summary>
        /// <value>
        /// The scanner list.
        /// </value>
        public List<ScanDetails> ScannerList { get; set; }
        /// <summary>
        /// Gets or sets the net weight.
        /// </summary>
        /// <value>
        /// The net weight.
        /// </value>
        public double netWeight { get; set; }
        /// <summary>
        /// Gets or sets the weight uom.
        /// </summary>
        /// <value>
        /// The weight uom.
        /// </value>
        public string weightUOM { get; set; } = "G";
        /// <summary>
        /// Gets or sets a value indicating whether [use scanner].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use scanner]; otherwise, <c>false</c>.
        /// </value>
        public bool useScanner { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [use text display].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use text display]; otherwise, <c>false</c>.
        /// </value>
        public bool useTextDisplay { get; set; }
        /// <summary>
        /// Gets or sets the packed lot number.
        /// </summary>
        /// <value>
        /// The packed lot number.
        /// </value>
        public string packedLotNumber { get; set; }
        /// <summary>
        /// Gets or sets the material number.
        /// </summary>
        /// <value>
        /// The material number.
        /// </value>
        public string materialNumber { get; set; }
        /// <summary>
        /// Gets or sets the plant identifier.
        /// </summary>
        /// <value>
        /// The plant identifier.
        /// </value>
        public string plantID { get; set; }
        /// <summary>
        /// Gets or sets the gross.
        /// </summary>
        /// <value>
        /// The gross.
        /// </value>
        public double gross { get; set; }
        /// <summary>
        /// Gets or sets the net.
        /// </summary>
        /// <value>
        /// The net.
        /// </value>
        public double net { get; set; }
        /// <summary>
        /// Gets or sets the tare.
        /// </summary>
        /// <value>
        /// The tare.
        /// </value>
        public double tare { get; set; }
        /// <summary>
        /// Gets or sets the uo m value.
        /// </summary>
        /// <value>
        /// The uo m value.
        /// </value>
        public string uoMValue { get; set; }
        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public string Operator { get; set; }
        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public Guid? OperatorId { get; set; }
        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        public DateTime createdDateTime { get; set; }
        /// <summary>
        /// Gets or sets the MFG packed in lot bundle identifier.
        /// </summary>
        /// <value>
        /// The MFG packed in lot bundle identifier.
        /// </value>
        public object mfgPackedInLotBundleId { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; } = "";
        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public double Quantity { get; set; }
        /// <summary>
        /// Gets or sets the previous weighed value.
        /// </summary>
        /// <value>
        /// The previous weighed value.
        /// </value>
        public double PreviousWeighedValue { get; set; }
        /// <summary>
        /// Gets or sets the scanner list reqd.
        /// </summary>
        /// <value>
        /// The scanner list reqd.
        /// </value>
        public int ScannerListReqd { get; set; }
        /// <summary>
        /// Gets or sets the sample.
        /// </summary>
        /// <value>
        /// The sample.
        /// </value>
        public Guid? Sample { get; set; }
        /// <summary>
        /// Gets or sets the messasured quantity.
        /// </summary>
        /// <value>
        /// The messasured quantity.
        /// </value>
        public decimal MessasuredQuantity { get; set; }
        /// <summary>
        /// Gets or sets the messasured quantity uo m value.
        /// </summary>
        /// <value>
        /// The messasured quantity uo m value.
        /// </value>
        public string MessasuredQuantityUoMValue { get; set; }
        /// <summary>
        /// Gets or sets the MFG source storage unit detail identifier.
        /// </summary>
        /// <value>
        /// The MFG source storage unit detail identifier.
        /// </value>
        public string MfgSourceStorageUnitDetailId { get; set; } = null;
        /// <summary>
        /// Gets or sets the MFG target storage unit detail identifier.
        /// </summary>
        /// <value>
        /// The MFG target storage unit detail identifier.
        /// </value>
        public string MfgTargetStorageUnitDetailId { get; set; } = null;
        /// <summary>
        /// Gets or sets the MFG source ancillary storage unit identifier.
        /// </summary>
        /// <value>
        /// The MFG source ancillary storage unit identifier.
        /// </value>
        public string MfgSourceAncillaryStorageUnitId { get; set; } = null;
        /// <summary>
        /// Gets or sets the supervisor identifier.
        /// </summary>
        /// <value>
        /// The supervisor identifier.
        /// </value>
        public string SupervisorId { get; set; } = null;
        /// <summary>
        /// Gets or sets a value indicating whether [order quantity packed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [order quantity packed]; otherwise, <c>false</c>.
        /// </value>
        public bool OrderQuantityPacked { get; set; } = false;
        /// <summary>
        /// Gets or sets a value indicating whether [supervisor authentication order complete].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supervisor authentication order complete]; otherwise, <c>false</c>.
        /// </value>
        public bool SupervisorAuthOrderComplete { get; set; } = false;
        /// <summary>
        /// Gets or sets the supervisor authentication comments.
        /// </summary>
        /// <value>
        /// The supervisor authentication comments.
        /// </value>
        public string SupervisorAuthComments { get; set; } = null;
    }
}
