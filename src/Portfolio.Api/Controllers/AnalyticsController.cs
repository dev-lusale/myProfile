using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Services;
using Portfolio.Shared.DTOs;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardMetricsDto>> GetDashboardMetrics()
    {
        var metrics = await _analyticsService.GetDashboardMetricsAsync();
        return Ok(metrics);
    }

    [Authorize]
    [HttpGet("projects")]
    public async Task<ActionResult<List<ProjectAnalyticsDto>>> GetProjectAnalytics()
    {
        var analytics = await _analyticsService.GetProjectAnalyticsAsync();
        return Ok(analytics);
    }

    [Authorize]
    [HttpGet("trends")]
    public async Task<ActionResult<List<VisitorTrendDto>>> GetVisitorTrends([FromQuery] int days = 30)
    {
        var trends = await _analyticsService.GetVisitorTrendsAsync(days);
        return Ok(trends);
    }

    [Authorize]
    [HttpGet("top-pages")]
    public async Task<ActionResult<List<TopPageDto>>> GetTopPages([FromQuery] int limit = 10)
    {
        var topPages = await _analyticsService.GetTopPagesAsync(limit);
        return Ok(topPages);
    }

    [HttpPost("session/start")]
    public async Task<ActionResult<string>> StartSession()
    {
        var userAgent = Request.Headers.UserAgent.ToString();
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        
        var sessionId = await _analyticsService.StartSessionAsync(userAgent, ipAddress);
        return Ok(new { sessionId });
    }

    [HttpPost("session/{sessionId}/end")]
    public async Task<IActionResult> EndSession(string sessionId)
    {
        await _analyticsService.EndSessionAsync(sessionId);
        return Ok();
    }

    [HttpPost("page-view")]
    public async Task<IActionResult> TrackPageView([FromBody] PageViewRequest request)
    {
        await _analyticsService.TrackPageViewAsync(request.SessionId, request.PageUrl, request.PageTitle);
        return Ok();
    }

    [HttpPost("event")]
    public async Task<IActionResult> TrackEvent([FromBody] EventTrackingRequest request)
    {
        await _analyticsService.TrackEventAsync(request.EventType, request.EventData, request.SessionId);
        return Ok();
    }
}

public class PageViewRequest
{
    public string SessionId { get; set; } = string.Empty;
    public string PageUrl { get; set; } = string.Empty;
    public string PageTitle { get; set; } = string.Empty;
}

public class EventTrackingRequest
{
    public string EventType { get; set; } = string.Empty;
    public string? EventData { get; set; }
    public string? SessionId { get; set; }
}