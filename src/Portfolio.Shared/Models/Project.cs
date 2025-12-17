using System.ComponentModel.DataAnnotations;

namespace Portfolio.Shared.Models;

public class Project
{
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public string? ImageUrl { get; set; }
    
    public string? GitHubUrl { get; set; }
    
    public string? LiveDemoUrl { get; set; }
    
    public List<string> TechStack { get; set; } = new();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;
    
    public int ViewCount { get; set; }
    
    public int ClickCount { get; set; }
    
    // Navigation properties
    public List<ProjectAnalytic> Analytics { get; set; } = new();
}

public class ProjectAnalytic
{
    public int Id { get; set; }
    
    public int ProjectId { get; set; }
    
    public Project Project { get; set; } = null!;
    
    public string EventType { get; set; } = string.Empty; // "view", "github_click", "demo_click"
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    public string? UserAgent { get; set; }
    
    public string? IpAddress { get; set; }
}