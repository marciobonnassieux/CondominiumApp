using Edifacil.Core.Entities;
using Edifacil.Core.Enums;
using Edifacil.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Edifacil.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Janitor,Admin")] // RBAC Appied
public class DeliveriesController : ControllerBase
{
    private readonly EdifacilDbContext _context;

    public DeliveriesController(EdifacilDbContext context)
    {
        _context = context;
    }

    [HttpPost("check-in")]
    public async Task<IActionResult> CheckIn([FromBody] DeliveryCheckInRequest request)
    {
        var janitorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (janitorIdClaim == null || !Guid.TryParse(janitorIdClaim.Value, out var janitorId))
            return Unauthorized();

        var unit = await _context.Units.Include(u => u.Resident).FirstOrDefaultAsync(u => u.Id == request.UnitId);
        if (unit == null) return NotFound("Unit not found.");

        var delivery = new Delivery
        {
            Id = Guid.NewGuid(),
            TrackingCode = request.TrackingCode,
            CarrierName = request.CarrierName,
            ReceiptDate = DateTime.UtcNow,
            Status = DeliveryStatus.Received,
            JanitorId = janitorId,
            UnitId = request.UnitId,
            CourierLog = new CourierLog
            {
                Id = Guid.NewGuid(),
                CourierName = request.CourierName,
                CourierDocument = request.CourierDocument,
                ConfirmationType = request.ConfirmationType,
                PackageCount = request.PackageCount,
                ProofFileUrl = request.ProofFileUrl
            }
        };

        _context.Deliveries.Add(delivery);
        await _context.SaveChangesAsync();

        // Simulate sending push notification to unit.Resident
        delivery.Status = DeliveryStatus.Notified;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Delivery checked in successfully", deliveryId = delivery.Id });
    }

    [HttpGet("unit/{unitId}")]
    [Authorize(Roles = "Resident,Janitor,Admin")]
    public async Task<IActionResult> GetByUnit(Guid unitId)
    {
        var deliveries = await _context.Deliveries
            .Include(d => d.CourierLog)
            .Where(d => d.UnitId == unitId)
            .OrderByDescending(d => d.ReceiptDate)
            .Select(d => new 
            {
                d.Id,
                d.TrackingCode,
                d.CarrierName,
                d.ReceiptDate,
                Status = d.Status.ToString(),
                d.CourierLog!.CourierName,
                d.CourierLog.PackageCount
            })
            .ToListAsync();

        return Ok(deliveries);
    }
}

public record DeliveryCheckInRequest(
    string TrackingCode, 
    string CarrierName, 
    Guid UnitId, 
    string CourierName, 
    string CourierDocument, 
    string ConfirmationType, 
    int PackageCount, 
    string ProofFileUrl);

