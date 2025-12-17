using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Services;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly PortfolioDbContext _context;
    private readonly IAnalyticsService _analyticsService;

    public ProjectsController(PortfolioDbContext context, IAnalyticsService analyticsService)
    {
        _context = context;
        _analyticsService = analyticsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
    {
        var projects = await _context.Projects
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project == null || !project.IsActive)
        {
            return NotFound();
        }

        // Track project view
        await TrackProjectEvent(id, "view");

        return Ok(project);
    }

    [HttpGet("trending")]
    public async Task<ActionResult<IEnumerable<Project>>> GetTrendingProjects()
    {
        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
        
        var trendingProjects = await _context.Projects
            .Where(p => p.IsActive)
            .Select(p => new
            {
                Project = p,
                RecentViews = p.Analytics.Count(a => a.EventType == "view" && a.Timestamp > thirtyDaysAgo)
            })
            .OrderByDescending(x => x.RecentViews)
            .Take(6)
            .Select(x => x.Project)
            .ToListAsync();

        return Ok(trendingProjects);
    }

    [HttpPost("{id}/track/{eventType}")]
    public async Task<IActionResult> TrackProjectEvent(int id, string eventType)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return NotFound();

        var analytic = new ProjectAnalytic
        {
            ProjectId = id,
            EventType = eventType,
            Timestamp = DateTime.UtcNow,
            UserAgent = Request.Headers.UserAgent,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
        };

        _context.ProjectAnalytics.Add(analytic);

        // Update project counters
        switch (eventType.ToLower())
        {
            case "view":
                project.ViewCount++;
                break;
            case "github_click":
            case "demo_click":
                project.ClickCount++;
                break;
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject(Project project)
    {
        project.CreatedAt = DateTime.UtcNow;
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, Project project)
    {
        if (id != project.Id) return BadRequest();

        _context.Entry(project).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProjectExists(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return NotFound();

        project.IsActive = false; // Soft delete
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ProjectExists(int id)
    {
        return await _context.Projects.AnyAsync(e => e.Id == id);
    }
}