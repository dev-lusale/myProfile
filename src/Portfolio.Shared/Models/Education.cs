using System.ComponentModel.DataAnnotations;

namespace Portfolio.Shared.Models;

public class Education
{
    public int Id { get; set; }
    
    [Required]
    public string Degree { get; set; } = string.Empty;
    
    [Required]
    public string Institution { get; set; } = string.Empty;
    
    public string Location { get; set; } = string.Empty;
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public string? ExpectedGraduation { get; set; }
    
    public string? GPA { get; set; }
    
    public string? Description { get; set; }
    
    public List<string> Achievements { get; set; } = new();
    
    public bool IsCompleted { get; set; } = true;
    
    public int DisplayOrder { get; set; }
}

public class Certification
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string IssuingOrganization { get; set; } = string.Empty;
    
    public DateTime IssueDate { get; set; }
    
    public DateTime? ExpiryDate { get; set; }
    
    public string? CredentialId { get; set; }
    
    public string? CredentialUrl { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public int DisplayOrder { get; set; }
}