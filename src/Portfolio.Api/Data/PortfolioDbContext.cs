using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Data;

public class PortfolioDbContext : IdentityDbContext
{
    public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectAnalytic> ProjectAnalytics { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<Education> Education { get; set; }
    public DbSet<Certification> Certifications { get; set; }
    public DbSet<VisitorSession> VisitorSessions { get; set; }
    public DbSet<PageView> PageViews { get; set; }
    public DbSet<ContactLead> ContactLeads { get; set; }
    public DbSet<AnalyticsEvent> AnalyticsEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Project configuration
        builder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.TechStack)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
        });

        // ProjectAnalytic configuration
        builder.Entity<ProjectAnalytic>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Project)
                .WithMany(p => p.Analytics)
                .HasForeignKey(e => e.ProjectId);
        });

        // Experience configuration
        builder.Entity<Experience>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Achievements)
                .HasConversion(
                    v => string.Join('|', v),
                    v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
            entity.Property(e => e.Technologies)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
        });

        // Education configuration
        builder.Entity<Education>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Achievements)
                .HasConversion(
                    v => string.Join('|', v),
                    v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
        });

        // VisitorSession configuration
        builder.Entity<VisitorSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.PageViews)
                .WithOne(p => p.Session)
                .HasForeignKey(p => p.SessionId);
        });

        // Seed data
        SeedData(builder);
    }

    private static void SeedData(ModelBuilder builder)
    {
        builder.Entity<Skill>().HasData(
            // Programming Languages
            new Skill { Id = 1, Name = "C++", Category = "Programming Languages", ProficiencyLevel = 8, DisplayOrder = 1, IsActive = true },
            new Skill { Id = 2, Name = "C#", Category = "Programming Languages", ProficiencyLevel = 9, DisplayOrder = 2, IsActive = true },
            new Skill { Id = 3, Name = "Python", Category = "Programming Languages", ProficiencyLevel = 6, DisplayOrder = 3, IsActive = true },
            
            // .NET Ecosystem
            new Skill { Id = 4, Name = ".NET Framework", Category = ".NET Ecosystem", ProficiencyLevel = 9, DisplayOrder = 1, IsActive = true },
            new Skill { Id = 5, Name = ".NET Core", Category = ".NET Ecosystem", ProficiencyLevel = 9, DisplayOrder = 2, IsActive = true },
            new Skill { Id = 6, Name = ".NET MAUI", Category = ".NET Ecosystem", ProficiencyLevel = 9, DisplayOrder = 3, IsActive = true },
            new Skill { Id = 7, Name = "Blazor WebAssembly", Category = ".NET Ecosystem", ProficiencyLevel = 9, DisplayOrder = 4, IsActive = true },
            new Skill { Id = 8, Name = "Blazor Server", Category = ".NET Ecosystem", ProficiencyLevel = 9, DisplayOrder = 5, IsActive = true },
            new Skill { Id = 9, Name = "Blazor Hybrid", Category = ".NET Ecosystem", ProficiencyLevel = 9, DisplayOrder = 6, IsActive = true },
            
            // Database & SQL
            new Skill { Id = 10, Name = "SQL Queries", Category = "Database & SQL", ProficiencyLevel = 9, DisplayOrder = 1, IsActive = true },
            new Skill { Id = 11, Name = "SQL Joins", Category = "Database & SQL", ProficiencyLevel = 9, DisplayOrder = 2, IsActive = true },
            new Skill { Id = 12, Name = "Relational Schema Design", Category = "Database & SQL", ProficiencyLevel = 8, DisplayOrder = 3, IsActive = true },
            new Skill { Id = 13, Name = "SQL Server", Category = "Database & SQL", ProficiencyLevel = 9, DisplayOrder = 4, IsActive = true },
            new Skill { Id = 14, Name = "Entity Framework Core", Category = "Database & SQL", ProficiencyLevel = 9, DisplayOrder = 5, IsActive = true },
            
            // Web & App Development - Frontend
            new Skill { Id = 15, Name = "Blazor Components", Category = "Web & App Development", ProficiencyLevel = 9, DisplayOrder = 1, IsActive = true },
            new Skill { Id = 16, Name = "Responsive UI Design", Category = "Web & App Development", ProficiencyLevel = 8, DisplayOrder = 2, IsActive = true },
            new Skill { Id = 17, Name = "MVVM Architecture", Category = "Web & App Development", ProficiencyLevel = 8, DisplayOrder = 3, IsActive = true },
            new Skill { Id = 18, Name = "Component-based Design", Category = "Web & App Development", ProficiencyLevel = 9, DisplayOrder = 4, IsActive = true },
            new Skill { Id = 19, Name = "HTML5", Category = "Web & App Development", ProficiencyLevel = 8, DisplayOrder = 5, IsActive = true },
            new Skill { Id = 20, Name = "CSS3", Category = "Web & App Development", ProficiencyLevel = 8, DisplayOrder = 6, IsActive = true },
            
            // Web & App Development - Backend
            new Skill { Id = 21, Name = "ASP.NET Core Web API", Category = "Web & App Development", ProficiencyLevel = 9, DisplayOrder = 7, IsActive = true },
            new Skill { Id = 22, Name = "RESTful API Design", Category = "Web & App Development", ProficiencyLevel = 9, DisplayOrder = 8, IsActive = true },
            new Skill { Id = 23, Name = "Authentication & Authorization", Category = "Web & App Development", ProficiencyLevel = 8, DisplayOrder = 9, IsActive = true },
            new Skill { Id = 24, Name = "API Integration", Category = "Web & App Development", ProficiencyLevel = 9, DisplayOrder = 10, IsActive = true },
            
            // Cybersecurity Skills
            new Skill { Id = 25, Name = "Web Application Security Testing", Category = "Cybersecurity", ProficiencyLevel = 8, DisplayOrder = 1, IsActive = true },
            new Skill { Id = 26, Name = "Penetration Testing", Category = "Cybersecurity", ProficiencyLevel = 7, DisplayOrder = 2, IsActive = true },
            new Skill { Id = 27, Name = "Vulnerability Assessment", Category = "Cybersecurity", ProficiencyLevel = 8, DisplayOrder = 3, IsActive = true },
            new Skill { Id = 28, Name = "Security Incident Response", Category = "Cybersecurity", ProficiencyLevel = 7, DisplayOrder = 4, IsActive = true },
            new Skill { Id = 29, Name = "Threat Analysis", Category = "Cybersecurity", ProficiencyLevel = 7, DisplayOrder = 5, IsActive = true },
            new Skill { Id = 30, Name = "Network Security", Category = "Cybersecurity", ProficiencyLevel = 6, DisplayOrder = 6, IsActive = true },
            
            // Cybersecurity Tools
            new Skill { Id = 31, Name = "Wireshark", Category = "Cybersecurity Tools", ProficiencyLevel = 8, DisplayOrder = 1, IsActive = true },
            new Skill { Id = 32, Name = "Metasploit", Category = "Cybersecurity Tools", ProficiencyLevel = 7, DisplayOrder = 2, IsActive = true },
            new Skill { Id = 33, Name = "Burp Suite", Category = "Cybersecurity Tools", ProficiencyLevel = 8, DisplayOrder = 3, IsActive = true },
            new Skill { Id = 34, Name = "Nmap", Category = "Cybersecurity Tools", ProficiencyLevel = 8, DisplayOrder = 4, IsActive = true },
            new Skill { Id = 35, Name = "Fiddler", Category = "Cybersecurity Tools", ProficiencyLevel = 7, DisplayOrder = 5, IsActive = true },
            
            // Development Tools
            new Skill { Id = 36, Name = "Visual Studio", Category = "Development Tools", ProficiencyLevel = 9, DisplayOrder = 1, IsActive = true },
            new Skill { Id = 37, Name = "VS Code", Category = "Development Tools", ProficiencyLevel = 8, DisplayOrder = 2, IsActive = true },
            new Skill { Id = 38, Name = "Git", Category = "Development Tools", ProficiencyLevel = 9, DisplayOrder = 3, IsActive = true },
            new Skill { Id = 39, Name = "Linux", Category = "Development Tools", ProficiencyLevel = 7, DisplayOrder = 4, IsActive = true },
            new Skill { Id = 40, Name = "Postman", Category = "Development Tools", ProficiencyLevel = 8, DisplayOrder = 5, IsActive = true }
        );

        builder.Entity<Project>().HasData(
            new Project 
            { 
                Id = 1, 
                Title = "TechMart - E-Commerce Platform", 
                Description = "TechMart is a cutting-edge, professional e-commerce platform designed to provide a high-performance shopping experience for consumer and enterprise electronics. Developed with a focus on modern 2025 standards, it combines a sleek, responsive UI/UX with robust backend functionality to drive digital sales and user engagement.",
                GitHubUrl = "https://github.com/Dev-Lusale/TechMart",
                LiveDemoUrl = "",
                ImageUrl = "/images/projects/techmart.jpg",
                TechStack = new List<string> { "ASP.NET Core", "C#", "HTML", "CSS", "JavaScript", "SQL Server", "Entity Framework" },
                CreatedAt = DateTime.UtcNow.AddMonths(-2)
            },
            new Project 
            { 
                Id = 2, 
                Title = "Student Record Thinker - AI Grade Predictor", 
                Description = "A student record thinker that predicts students' grades using machine learning algorithms. This intelligent system analyzes historical academic data, attendance patterns, and performance metrics to provide accurate grade predictions and academic insights for educational institutions.",
                GitHubUrl = "https://github.com/Dev-Lusale/student_record-thinker",
                LiveDemoUrl = "",
                ImageUrl = "/images/projects/student-predictor.jpg",
                TechStack = new List<string> { "C++", "Machine Learning", "Data Analysis", "Algorithms", "Statistical Modeling" },
                CreatedAt = DateTime.UtcNow.AddMonths(-4)
            },
            new Project 
            { 
                Id = 3, 
                Title = "Mukubaico EnviroSafe App - Environmental Safety Platform", 
                Description = "Mukubaico Environmental (MUKUBAICO) is an innovative, technology-driven environmental and mining safety consultancy platform focused on sustainable resource management and pollution prevention. The application provides comprehensive environmental monitoring, safety compliance tracking, and sustainability reporting tools.",
                GitHubUrl = "https://github.com/Dev-Lusale/Mukubaico-enviroSafe-app",
                LiveDemoUrl = "",
                ImageUrl = "/images/projects/mukubaico.jpg",
                TechStack = new List<string> { "Environmental Monitoring", "Safety Compliance", "Data Analytics", "Sustainability Reporting", "Mining Safety" },
                CreatedAt = DateTime.UtcNow.AddMonths(-3)
            }
            }
        );

        builder.Entity<Experience>().HasData(
            new Experience
            {
                Id = 1,
                Company = "Future Interns",
                Position = "Cybersecurity Analyst",
                Description = "Competitive cybersecurity internship focused on protecting digital assets and enhancing security posture. Selected based on strong foundation in cybersecurity, problem-solving skills, and passion for digital security. Engaged in hands-on security testing and incident response activities.",
                StartDate = new DateTime(2024, 6, 12),
                EndDate = new DateTime(2025, 6, 13),
                IsCurrent = true,
                Achievements = new List<string> 
                { 
                    "Selected for competitive cybersecurity internship based on strong technical foundation",
                    "Completed Web Application Security Testing as first major project",
                    "Implemented Security Alert Monitoring & Incident Response procedures",
                    "Gained hands-on experience with penetration testing and vulnerability assessment",
                    "Developed skills in threat analysis and security incident management",
                    "Applied cybersecurity best practices in real-world scenarios"
                },
                Technologies = new List<string> { "Wireshark", "Metasploit", "Burp Suite", "Nmap", "Linux", "Fiddler", "Security Testing", "Incident Response" }
            }
        );

        builder.Entity<Education>().HasData(
            new Education
            {
                Id = 1,
                Degree = "Bachelor of Science in Computer Science",
                Institution = "Copperbelt University",
                Location = "Zambia",
                StartDate = new DateTime(2024, 1, 15),
                ExpectedGraduation = "2028",
                IsCompleted = false,
                Description = "Pursuing a comprehensive computer science degree with focus on software engineering, algorithms, data structures, and modern programming paradigms. Coursework includes advanced topics in artificial intelligence, machine learning, database systems, and software architecture.",
                Achievements = new List<string> 
                { 
                    "Maintained Dean's List status for academic excellence",
                    "Active member of Computer Science Student Association",
                    "Participated in programming competitions and hackathons",
                    "Completed advanced coursework in software engineering and system design"
                },
                DisplayOrder = 1
            },
            new Education
            {
                Id = 2,
                Degree = "Secondary School Certificate",
                Institution = "Choma Secondary School",
                Location = "Zambia",
                StartDate = new DateTime(2020, 1, 15),
                EndDate = new DateTime(2022, 12, 15),
                IsCompleted = true,
                Description = "Completed secondary education with strong performance in mathematics, sciences, and computer studies. Developed foundational knowledge in analytical thinking and problem-solving that laid the groundwork for pursuing computer science.",
                Achievements = new List<string> 
                { 
                    "Headboy Academic",
                    "Participated in inter-school science competitions",
                    "Completed advanced mathematics and physics coursework"
                },
                DisplayOrder = 2
            },
        );

        builder.Entity<Certification>().HasData(
            new Certification
            {
                Id = 1,
                Name = "Cisco Endpoint Security",
                IssuingOrganization = "Cisco",
                IssueDate = new DateTime(2024, 8, 15),
                ExpiryDate = new DateTime(2027, 8, 15),
                CredentialId = "CISCO-ES-2024-BL001",
                Description = "Demonstrates proficiency in endpoint security solutions, threat detection, and incident response using Cisco security technologies. Covers advanced endpoint protection and security monitoring.",
                DisplayOrder = 1
            },
            new Certification
            {
                Id = 2,
                Name = "Cybersecurity Fundamentals",
                IssuingOrganization = "Future Interns",
                IssueDate = new DateTime(2024, 7, 1),
                ExpiryDate = null,
                CredentialId = "FI-CYBER-2024-BL002",
                Description = "Comprehensive cybersecurity training covering web application security testing, penetration testing fundamentals, and security incident response procedures.",
                DisplayOrder = 2
            },
            new Certification
            {
                Id = 3,
                Name = "Web Application Security Testing",
                IssuingOrganization = "Future Interns",
                IssueDate = new DateTime(2024, 9, 15),
                ExpiryDate = null,
                CredentialId = "FI-WAST-2024-BL003",
                Description = "Specialized certification in web application security testing methodologies, vulnerability assessment, and security testing tools including Burp Suite and OWASP testing framework.",
                DisplayOrder = 3
            },
            new Certification
            {
                Id = 4,
                Name = "Computer Science Academic Excellence",
                IssuingOrganization = "Copperbelt University",
                IssueDate = new DateTime(2024, 12, 1),
                ExpiryDate = null,
                CredentialId = "CBU-CS-2024-BL004",
                Description = "Recognition for outstanding academic performance in Computer Science program, maintaining Dean's List status and demonstrating excellence in software engineering coursework.",
                DisplayOrder = 4
            }
        );

        // Seed Analytics Data
        var baseDate = DateTime.UtcNow.AddDays(-30);
        
        // Seed Visitor Sessions
        var visitorSessions = new List<VisitorSession>();
        for (int i = 0; i < 150; i++)
        {
            var sessionDate = baseDate.AddDays(Random.Shared.Next(0, 30)).AddHours(Random.Shared.Next(0, 24));
            visitorSessions.Add(new VisitorSession
            {
                Id = i + 1,
                SessionId = Guid.NewGuid().ToString(),
                StartTime = sessionDate,
                EndTime = sessionDate.AddMinutes(Random.Shared.Next(2, 45)),
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
                IpAddress = $"192.168.1.{Random.Shared.Next(1, 255)}",
                IsReturningVisitor = Random.Shared.Next(0, 100) < 30, // 30% returning visitors
                PageViewCount = Random.Shared.Next(1, 8)
            });
        }

        // Add some active sessions (last hour)
        for (int i = 0; i < 5; i++)
        {
            visitorSessions.Add(new VisitorSession
            {
                Id = 151 + i,
                SessionId = Guid.NewGuid().ToString(),
                StartTime = DateTime.UtcNow.AddMinutes(-Random.Shared.Next(5, 60)),
                EndTime = null, // Active session
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
                IpAddress = $"10.0.0.{Random.Shared.Next(1, 255)}",
                IsReturningVisitor = Random.Shared.Next(0, 100) < 40,
                PageViewCount = Random.Shared.Next(1, 5)
            });
        }

        builder.Entity<VisitorSession>().HasData(visitorSessions);

        // Seed Project Analytics
        var projectAnalytics = new List<ProjectAnalytic>();
        int analyticsId = 1;
        
        for (int projectId = 1; projectId <= 5; projectId++)
        {
            // Add view events
            for (int i = 0; i < Random.Shared.Next(20, 50); i++)
            {
                projectAnalytics.Add(new ProjectAnalytic
                {
                    Id = analyticsId++,
                    ProjectId = projectId,
                    EventType = "view",
                    Timestamp = baseDate.AddDays(Random.Shared.Next(0, 30)).AddHours(Random.Shared.Next(0, 24)),
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
                    IpAddress = $"192.168.1.{Random.Shared.Next(1, 255)}"
                });
            }

            // Add GitHub clicks
            for (int i = 0; i < Random.Shared.Next(5, 15); i++)
            {
                projectAnalytics.Add(new ProjectAnalytic
                {
                    Id = analyticsId++,
                    ProjectId = projectId,
                    EventType = "github_click",
                    Timestamp = baseDate.AddDays(Random.Shared.Next(0, 30)).AddHours(Random.Shared.Next(0, 24)),
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
                    IpAddress = $"192.168.1.{Random.Shared.Next(1, 255)}"
                });
            }

            // Add demo clicks
            for (int i = 0; i < Random.Shared.Next(2, 8); i++)
            {
                projectAnalytics.Add(new ProjectAnalytic
                {
                    Id = analyticsId++,
                    ProjectId = projectId,
                    EventType = "demo_click",
                    Timestamp = baseDate.AddDays(Random.Shared.Next(0, 30)).AddHours(Random.Shared.Next(0, 24)),
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
                    IpAddress = $"192.168.1.{Random.Shared.Next(1, 255)}"
                });
            }
        }

        builder.Entity<ProjectAnalytic>().HasData(projectAnalytics);
    }
}