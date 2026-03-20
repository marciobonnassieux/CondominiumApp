using Condominium.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Condominium.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(CondominiumDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Profiles.AnyAsync()) return; // Se já tiver perfis, o banco já foi populado

        var adminProfile = new Profile { Id = Guid.NewGuid(), Name = "Admin", PermissionsList = "All" };
        var janitorProfile = new Profile { Id = Guid.NewGuid(), Name = "Janitor", PermissionsList = "Deliveries" };
        var residentProfile = new Profile { Id = Guid.NewGuid(), Name = "Resident", PermissionsList = "ViewOnly" };

        context.Profiles.AddRange(adminProfile, janitorProfile, residentProfile);

        var janitor = new User
        {
            Id = Guid.NewGuid(),
            Name = "João Porteiro",
            Cpf = "12345678900",
            Email = "joao@condominium.com",
            PasswordHash = "123456", // Texto plano apenas para exemplo de desenvolvimento
            Phone = "11999999999",
            Profile = janitorProfile
        };

        var resident1 = new User { Id = Guid.NewGuid(), Name = "Maria", Cpf = "98765432100", PasswordHash = "123456", Profile = residentProfile };
        var resident2 = new User { Id = Guid.NewGuid(), Name = "Carlos", Cpf = "11122233344", PasswordHash = "123456", Profile = residentProfile };

        context.Users.AddRange(janitor, resident1, resident2);

        var unit1 = new Unit { Id = Guid.NewGuid(), Block = "A", ApartmentNumber = "101", Resident = resident1 };
        var unit2 = new Unit { Id = Guid.NewGuid(), Block = "A", ApartmentNumber = "102", Resident = resident2 };
        var unit3 = new Unit { Id = Guid.NewGuid(), Block = "B", ApartmentNumber = "201", Resident = resident1 };

        context.Units.AddRange(unit1, unit2, unit3);

        await context.SaveChangesAsync();
    }
}
