namespace Infrastructure.Data.Entities;

public class CustomerPreferences : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public bool ReceiveEmails { get; set; } = true;
    public bool ReceiveSms { get; set; } = false;
    public string Language { get; set; } = "en";
    public string Currency { get; set; } = "USD";
}
