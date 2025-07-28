namespace QuokkaServiceRegistry.Models;

public class GdprContractTermination
{
    public int Id { get; set; }
    
    // Data handling options
    public ContractTerminationOption TerminationOption { get; set; }
    public string? TerminationInstructions { get; set; }
    
    // Data return process
    public bool DataReturnRequired { get; set; }
    public DataReturnFormat? ReturnFormat { get; set; }
    public string? ReturnDeliveryMethod { get; set; }
    public int? ReturnTimeframeDays { get; set; }
    
    // Data deletion process
    public bool DataDeletionRequired { get; set; }
    public DeletionMethod? DeletionMethod { get; set; }
    public int? DeletionTimeframeDays { get; set; }
    public bool DeletionCertificationRequired { get; set; }
    
    // Backup and archival data
    public bool BackupDataExists { get; set; }
    public string? BackupDataLocation { get; set; }
    public int? BackupRetentionDays { get; set; }
    public bool BackupDeletionIncluded { get; set; }
    
    // Process documentation
    public string? ProcessDescription { get; set; }
    public bool ProcessDocumented { get; set; }
    public string? ResponsibleParty { get; set; }
    
    // Compliance verification
    public bool RequiresControllerConfirmation { get; set; }
    public bool ProcessAuditable { get; set; }
    public string? ComplianceNotes { get; set; }
    
    // Foreign key
    public int GdprDataRegisterId { get; set; }
    public GdprDataRegister? GdprDataRegister { get; set; }
}

public enum ContractTerminationOption
{
    ReturnData,
    DeleteData,
    ReturnAndDelete,
    ControllerChoice
}

public enum DataReturnFormat
{
    OriginalFormat,
    StructuredData,
    DatabaseExport,
    JsonFormat,
    XmlFormat,
    CsvFormat,
    Other
}

public enum DeletionMethod
{
    SecureDeletion,
    Cryptographic,
    PhysicalDestruction,
    Overwriting,
    Degaussing,
    Other
}