using System.ComponentModel.DataAnnotations;

namespace QuokkaServiceRegistry.Models;

public enum PaymentMethodType
{
    CreditCard,
    BankTransfer,
    Prepaid,
    SEPA
}

public class PaymentMethod
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public PaymentMethodType Type { get; set; }
    
    [Required]
    public int CostCenterId { get; set; }
    public CostCenter CostCenter { get; set; } = null!;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Credit Card specific fields
    [MaxLength(20)]
    public string? CardNumber { get; set; }
    
    [MaxLength(200)]
    public string? CardHolderName { get; set; }
    
    [MaxLength(7)]
    public string? ExpiryDate { get; set; }
    
    // Bank Transfer specific fields
    [MaxLength(200)]
    public string? BankName { get; set; }
    
    [MaxLength(50)]
    public string? AccountNumber { get; set; }
    
    [MaxLength(20)]
    public string? RoutingNumber { get; set; }
    
    [MaxLength(34)]
    public string? Iban { get; set; }
    
    [MaxLength(11)]
    public string? Swift { get; set; }
    
    // SEPA specific fields
    [MaxLength(35)]
    public string? SepaMandateId { get; set; }
    
    [MaxLength(35)]
    public string? SepaCreditorId { get; set; }
    
    public DateTime? SepaMandateDate { get; set; }
    
    // Prepaid specific fields
    [MaxLength(100)]
    public string? PrepaidVoucherCode { get; set; }
    
    public decimal? PrepaidBalance { get; set; }
    
    public DateTime? PrepaidExpiryDate { get; set; }
    
    // Payment gateway fields
    [MaxLength(100)]
    public string? PaymentMethodId { get; set; }
    
    // Outstanding balance tracking
    public decimal? OutstandingBalance { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? NextBillingDate { get; set; }
    
    // Navigation properties
    public List<CatalogService> Services { get; set; } = new();
}