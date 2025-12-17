namespace Portfolio.Shared.DTOs;

public class DashboardMetricsDto
{
    public int TotalVisitors { get; set; }
    public int ActiveSessions { get; set; }
    public int TotalProjectViews { get; set; }
    public int TodayVisitors { get; set; }
    public double AverageSessionDuration { get; set; }
    public int ReturningVisitorPercentage { get; set; }
}

public class ProjectAnalyticsDto
{
    public int ProjectId { get; set; }
    public string ProjectTitle { get; set; } = string.Empty;
    public int Views { get; set; }
    public int GitHubClicks { get; set; }
    public int DemoClicks { get; set; }
    public List<DailyMetric> DailyViews { get; set; } = new();
}

public class DailyMetric
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
}

public class VisitorTrendDto
{
    public DateTime Date { get; set; }
    public int NewVisitors { get; set; }
    public int ReturningVisitors { get; set; }
    public int TotalPageViews { get; set; }
    public double AverageSessionDuration { get; set; }
}

public class TopPageDto
{
    public string PageUrl { get; set; } = string.Empty;
    public string PageTitle { get; set; } = string.Empty;
    public int Views { get; set; }
    public double AverageTimeOnPage { get; set; }
}