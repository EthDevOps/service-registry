namespace QuokkaServiceRegistry.Models;

public class GdprDataSubjectRights
{
    public int Id { get; set; }
    
    // Rights support capabilities
    public bool SupportsRightOfAccess { get; set; }
    public bool SupportsRightOfRectification { get; set; }
    public bool SupportsRightOfErasure { get; set; }
    public bool SupportsRightToPortability { get; set; }
    public bool SupportsRightToRestriction { get; set; }
    public bool SupportsRightToObject { get; set; }
    public bool SupportsAutomatedDecisionMakingRights { get; set; }
    
    // Response procedures
    public int ResponseTimeframeDays { get; set; } = 30;
    public string? ResponseProcedureDescription { get; set; }
    public string? ResponsibleTeam { get; set; }
    public string? EscalationProcess { get; set; }
    
    // Technical assistance
    public bool TechnicalAssistanceAvailable { get; set; }
    public string? TechnicalAssistanceDescription { get; set; }
    public List<string> AssistanceCapabilities { get; set; } = new();
    
    // Data export capabilities
    public bool DataExportCapable { get; set; }
    public List<DataExportFormat> SupportedExportFormats { get; set; } = new();
    public string? ExportProcessDescription { get; set; }
    
    // Request handling
    public RequestHandlingMethod RequestHandlingMethod { get; set; }
    public string? RequestContactEmail { get; set; }
    public string? RequestContactPhone { get; set; }
    public string? OnlinePortalUrl { get; set; }
    
    // Verification process
    public bool IdentityVerificationRequired { get; set; }
    public string? VerificationProcessDescription { get; set; }
    public List<VerificationMethod> AcceptedVerificationMethods { get; set; } = new();
    
    // Compliance tracking
    public DateTime? LastRightsAssessment { get; set; }
    public string? ComplianceNotes { get; set; }
    
    // Foreign key
    public int GdprDataRegisterId { get; set; }
    public GdprDataRegister? GdprDataRegister { get; set; }
}

public enum DataExportFormat
{
    Json,
    Xml,
    Csv,
    Pdf,
    StructuredText,
    DatabaseDump,
    Other
}

public enum RequestHandlingMethod
{
    Email,
    OnlinePortal,
    PhoneCall,
    WrittenRequest,
    ApiIntegration,
    Multiple
}

public enum VerificationMethod
{
    EmailVerification,
    DocumentUpload,
    PhoneVerification,
    TwoFactorAuth,
    DigitalSignature,
    InPersonVerification,
    Other
}