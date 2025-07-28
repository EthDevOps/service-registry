namespace QuokkaServiceRegistry.Models;

public class GdprConfidentialityRecord
{
    public int Id { get; set; }
    
    // Staff confidentiality commitments
    public bool StaffConfidentialityCommitmentsInPlace { get; set; }
    public DateTime? LastConfidentialityTrainingDate { get; set; }
    public string? ConfidentialityTrainingProvider { get; set; }
    public List<string> TrainedPersonnel { get; set; } = new();
    
    // Access authorization
    public bool AccessAuthorizationProcessInPlace { get; set; }
    public string? AccessControlDescription { get; set; }
    public List<string> AuthorizedPersonnel { get; set; } = new();
    public DateTime? LastAccessReviewDate { get; set; }
    
    // Confidentiality documentation
    public string? ConfidentialityPolicyReference { get; set; }
    public DateTime? PolicyLastUpdated { get; set; }
    public string? ConfidentialityAgreementTemplate { get; set; }
    
    // Monitoring and compliance
    public bool ConfidentialityMonitoringInPlace { get; set; }
    public string? MonitoringDescription { get; set; }
    public DateTime? LastConfidentialityAudit { get; set; }
    public string? AuditFindings { get; set; }
    
    public string? Notes { get; set; }
    
    // Foreign key
    public int GdprDataRegisterId { get; set; }
    public GdprDataRegister? GdprDataRegister { get; set; }
}