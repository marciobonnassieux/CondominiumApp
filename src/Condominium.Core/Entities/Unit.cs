namespace Condominium.Core.Entities;

public class Unit
{
    public Guid Id { get; set; }
    public string Block { get; set; } = string.Empty;
    public string ApartmentNumber { get; set; } = string.Empty;
    
    public Guid ResidentId { get; set; }
    public User Resident { get; set; } = null!;
    
    public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
}
