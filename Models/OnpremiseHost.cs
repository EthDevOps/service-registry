namespace QuokkaServiceRegistry.Models;

public class OnpremiseHost
{
    public int Id { get; set; }
    public CatalogVendor CloudProvider { get; set; } = null!;
    public string Region { get; set; } = string.Empty;
    public string NetboxReferenceUrl { get; set; } = string.Empty;
    
    // Foreign key to CatalogService
    public int CatalogServiceId { get; set; }
    public CatalogService CatalogService { get; set; } = null!;
}