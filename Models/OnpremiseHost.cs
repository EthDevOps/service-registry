namespace QuokkaServiceRegistry.Models;

public class OnpremiseHost
{
    public int Id { get; set; }
    public string CloudProvider { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string NetboxReferenceUrl { get; set; } = string.Empty;
    
    // Foreign key to CatalogService
    public int CatalogServiceId { get; set; }
    public CatalogService CatalogService { get; set; } = null!;
}