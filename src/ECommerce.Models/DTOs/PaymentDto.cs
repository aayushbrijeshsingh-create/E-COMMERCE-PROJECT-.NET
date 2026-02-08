namespace ECommerce.Models.DTOs;

public class PaymentDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreatePaymentDto
{
    public Guid OrderId { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
}

public class PaymentResultDto
{
    public bool Success { get; set; }
    public Guid? PaymentId { get; set; }
    public string Message { get; set; } = string.Empty;
}
