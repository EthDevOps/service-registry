using System.ComponentModel.DataAnnotations;

namespace QuokkaServiceRegistry.Models.ViewModels;

public class CatalogServiceViewModel
{
    public int Id { get; set; }
    
    [Required]
    [Display(Name = "Service Name")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Is Proprietary")]
    public bool IsPropriety { get; set; }
    
    // Vendor Information
    [Required]
    [Display(Name = "Vendor")]
    public int VendorId { get; set; }
    public CatalogVendor? Vendor { get; set; }
    
    // License Information
    [Required]
    [Display(Name = "License")]
    public int LicenseId { get; set; }
    public SoftwareLicense? License { get; set; }
    
    // Hosting Information
    [Required]
    [Display(Name = "Hosting Type")]
    public CatalogHostingType HostingType { get; set; }
    
    [Required]
    [Display(Name = "Hosting Country")]
    [StringLength(100)]
    public string HostingCountry { get; set; } = string.Empty;
    
    [Display(Name = "SaaS Region Reference")]
    [StringLength(200)]
    public string? SaasRegionReference { get; set; }
    
    // Lifecycle Information
    [Required]
    [Display(Name = "Lifecycle")]
    public int LifecycleId { get; set; }
    public ServiceLifecycleInfo? Lifecycle { get; set; }
    
    // Optional Information
    [Display(Name = "Subscription")]
    public int? SubscriptionId { get; set; }
    public ServiceSubscription? Subscription { get; set; }
    
    [Display(Name = "GDPR Register")]
    public int? GdprRegisterId { get; set; }
    public GdprDataRegister? GdprRegister { get; set; }
}

public class CatalogServiceCreateViewModel
{
    [Required]
    [Display(Name = "Service Name")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Is Proprietary")]
    public bool IsPropriety { get; set; }
    
    // Vendor Information
    [Required]
    [Display(Name = "Vendor")]
    public int? VendorId { get; set; }
    
    [Display(Name = "Create New Vendor")]
    public bool CreateNewVendor { get; set; }
    
    [Display(Name = "Vendor Name")]
    [StringLength(200)]
    public string? NewVendorName { get; set; }
    
    [Display(Name = "Vendor Website")]
    [Url]
    [StringLength(500)]
    public string? NewVendorWebsite { get; set; }
    
    // License Information
    [Required]
    [Display(Name = "License")]
    public int? LicenseId { get; set; }
    
    [Display(Name = "Create New License")]
    public bool CreateNewLicense { get; set; }
    
    [Display(Name = "License Name")]
    [StringLength(200)]
    public string? NewLicenseName { get; set; }
    
    [Display(Name = "License Abbreviation")]
    [StringLength(50)]
    public string? NewLicenseAbbreviation { get; set; }
    
    [Display(Name = "License URL")]
    [Url]
    [StringLength(500)]
    public string? NewLicenseUrl { get; set; }
    
    // Hosting Information
    [Required]
    [Display(Name = "Hosting Type")]
    public CatalogHostingType HostingType { get; set; }
    
    [Required]
    [Display(Name = "Hosting Country")]
    [StringLength(100)]
    public string HostingCountry { get; set; } = string.Empty;
    
    [Display(Name = "SaaS Region Reference")]
    [StringLength(200)]
    public string? SaasRegionReference { get; set; }
    
    // Lifecycle Information
    [Required]
    [Display(Name = "Current Stage")]
    public ServiceLifecycleStageType CurrentStage { get; set; }
    
    [Display(Name = "Migration Path")]
    [StringLength(1000)]
    public string? MigrationPath { get; set; }
    
    // Subscription Information
    [Display(Name = "Has Subscription")]
    public bool HasSubscription { get; set; }
    
    [Display(Name = "Subscription Name")]
    [StringLength(200)]
    public string? SubscriptionName { get; set; }
    
    [Display(Name = "Seats Used")]
    public int? SeatsUsed { get; set; }
    
    [Display(Name = "Seats Available")]
    public int? SeatsAvailable { get; set; }
    
    [Display(Name = "Cost Per Seat (USD)")]
    [Range(0, double.MaxValue, ErrorMessage = "Cost must be positive")]
    public double? CostPerSeatUsd { get; set; }
    
    [Display(Name = "Term Duration (Days)")]
    public int? TermDurationDays { get; set; }
    
    // GDPR Information
    [Display(Name = "Requires GDPR Register")]
    public bool RequiresGdprRegister { get; set; }
    
    [Display(Name = "Data Categories")]
    public List<DataCategory> DataCategories { get; set; } = new();
    
    [Display(Name = "Processing Purposes")]
    public List<ProcessingPurpose> ProcessingPurposes { get; set; } = new();
    
    [Display(Name = "Processing Frequency")]
    public ProcessingFrequency? ProcessingFrequency { get; set; }
    
    [Display(Name = "Processes Employee PII")]
    public bool ProcessesEmployeePii { get; set; }
    
    [Display(Name = "Processes External User PII")]
    public bool ProcessesExternalUserPii { get; set; }
    
    [Display(Name = "Processes Sensitive Data")]
    public bool ProcessesSensitiveData { get; set; }
}

public class CatalogServiceEditViewModel : CatalogServiceCreateViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Current Vendor")]
    public string? CurrentVendorName { get; set; }
    
    [Display(Name = "Current License")]
    public string? CurrentLicenseName { get; set; }
}