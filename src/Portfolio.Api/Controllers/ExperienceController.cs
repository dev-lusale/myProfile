using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExperienceController : ControllerBase
{
    private readonly PortfolioDbContext _context;

    public ExperienceController(PortfolioDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Experience>>> GetExperiences()
    {
        var experiences = await _context.Experiences
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();

        return Ok(experiences);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Experience>> GetExperience(int id)
    {
        var experience = await _context.Experiences.FindAsync(id);

        if (experience == null)
        {
            return NotFound();
        }

        return Ok(experience);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Experience>> CreateExperience(Experience experience)
    {
        _context.Experiences.Add(experience);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetExperience), new { id = experience.Id }, experience);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExperience(int id, Experience experience)
    {
        if (id != experience.Id) return BadRequest();

        _context.Entry(experience).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ExperienceExists(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExperience(int id)
    {
        var experience = await _context.Experiences.FindAsync(id);
        if (experience == null) return NotFound();

        _context.Experiences.Remove(experience);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ExperienceExists(int id)
    {
        return await _context.Experiences.AnyAsync(e => e.Id == id);
    }
}