namespace QuokkaServiceRegistry.Models;

public class GdprSubProcessor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string? ContactPhone { get; set; }
    
    public SubProcessorType ProcessorType { get; set; }
    public string ProcessingDescription { get; set; } = string.Empty;
    public List<string> DataCategoriesProcessed { get; set; } = new();
    
    // Controller consent
    public bool ControllerConsentRequired { get; set; }
    public bool ControllerConsentReceived { get; set; }
    public DateTime? ConsentDate { get; set; }
    public string? ConsentReference { get; set; }
    
    // Agreement details
    public bool HasProcessingAgreement { get; set; }
    public DateTime? AgreementDate { get; set; }
    public string? AgreementReference { get; set; }
    public DateTime? AgreementExpiryDate { get; set; }
    
    // Status
    public SubProcessorStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? Notes { get; set; }
    
    // Foreign key
    public int GdprDataRegisterId { get; set; }
    public GdprDataRegister? GdprDataRegister { get; set; }
}

public enum SubProcessorType
{
    CloudProvider,
    DataAnalytics,
    PaymentProcessor,
    EmailService,
    CustomerSupport,
    BackupService,
    SecurityService,
    Other
}

public enum SubProcessorStatus
{
    PendingApproval,
    Active,
    Suspended,
    Terminated,
    UnderReview
}