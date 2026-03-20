using Condominium.Core.Interfaces;
using Condominium.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Condominium.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly CondominiumDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(CondominiumDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Cpf == request.Cpf);

        if (user == null || user.PasswordHash != request.Password)
            return Unauthorized(new { message = "Invalid credentials" });

        var token = _tokenService.GenerateToken(user);
        return Ok(new { token = token, role = user.Profile?.Name });
    }
}

public record LoginRequest(string Cpf, string Password);
