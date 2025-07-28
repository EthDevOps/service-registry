namespace QuokkaServiceRegistry.Models;

public class GdprProcessingInstruction
{
    public int Id { get; set; }
    public string InstructionTitle { get; set; } = string.Empty;
    public string InstructionDetails { get; set; } = string.Empty;
    public DateTime DateIssued { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string IssuedBy { get; set; } = string.Empty;
    public bool RequiresControllerApproval { get; set; }
    public bool ControllerApprovalReceived { get; set; }
    public DateTime? ControllerApprovalDate { get; set; }
    public string? ApprovalReference { get; set; }
    public ProcessingInstructionStatus Status { get; set; }
    public string? Notes { get; set; }
    
    // Foreign key
    public int GdprDataRegisterId { get; set; }
    public GdprDataRegister? GdprDataRegister { get; set; }
}

public enum ProcessingInstructionStatus
{
    Draft,
    PendingApproval,
    Approved,
    Active,
    Superseded,
    Revoked
}