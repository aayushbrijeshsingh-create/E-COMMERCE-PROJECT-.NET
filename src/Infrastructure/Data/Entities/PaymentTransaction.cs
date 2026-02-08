namespace Infrastructure.Data.Entities;

public enum TransactionType
{
    Auth = 1,
    Capture = 2,
    Refund = 3
}

public enum TransactionStatus
{
    Pending = 1,
    Success = 2,
    Failed = 3
}

public class PaymentTransaction : BaseEntity
{
    public Guid PaymentId { get; set; }
    public Payment? Payment { get; set; }
    public string? GatewayTransactionId { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public string? RawResponse { get; set; }
}
