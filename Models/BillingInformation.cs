namespace QuokkaServiceRegistry.Models;

public class BillingInformation
{
    public int Id { get; set; }
    public BillingType Type { get; set; }
    public CostCenter CostCenter { get; set; }
    
    // Credit Card Information
    public string? CardHolderName { get; set; }
    public string? CardNumber { get; set; }
    public string? ExpiryDate { get; set; }
    public Address? BillingAddress { get; set; }
    
    // Bank Transfer Information
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
    public string? RoutingNumber { get; set; }
    public string? Iban { get; set; }
    public string? Swift { get; set; }
    
    // SEPA Information
    public string? SepaMandateId { get; set; }
    public DateTime? SepaMandateDate { get; set; }
    public string? SepaCreditorId { get; set; }
    
    // PrePaid Information
    public decimal? PrepaidBalance { get; set; }
    public DateTime? PrepaidExpiryDate { get; set; }
    public string? PrepaidVoucherCode { get; set; }
    
    // General Payment Information
    public string? PaymentMethodId { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? NextBillingDate { get; set; }
    public decimal? OutstandingBalance { get; set; }
}