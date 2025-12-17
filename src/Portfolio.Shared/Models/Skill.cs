using System.ComponentModel.DataAnnotations;

namespace Portfolio.Shared.Models;

public class Skill
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Category { get; set; } = string.Empty; // "Frontend", "Backend", "Database", "DevOps", etc.
    
    [Range(1, 10)]
    public int ProficiencyLevel { get; set; } = 5;
    
    public string? IconUrl { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public int DisplayOrder { get; set; }
}

public class Experience
{
    public int Id { get; set; }
    
    [Required]
    public string Company { get; set; } = string.Empty;
    
    [Required]
    public string Position { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public bool IsCurrent { get; set; }
    
    public List<string> Achievements { get; set; } = new();
    
    public List<string> Technologies { get; set; } = new();
}