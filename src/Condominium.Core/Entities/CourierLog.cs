namespace Condominium.Core.Entities;

public class CourierLog
{
    public Guid Id { get; set; }
    
    public Guid DeliveryId { get; set; }
    public Delivery Delivery { get; set; } = null!;
    
    public string CourierName { get; set; } = string.Empty;
    public string CourierDocument { get; set; } = string.Empty;
    
    // Digital_Signature or Screen_Print
    public string ConfirmationType { get; set; } = string.Empty; 
    public int PackageCount { get; set; }
    
    public string ProofFileUrl { get; set; } = string.Empty;
}
