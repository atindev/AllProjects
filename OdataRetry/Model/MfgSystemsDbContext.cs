using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using West.Manufacturing.Model;

namespace OdataRetry.Model
{
    public class MfgSystemsDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MfgSystemsDbContext(IHttpContextAccessor httpContextAccessor, DbContextOptions options)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            Database.EnsureCreated();
        }

        /* Order Related */
        public DbSet<MfgOrder> MfgOrder { get; set; }
        public DbSet<MfgOrderComponent> MfgOrderComponent { get; set; }
        public DbSet<MfgOrderOperation> MfgOrderOperation { get; set; }
        public DbSet<MfgMixBatchTemplate> MfgMixBatchTemplate { get; set; }
        public DbSet<MfgMixBatchBOMTemplate> MfgMixBatchBOMTemplate { get; set; }

        public DbSet<MfgOrderPackagingStructure> MfgOrderPackagingStructure { get; set; }
        public DbSet<MfgOrderComponentGHS> MfgOrderComponentsGHS { get; set; }
        public DbSet<MfgGHSDetail> MfgGHSDetail { get; set; }
        public DbSet<MfgWorkCenter> MfgWorkCenter { get; set; }
        public DbSet<MfgInboundInterface> MfgInboundInterface { get; set; }
        public DbSet<MfgOutboundInterface> MfgOutboundInterface { get; set; }
        public DbSet<MfgInterfaceConfiguration> MfgInterfaceConfiguration { get; set; }
        public DbSet<MfgExecution> MfgExecution { get; set; }
        public DbSet<MfgExecutionEventInterface> MfgExecutionEventInterface { get; set; }
        public DbSet<MfgExceptionLog> MfgExceptionLog { get; set; }
        public DbSet<MfgSiteConfiguration> MfgSiteConfiguration { get; set; }
        public DbSet<WDTransactionLog> WDTransactionLog { get; set; }
        public DbSet<MfgUniqueIdGenerator> MfgUniqueIdGenerator { get; set; }
        public DbSet<MfgLabelData> MfgLabelData { get; set; }

        public DbSet<MfgEquipmentStatusDefinition> MfgEquipmentStatusDefinition { get; set; }

        public DbSet<MfgEquipmentStatusHistory> MfgEquipmentStatusHistory { get; set; }

        public DbSet<MfgEquipmentStatus> MfgEquipmentStatus { get; set; }

        public DbSet<MfgSite> MfgSite { get; set; }
        public DbSet<MfgLocation> MfgLocation { get; set; }
        public DbSet<MfgCell> MfgCell { get; set; }

        public DbSet<WDOrderMixBatch> WDOrderMixBatch { get; set; }
        public DbSet<WDOrderMixBatchBOM> WDOrderMixBatchBOM { get; set; }

        public DbSet<WDOrderMixBatchOperation> WDOrderMixBatchOperation { get; set; }
        public DbSet<MfgShiftSchedule> MfgShiftSchedules { get; set; }


        public DbSet<MfgLanguage> MfgLanguage { get; set; }
        public DbSet<MfgMessageDetail> MfgMessageDetail { get; set; }
        public DbSet<MfgMessage> MfgMessage { get; set; }
        public DbSet<MfgApplicationEvent> MfgApplicationEvent { get; set; }
        public DbSet<MfgApplicationLog> MfgApplicationLog { get; set; }
        public DbSet<MfgCodeDetail> MfgCodeDetail { get; set; }

        public DbSet<MfgProcessArea> MfgProcessArea { get; set; }

        public DbSet<MfgShiftGroup> MfgShiftGroup { get; set; }

        public DbSet<MfgShiftCalendar> MfgShiftCalendar { get; set; }

        // Product Master / SAP Plant Storagae Location
        public DbSet<MfgProduct> MfgProduct { get; set; }
        public DbSet<MfgInventory> MfgInventory { get; set; }
        public DbSet<MfgBatch> MfgBatch { get; set; }
        public DbSet<MfgProductAlternateUOM> MfgProductAlternateUOM { get; set; }
        public DbSet<MfgPlantStorageLocationMaster> MfgPlantStorageLocationMaster { get; set; }
        public DbSet<MfgBatchStatus> MfgBatchStatus { get; set; }

        // Added object tables
        public DbSet<MfgObjectClassPropertyValue> MfgObjectClassPropertyValue { get; set; }
        public DbSet<MfgObjectClassProperty> MfgObjectClassProperty { get; set; }
        public DbSet<MfgObjectStateProperty> MfgObjectStateProperty { get; set; }
        public DbSet<MfgObjectClass> MfgObjectClass { get; set; }
        public DbSet<MfgObject> MfgObject { get; set; }
        public DbSet<MfgObjectGroup> MfgObjectGroup { get; set; }
        public DbSet<MfgGroup> MfgGroup { get; set; }

        // Test Objects to Be Removed after POC:
        public DbSet<POCTestObjectTable1> POCTestObjectTable1 { get; set; }
        public DbSet<POCTestObjectTable2> POCTestObjectTable2 { get; set; }


        // Orchestration Generic Execution Structures
        public DbSet<MfgExecutionProcessing> MfgExecutionProcessing { get; set; }
        public DbSet<MfgExecutionConfiguration> MfgExecutionConfiguration { get; set; }
        public DbSet<MfgProcess> MfgProcess { get; set; }
        public DbSet<MfgOperation> MfgOperation { get; set; }
        public DbSet<MfgOperationProcess> MfgOperationProcess { get; set; }
        public DbSet<MfgStage> MfgStage { get; set; }
        public DbSet<MfgProcessStage> MfgProcessStage { get; set; }
        public DbSet<MfgOperationProcessStage> MfgOperationProcessStage { get; set; }
        public DbSet<MfgProcessExecution> MfgProcessExecution { get; set; }
        public DbSet<MfgProcessExecutionDetail> MfgProcessExecutionDetail { get; set; }
        public DbSet<MfgProcessInstruction> MfgProcessInstruction { get; set; }
        public DbSet<MfgProcessInstructionDetail> MfgProcessInstructionDetail { get; set; }
        public DbSet<MfgProcessInstructionCode> MfgProcessInstructionCode { get; set; }

        // Label Set Structures
        public DbSet<MfgLabelSet> MfgLabelSet { get; set; }
        public DbSet<MfgLabelDefinition> MfgLabelDefinition { get; set; }
        public DbSet<MfgLabelPrinter> MfgLabelPrinter { get; set; }
        public DbSet<MfgLabelFile> MfgLabelFile { get; set; }
        public DbSet<MfgLabelPrinterType> MfgLabelPrinterType { get; set; }
        public DbSet<MfgLabelSetLabel> MfgLabelSetLabel { get; set; }
        public DbSet<MfgLabelType> MfgLabelType { get; set; }

        // Security Structures
        public DbSet<MfgUser> MfgUser { get; set; }
        public DbSet<MfgRole> MfgRole { get; set; }
        public DbSet<MfgUserRole> MfgUserRole { get; set; }
        public DbSet<MfgAuthorization> MfgAuthorization { get; set; }
        public DbSet<MfgRoleAuthorization> MfgRoleAuthorization { get; set; }

        // Master Data Structures
        public DbSet<MfgEquipment> MfgEquipment { get; set; }
        public DbSet<MfgScale> MfgScale { get; set; }
        public DbSet<MfgState> MfgState { get; set; }
        public DbSet<MfgStateDiagram> MfgStateDiagram { get; set; }
        public DbSet<MfgStateOption> MfgStateOption { get; set; }
        public DbSet<MfgProductionUnit> MfgProductionUnit { get; set; }
        public DbSet<MfgStorageUnitType> MfgStorageUnitType { get; set; }
        public DbSet<MfgProductionUnitOperation> MfgProductionUnitOperation { get; set; }
        public DbSet<MfgStateChangeHistory> MfgStateChangeHistory { get; set; }

        public DbSet<MfgStorageUnit> MfgStorageUnit { get; set; }
        public DbSet<MfgProductionUnitStorageUnit> MfgProductionUnitStorageUnit { get; set; }
        public DbSet<MfgProductionUnitEquipment> MfgProductionUnitEquipment { get; set; }
        public DbSet<MfgStorageUnitOperation> MfgStorageUnitOperation { get; set; }
    }
}
