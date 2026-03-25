using Edifacil.Core.Entities;

namespace Edifacil.Core.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}

