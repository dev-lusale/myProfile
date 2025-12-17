using System.ComponentModel.DataAnnotations;

namespace Portfolio.Shared.Models;

public class VisitorSession
{
    public int Id { get; set; }
    
    public string SessionId { get; set; } = string.Empty;
    
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    
    public DateTime? EndTime { get; set; }
    
    public string? UserAgent { get; set; }
    
    public string? IpAddress { get; set; }
    
    public string? Country { get; set; }
    
    public string? City { get; set; }
    
    public bool IsReturningVisitor { get; set; }
    
    public int PageViewCount { get; set; }
    
    public TimeSpan? Duration => EndTime?.Subtract(StartTime);
    
    // Navigation properties
    public List<PageView> PageViews { get; set; } = new();
}

public class PageView
{
    public int Id { get; set; }
    
    public int SessionId { get; set; }
    
    public VisitorSession Session { get; set; } = null!;
    
    public string PageUrl { get; set; } = string.Empty;
    
    public string PageTitle { get; set; } = string.Empty;
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    public TimeSpan TimeOnPage { get; set; }
}

public class ContactLead
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    public string? Company { get; set; }
    
    public string Message { get; set; } = string.Empty;
    
    [Required]
    public string InterestType { get; set; } = string.Empty; // "Hiring", "Collaboration", "Consulting"
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsRead { get; set; }
    
    public string? Notes { get; set; }
}

public class AnalyticsEvent
{
    public int Id { get; set; }
    
    public string EventType { get; set; } = string.Empty; // "cv_download", "contact_form", "project_click"
    
    public string? EventData { get; set; } // JSON data for additional context
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    public string? SessionId { get; set; }
    
    public string? UserAgent { get; set; }
    
    public string? IpAddress { get; set; }
}