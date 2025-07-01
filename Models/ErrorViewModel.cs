namespace QuokkaServiceRegistry.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

public class CatalogService
{
    public string Name { get; set; }
    public CatalogVendor Vendor { get; set; }
    public bool IsPropriety { get; set; }
    public SoftwareLicense License { get; set; }
    public ServiceSubscription? Subscription { get; set; }
    public GdprDataRegister? GdprRegister { get; set; }
    public HostingInformation Hosting { get; set; }
    public ServiceLifecycleInfo Lifecycle { get; set; }
}

public class HostingInformation
{
    public CatalogHostingType HostingType { get; set; }
    public string HostingCountry { get; set; }
    public string? SaasRegionReference { get; set; }
    public List<OnpremiseHost>?  OnpremisHosts { get; set; }
}

public class OnpremiseHost
{
    public CatalogVendor CloudProvider { get; set; }
    public string Region { get; set; }
    public string NetboxReferenceUrl { get; set; }
}

public class ServiceSubscription
{
    public string Name { get; set; }
    public int SeatsUsed { get; set; }
    public int SeatsAvailable { get; set; }
    public double CostPerSeatUSD { get; set; }
    public TimeSpan TermDuration { get; set; }
}

public class SoftwareLicense
{
    public string Name { get; set; }
    public string Abbreviation { get; set; }
    public string LicenseUrl { get; set; }
    
}

public enum CatalogHostingType
{
    SaaS,
    SelfHosted,
}

public class CatalogVendor
{
    public string Name { get; set; }
    public string WebsiteUrl { get; set; }
    public Address Address { get; set; }
    public BillingInformation BillingInformation { get; set; }
    public bool GdprProcessingAgreementExists { get; set; }
    public string? ProcessingAgreementLink { get; set; }
}

public class BillingInformation
{
    public BillingType Type { get; set; }
    public CostCenter CostCenter { get; set; }
    
    // Credit Card Information
    public string? CardHolderName { get; set; }
    public string? CardNumber { get; set; }
    public string? ExpiryDate { get; set; }
    
    // Bank Transfer Information
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
    
    // SEPA Information
    public string? SepaMandateId { get; set; }
    public DateTime? SepaMandateDate { get; set; }
    public string? SepaCreditorId { get; set; }
    
    // General Payment Information
    public string? PaymentMethodId { get; set; }
    public bool IsActive { get; set; }
}

public class CostCenter
{
}

public enum BillingType
{
    Free,
    CreditCard,
    BankTransfer,
    PrePaid,
    SEPA
}

public class Address
{
    public string Country { get; set; }
    public string City { get; set; }
}

public enum DataCategory
{
    PersonalData,
    ConfidentialInformation,
    SensitiveData,
    PublicData
}

public enum ProcessingPurpose
{
    HumanResources,
    Payroll,
    Authentication,
    Communication,
    ProjectManagement,
    CustomerSupport,
    Marketing,
    Analytics,
    Compliance,
    BackupAndRecovery,
    SystemAdministration
}

public enum ProcessingFrequency
{
    Continuous,
    Daily,
    Weekly,
    Monthly,
    Quarterly,
    Annually,
    AdHoc,
    OnDemand
}

public enum SecurityMeasure
{
    Encryption,
    AccessControl,
    TwoFactorAuthentication,
    SingleSignOn,
    DataBackup,
    AuditLogging,
    NetworkSecurity,
    DataMinimization,
    Pseudonymization,
    Anonymization
}

public class GdprDataRegister
{
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

public enum ServiceLifecycleStageType
{
    ProofOfConcept,
    OrganizationWideTrial,
    Production,
    Deprecated,
    Decommissioned
}

public enum ServiceLifecycleStageStatus
{
    Planned,
    InProgress,
    Completed,
    Cancelled,
    OnHold
}

public class ServiceLifecycleStage
{
    public ServiceLifecycleStageType StageType { get; set; }
    public ServiceLifecycleStageStatus Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Sponsor { get; set; }
    public string? ApprovedBy { get; set; }
    public List<string> Participants { get; set; } = new();
    public List<string> Departments { get; set; } = new();
    public string? Outcome { get; set; }
    public string? Feedback { get; set; }
    public List<string> Criteria { get; set; } = new();
    public Dictionary<string, string> CustomProperties { get; set; } = new();
    public List<string> Notes { get; set; } = new();
}

public class ServiceLifecycleInfo
{
    public List<ServiceLifecycleStage> Stages { get; set; } = new();
    public ServiceLifecycleStageType CurrentStage { get; set; }
    public string? MigrationPath { get; set; }
    public List<string> GeneralNotes { get; set; } = new();
}


