using Condominium.Core.Entities;

namespace Condominium.Core.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
