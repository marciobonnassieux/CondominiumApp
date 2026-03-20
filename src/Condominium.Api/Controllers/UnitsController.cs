using Condominium.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Condominium.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Janitor,Admin")]
public class UnitsController : ControllerBase
{
    private readonly CondominiumDbContext _context;

    public UnitsController(CondominiumDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var units = await _context.Units
            .Select(u => new { u.Id, u.Block, u.ApartmentNumber })
            .OrderBy(u => u.Block).ThenBy(u => u.ApartmentNumber)
            .ToListAsync();
            
        return Ok(units);
    }
}
