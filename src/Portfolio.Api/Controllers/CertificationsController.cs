using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificationsController : ControllerBase
{
    private readonly PortfolioDbContext _context;

    public CertificationsController(PortfolioDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Certification>>> GetCertifications()
    {
        var certifications = await _context.Certifications
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        return Ok(certifications);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Certification>> GetCertification(int id)
    {
        var certification = await _context.Certifications.FindAsync(id);

        if (certification == null || !certification.IsActive)
        {
            return NotFound();
        }

        return Ok(certification);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<Certification>>> GetActiveCertifications()
    {
        var activeCertifications = await _context.Certifications
            .Where(c => c.IsActive && (c.ExpiryDate == null || c.ExpiryDate > DateTime.UtcNow))
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        return Ok(activeCertifications);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Certification>> CreateCertification(Certification certification)
    {
        _context.Certifications.Add(certification);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCertification), new { id = certification.Id }, certification);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCertification(int id, Certification certification)
    {
        if (id != certification.Id) return BadRequest();

        _context.Entry(certification).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CertificationExists(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCertification(int id)
    {
        var certification = await _context.Certifications.FindAsync(id);
        if (certification == null) return NotFound();

        certification.IsActive = false; // Soft delete
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> CertificationExists(int id)
    {
        return await _context.Certifications.AnyAsync(e => e.Id == id);
    }
}