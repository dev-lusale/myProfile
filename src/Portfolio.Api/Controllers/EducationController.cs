using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EducationController : ControllerBase
{
    private readonly PortfolioDbContext _context;

    public EducationController(PortfolioDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Education>>> GetEducation()
    {
        var education = await _context.Education
            .OrderBy(e => e.DisplayOrder)
            .ToListAsync();

        return Ok(education);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Education>> GetEducation(int id)
    {
        var education = await _context.Education.FindAsync(id);

        if (education == null)
        {
            return NotFound();
        }

        return Ok(education);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Education>> CreateEducation(Education education)
    {
        _context.Education.Add(education);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEducation), new { id = education.Id }, education);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEducation(int id, Education education)
    {
        if (id != education.Id) return BadRequest();

        _context.Entry(education).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await EducationExists(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEducation(int id)
    {
        var education = await _context.Education.FindAsync(id);
        if (education == null) return NotFound();

        _context.Education.Remove(education);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> EducationExists(int id)
    {
        return await _context.Education.AnyAsync(e => e.Id == id);
    }
}