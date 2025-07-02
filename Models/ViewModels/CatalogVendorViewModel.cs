using System.ComponentModel.DataAnnotations;

namespace QuokkaServiceRegistry.Models.ViewModels;

public class CatalogVendorViewModel
{
    public int Id { get; set; }
    
    [Required]
    [Display(Name = "Vendor Name")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "Website URL")]
    [Url(ErrorMessage = "Please enter a valid URL")]
    [StringLength(500)]
    public string WebsiteUrl { get; set; } = string.Empty;
    
    [Display(Name = "GDPR Processing Agreement Exists")]
    public bool GdprProcessingAgreementExists { get; set; }
    
    [Display(Name = "Processing Agreement Link")]
    [Url(ErrorMessage = "Please enter a valid URL")]
    [StringLength(500)]
    public string? ProcessingAgreementLink { get; set; }
    
    // Address Information
    [Display(Name = "Address")]
    public int? AddressId { get; set; }
    public Address? Address { get; set; }
    
    // Billing Information
    [Display(Name = "Billing Information")]
    public int? BillingInformationId { get; set; }
    public BillingInformation? BillingInformation { get; set; }
}

public class CatalogVendorCreateViewModel
{
    [Required]
    [Display(Name = "Vendor Name")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "Website URL")]
    [Url(ErrorMessage = "Please enter a valid URL")]
    [StringLength(500)]
    public string WebsiteUrl { get; set; } = string.Empty;
    
    [Display(Name = "GDPR Processing Agreement Exists")]
    public bool GdprProcessingAgreementExists { get; set; }
    
    [Display(Name = "Processing Agreement Link")]
    [Url(ErrorMessage = "Please enter a valid URL")]
    [StringLength(500)]
    public string? ProcessingAgreementLink { get; set; }
    
    // Address Information
    [Display(Name = "Create New Address")]
    public bool CreateNewAddress { get; set; }
    
    [Display(Name = "Existing Address")]
    public int? ExistingAddressId { get; set; }
    
    [Display(Name = "Country")]
    [StringLength(100)]
    public string? Country { get; set; }
    
    [Display(Name = "City")]
    [StringLength(100)]
    public string? City { get; set; }
    
    // Billing Information
    [Display(Name = "Create New Billing Information")]
    public bool CreateNewBillingInfo { get; set; }
    
    [Display(Name = "Existing Billing Information")]
    public int? ExistingBillingInfoId { get; set; }
    
    [Display(Name = "Billing Type")]
    public BillingType? BillingType { get; set; }
    
    [Display(Name = "Cost Center")]
    public int? CostCenterId { get; set; }
    
    [Display(Name = "New Cost Center Name")]
    [StringLength(200)]
    public string? NewCostCenterName { get; set; }
    
    [Display(Name = "Cost Center Code")]
    [StringLength(50)]
    public string? CostCenterCode { get; set; }
    
    [Display(Name = "Cost Center Description")]
    [StringLength(500)]
    public string? CostCenterDescription { get; set; }
    
    // Credit Card Information (if applicable)
    [Display(Name = "Card Holder Name")]
    [StringLength(200)]
    public string? CardHolderName { get; set; }
    
    [Display(Name = "Card Number")]
    [StringLength(20)]
    public string? CardNumber { get; set; }
    
    [Display(Name = "Expiry Date")]
    [StringLength(7)]
    public string? ExpiryDate { get; set; }
    
    [Display(Name = "CVV")]
    [StringLength(4)]
    public string? Cvv { get; set; }
}

public class CatalogVendorEditViewModel : CatalogVendorCreateViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Current Address")]
    public string? CurrentAddressDisplay { get; set; }
    
    [Display(Name = "Current Billing Type")]
    public string? CurrentBillingTypeDisplay { get; set; }
    
    [Display(Name = "Services Using This Vendor")]
    public int ServicesCount { get; set; }
}

public class CatalogVendorDetailsViewModel
{
    public CatalogVendor Vendor { get; set; } = null!;
    public List<CatalogService> Services { get; set; } = new();
    public int TotalServices { get; set; }
    public int ActiveServices { get; set; }
    public int SaasServices { get; set; }
    public int SelfHostedServices { get; set; }
}

public class VendorQuickStatsViewModel
{
    public int TotalVendors { get; set; }
    public int VendorsWithGdprAgreement { get; set; }
    public int VendorsWithActiveServices { get; set; }
    public int VendorsWithSubscriptions { get; set; }
}