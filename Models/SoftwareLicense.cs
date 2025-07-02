namespace QuokkaServiceRegistry.Models;

public class SoftwareLicense
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public string? LicenseUrl { get; set; }
    public LicenseType Type { get; set; }
    public bool IsCopyleft { get; set; }
    public bool IsOsiApproved { get; set; }
    public string? Description { get; set; }
    public bool RequiresAttribution { get; set; }
    public bool RequiresSourceDisclosure { get; set; }
    public CompatibilityLevel CompatibilityLevel { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}