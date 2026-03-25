namespace Edifacil.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;
    
    public ICollection<Unit> Units { get; set; } = new List<Unit>();
}

