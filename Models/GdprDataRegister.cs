namespace QuokkaServiceRegistry.Models;

public class GdprDataRegister
{
    public int Id { get; set; }
    public List<DataCategory> DataCategories { get; set; } = new();
    public List<ProcessingPurpose> ProcessingPurposes { get; set; } = new();
    public List<string> DataTransfers { get; set; } = new();
    public List<SecurityMeasure> SecurityMeasures { get; set; } = new();
    public ProcessingFrequency ProcessingFrequency { get; set; }
    public List<string> PurposesOfUse { get; set; } = new();
    
    // PII and sensitive data tracking
    public bool ProcessesEmployeePii { get; set; }
    public bool ProcessesExternalUserPii { get; set; }
    public bool ProcessesSensitiveData { get; set; }
    public List<string> SensitiveDataTypes { get; set; } = new();
    
    // Art. 28(3) GDPR processing agreements
    public bool HasProcessingAgreement { get; set; }
    public DateTime? ProcessingAgreementDate { get; set; }
    public string? ProcessingAgreementReference { get; set; }
    
    // Data retention and deletion
    public TimeSpan? DataRetentionPeriod { get; set; }
    public string? DataDeletionProcess { get; set; }
    
    // Data subject rights
    public bool SupportsDataPortability { get; set; }
    public bool SupportsDataDeletion { get; set; }
    public bool SupportsDataCorrection { get; set; }
    
    // Additional compliance information
    public string? DataProtectionOfficerContact { get; set; }
    public DateTime? LastGdprAssessment { get; set; }
    public string? ComplianceNotes { get; set; }
}