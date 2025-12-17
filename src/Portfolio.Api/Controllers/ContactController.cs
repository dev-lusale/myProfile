using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Api.Services;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly PortfolioDbContext _context;
    private readonly IAnalyticsService _analyticsService;
    private readonly IEmailService _emailService;
    private readonly ILogger<ContactController> _logger;

    public ContactController(
        PortfolioDbContext context, 
        IAnalyticsService analyticsService,
        IEmailService emailService,
        ILogger<ContactController> logger)
    {
        _context = context;
        _analyticsService = analyticsService;
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ContactLead>> SubmitContact(ContactLead contactLead)
    {
        try
        {
            contactLead.CreatedAt = DateTime.UtcNow;
            _context.ContactLeads.Add(contactLead);
            await _context.SaveChangesAsync();

            // Track contact form submission
            await _analyticsService.TrackEventAsync("contact_form_submission", 
                $"Interest: {contactLead.InterestType}, Company: {contactLead.Company}");

            // Send email notifications
            try
            {
                await _emailService.SendContactNotificationAsync(contactLead);
                await _emailService.SendContactConfirmationAsync(contactLead);
                _logger.LogInformation("Email notifications sent successfully for contact submission from {Name}", contactLead.Name);
            }
            catch (Exception emailEx)
            {
                _logger.LogError(emailEx, "Failed to send email notifications for contact submission from {Name}", contactLead.Name);
                // Don't fail the entire request if email fails
            }

            return CreatedAtAction(nameof(GetContactLead), new { id = contactLead.Id }, contactLead);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process contact submission from {Name}", contactLead.Name);
            return StatusCode(500, "An error occurred while processing your request. Please try again.");
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactLead>>> GetContactLeads([FromQuery] bool unreadOnly = false)
    {
        var query = _context.ContactLeads.AsQueryable();
        
        if (unreadOnly)
        {
            query = query.Where(cl => !cl.IsRead);
        }

        var leads = await query
            .OrderByDescending(cl => cl.CreatedAt)
            .ToListAsync();

        return Ok(leads);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactLead>> GetContactLead(int id)
    {
        var contactLead = await _context.ContactLeads.FindAsync(id);

        if (contactLead == null)
        {
            return NotFound();
        }

        return Ok(contactLead);
    }

    [Authorize]
    [HttpPut("{id}/mark-read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var contactLead = await _context.ContactLeads.FindAsync(id);
        if (contactLead == null) return NotFound();

        contactLead.IsRead = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpPut("{id}/notes")]
    public async Task<IActionResult> UpdateNotes(int id, [FromBody] string notes)
    {
        var contactLead = await _context.ContactLeads.FindAsync(id);
        if (contactLead == null) return NotFound();

        contactLead.Notes = notes;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContactLead(int id)
    {
        var contactLead = await _context.ContactLeads.FindAsync(id);
        if (contactLead == null) return NotFound();

        _context.ContactLeads.Remove(contactLead);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}