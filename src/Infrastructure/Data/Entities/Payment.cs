namespace Infrastructure.Data.Entities;

public enum PaymentStatus
{
    Initiated = 1,
    Authorized = 2,
    Captured = 3,
    Failed = 4,
    Refunded = 5
}

public class Payment : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public Guid? PaymentMethodId { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public PaymentStatus Status { get; set; } = PaymentStatus.Initiated;
    public string? Provider { get; set; }
    public ICollection<PaymentTransaction> Transactions { get; set; } = new List<PaymentTransaction>();
}
