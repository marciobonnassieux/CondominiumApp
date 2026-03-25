namespace Edifacil.Core.Entities;

public class Profile
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty; // e.g. Admin, Janitor, Resident, Service_Provider
    public string PermissionsList { get; set; } = string.Empty;
    
    public ICollection<User> Users { get; set; } = new List<User>();
}

