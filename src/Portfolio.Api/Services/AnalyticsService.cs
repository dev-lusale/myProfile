using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Data;
using Portfolio.Shared.DTOs;
using Portfolio.Shared.Models;

namespace Portfolio.Api.Services;

public interface IAnalyticsService
{
    Task<DashboardMetricsDto> GetDashboardMetricsAsync();
    Task<List<ProjectAnalyticsDto>> GetProjectAnalyticsAsync();
    Task<List<VisitorTrendDto>> GetVisitorTrendsAsync(int days = 30);
    Task<List<TopPageDto>> GetTopPagesAsync(int limit = 10);
    Task TrackEventAsync(string eventType, string? eventData = null, string? sessionId = null);
    Task TrackPageViewAsync(string sessionId, string pageUrl, string pageTitle);
    Task<string> StartSessionAsync(string userAgent, string ipAddress);
    Task EndSessionAsync(string sessionId);
}

public class AnalyticsService : IAnalyticsService
{
    private readonly PortfolioDbContext _context;

    public AnalyticsService(PortfolioDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardMetricsDto> GetDashboardMetricsAsync()
    {
        var today = DateTime.UtcNow.Date;
        var thirtyDaysAgo = today.AddDays(-30);

        var totalVisitors = await _context.VisitorSessions.CountAsync();
        var activeSessions = await _context.VisitorSessions
            .Where(s => s.EndTime == null && s.StartTime > DateTime.UtcNow.AddHours(-1))
            .CountAsync();
        
        var totalProjectViews = await _context.ProjectAnalytics
            .Where(pa => pa.EventType == "view")
            .CountAsync();
        
        var todayVisitors = await _context.VisitorSessions
            .Where(s => s.StartTime >= today)
            .CountAsync();
        
        var sessionsWithDuration = await _context.VisitorSessions
            .Where(s => s.EndTime != null && s.StartTime > thirtyDaysAgo)
            .ToListAsync();
        
        var avgSessionDuration = sessionsWithDuration.Any() 
            ? sessionsWithDuration.Average(s => (s.EndTime!.Value - s.StartTime).TotalSeconds)
            : 0.0;
        
        var returningVisitors = await _context.VisitorSessions
            .Where(s => s.IsReturningVisitor && s.StartTime > thirtyDaysAgo)
            .CountAsync();
        
        var totalRecentVisitors = await _context.VisitorSessions
            .Where(s => s.StartTime > thirtyDaysAgo)
            .CountAsync();

        return new DashboardMetricsDto
        {
            TotalVisitors = totalVisitors,
            ActiveSessions = activeSessions,
            TotalProjectViews = totalProjectViews,
            TodayVisitors = todayVisitors,
            AverageSessionDuration = avgSessionDuration,
            ReturningVisitorPercentage = totalRecentVisitors > 0 ? (int)((double)returningVisitors / totalRecentVisitors * 100) : 0
        };
    }

    public async Task<List<ProjectAnalyticsDto>> GetProjectAnalyticsAsync()
    {
        var projects = await _context.Projects
            .Include(p => p.Analytics)
            .ToListAsync();

        var result = new List<ProjectAnalyticsDto>();

        foreach (var project in projects)
        {
            var views = project.Analytics.Count(a => a.EventType == "view");
            var githubClicks = project.Analytics.Count(a => a.EventType == "github_click");
            var demoClicks = project.Analytics.Count(a => a.EventType == "demo_click");

            var dailyViews = await _context.ProjectAnalytics
                .Where(pa => pa.ProjectId == project.Id && pa.EventType == "view")
                .GroupBy(pa => pa.Timestamp.Date)
                .Select(g => new DailyMetric
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(dm => dm.Date)
                .ToListAsync();

            result.Add(new ProjectAnalyticsDto
            {
                ProjectId = project.Id,
                ProjectTitle = project.Title,
                Views = views,
                GitHubClicks = githubClicks,
                DemoClicks = demoClicks,
                DailyViews = dailyViews
            });
        }

        return result;
    }

    public async Task<List<VisitorTrendDto>> GetVisitorTrendsAsync(int days = 30)
    {
        var startDate = DateTime.UtcNow.Date.AddDays(-days);
        
        var sessionsByDate = await _context.VisitorSessions
            .Where(s => s.StartTime >= startDate)
            .ToListAsync();

        var trends = sessionsByDate
            .GroupBy(s => s.StartTime.Date)
            .Select(g => new VisitorTrendDto
            {
                Date = g.Key,
                NewVisitors = g.Count(s => !s.IsReturningVisitor),
                ReturningVisitors = g.Count(s => s.IsReturningVisitor),
                TotalPageViews = g.Sum(s => s.PageViewCount),
                AverageSessionDuration = g.Where(s => s.EndTime != null).Any()
                    ? g.Where(s => s.EndTime != null).Average(s => (s.EndTime!.Value - s.StartTime).TotalSeconds)
                    : 0.0
            })
            .OrderBy(t => t.Date)
            .ToList();

        return trends;
    }

    public async Task<List<TopPageDto>> GetTopPagesAsync(int limit = 10)
    {
        var topPages = await _context.PageViews
            .GroupBy(pv => new { pv.PageUrl, pv.PageTitle })
            .Select(g => new TopPageDto
            {
                PageUrl = g.Key.PageUrl,
                PageTitle = g.Key.PageTitle,
                Views = g.Count(),
                AverageTimeOnPage = g.Average(pv => pv.TimeOnPage.TotalSeconds)
            })
            .OrderByDescending(tp => tp.Views)
            .Take(limit)
            .ToListAsync();

        return topPages;
    }

    public async Task TrackEventAsync(string eventType, string? eventData = null, string? sessionId = null)
    {
        var analyticsEvent = new AnalyticsEvent
        {
            EventType = eventType,
            EventData = eventData,
            SessionId = sessionId,
            Timestamp = DateTime.UtcNow
        };

        _context.AnalyticsEvents.Add(analyticsEvent);
        await _context.SaveChangesAsync();
    }

    public async Task TrackPageViewAsync(string sessionId, string pageUrl, string pageTitle)
    {
        var session = await _context.VisitorSessions
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);

        if (session != null)
        {
            var pageView = new PageView
            {
                SessionId = session.Id,
                PageUrl = pageUrl,
                PageTitle = pageTitle,
                Timestamp = DateTime.UtcNow,
                TimeOnPage = TimeSpan.FromSeconds(0) // Will be updated when user leaves page
            };

            _context.PageViews.Add(pageView);
            session.PageViewCount++;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<string> StartSessionAsync(string userAgent, string ipAddress)
    {
        var sessionId = Guid.NewGuid().ToString();
        
        // Check if this is a returning visitor (simplified logic)
        var isReturning = await _context.VisitorSessions
            .AnyAsync(s => s.IpAddress == ipAddress && s.StartTime > DateTime.UtcNow.AddDays(-30));

        var session = new VisitorSession
        {
            SessionId = sessionId,
            StartTime = DateTime.UtcNow,
            UserAgent = userAgent,
            IpAddress = ipAddress,
            IsReturningVisitor = isReturning
        };

        _context.VisitorSessions.Add(session);
        await _context.SaveChangesAsync();

        return sessionId;
    }

    public async Task EndSessionAsync(string sessionId)
    {
        var session = await _context.VisitorSessions
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);

        if (session != null)
        {
            session.EndTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}