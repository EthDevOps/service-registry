namespace QuokkaServiceRegistry.Models;

public class ServiceLifecycleInfo
{
    public int Id { get; set; }
    public List<ServiceLifecycleStage> Stages { get; set; } = new();
    public ServiceLifecycleStageType CurrentStage { get; set; }
    public string? MigrationPath { get; set; }
    public List<string> GeneralNotes { get; set; } = new();
}