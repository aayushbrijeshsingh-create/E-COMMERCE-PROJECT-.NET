namespace Infrastructure.Data.Entities;

public enum PaymentType
{
    Card = 1,
    UPI = 2,
    NetBanking = 3,
    Wallet = 4
}

public class PaymentMethod : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public PaymentType Type { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string? MaskedDetails { get; set; }
    public int? ExpiryMonth { get; set; }
    public int? ExpiryYear { get; set; }
    public bool IsDefault { get; set; } = false;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
