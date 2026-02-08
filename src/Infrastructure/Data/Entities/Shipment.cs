namespace Infrastructure.Data.Entities;

public enum ShipmentStatus
{
    Pending = 1,
    Packed = 2,
    Shipped = 3,
    Delivered = 4,
    Returned = 5
}

public class Shipment : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public Guid ShippingMethodId { get; set; }
    public ShippingMethod? ShippingMethod { get; set; }
    public string? TrackingNumber { get; set; }
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
}
