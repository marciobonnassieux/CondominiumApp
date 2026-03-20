using Condominium.Core.Enums;

namespace Condominium.Core.Entities;

public class Delivery
{
    public Guid Id { get; set; }
    public string TrackingCode { get; set; } = string.Empty;
    public string CarrierName { get; set; } = string.Empty;
    public DateTime ReceiptDate { get; set; }
    public DeliveryStatus Status { get; set; }
    
    public Guid JanitorId { get; set; }
    public User Janitor { get; set; } = null!;
    
    public Guid UnitId { get; set; }
    public Unit Unit { get; set; } = null!;
    
    public CourierLog? CourierLog { get; set; }
}
