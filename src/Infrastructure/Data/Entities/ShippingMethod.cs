namespace Infrastructure.Data.Entities;

public class ShippingMethod : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string CarrierCode { get; set; } = string.Empty;
    public int EstimatedDaysMin { get; set; }
    public int EstimatedDaysMax { get; set; }
    public decimal FlatRate { get; set; }
    public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}
