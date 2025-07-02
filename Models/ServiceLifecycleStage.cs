namespace QuokkaServiceRegistry.Models;

public class ServiceLifecycleStage
{
    public int Id { get; set; }
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