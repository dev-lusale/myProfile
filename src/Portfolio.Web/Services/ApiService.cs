using Portfolio.Shared.Models;
using Portfolio.Shared.DTOs;
using System.Net.Http.Json;

namespace Portfolio.Web.Services;

public interface IApiService
{
    Task<List<Project>> GetProjectsAsync();
    Task<Project?> GetProjectAsync(int id);
    Task<List<Project>> GetTrendingProjectsAsync();
    Task<List<Skill>> GetSkillsAsync();
    Task<List<Experience>> GetExperiencesAsync();
    Task<List<Education>> GetEducationAsync();
    Task<List<Certification>> GetCertificationsAsync();
    Task<DashboardMetricsDto> GetDashboardMetricsAsync();
    Task TrackProjectEventAsync(int projectId, string eventType);
    Task<string> StartSessionAsync();
    Task TrackPageViewAsync(string sessionId, string pageUrl, string pageTitle);
    Task TrackEventAsync(string eventType, string? eventData = null, string? sessionId = null);
    Task<bool> SubmitContactAsync(ContactLead contactLead);
}

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Project>> GetProjectsAsync()
    {
        try
        {
            var projects = await _httpClient.GetFromJsonAsync<List<Project>>("api/projects");
            return projects ?? new List<Project>();
        }
        catch
        {
            return new List<Project>();
        }
    }

    public async Task<Project?> GetProjectAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Project>($"api/projects/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<Project>> GetTrendingProjectsAsync()
    {
        try
        {
            var projects = await _httpClient.GetFromJsonAsync<List<Project>>("api/projects/trending");
            return projects ?? new List<Project>();
        }
        catch
        {
            return new List<Project>();
        }
    }

    public async Task<List<Skill>> GetSkillsAsync()
    {
        try
        {
            var skills = await _httpClient.GetFromJsonAsync<List<Skill>>("api/skills");
            return skills ?? new List<Skill>();
        }
        catch
        {
            return new List<Skill>();
        }
    }

    public async Task<List<Experience>> GetExperiencesAsync()
    {
        try
        {
            var experiences = await _httpClient.GetFromJsonAsync<List<Experience>>("api/experience");
            return experiences ?? new List<Experience>();
        }
        catch
        {
            return new List<Experience>();
        }
    }

    public async Task<List<Education>> GetEducationAsync()
    {
        try
        {
            var education = await _httpClient.GetFromJsonAsync<List<Education>>("api/education");
            return education ?? new List<Education>();
        }
        catch
        {
            return new List<Education>();
        }
    }

    public async Task<List<Certification>> GetCertificationsAsync()
    {
        try
        {
            var certifications = await _httpClient.GetFromJsonAsync<List<Certification>>("api/certifications");
            return certifications ?? new List<Certification>();
        }
        catch
        {
            return new List<Certification>();
        }
    }

    public async Task<DashboardMetricsDto> GetDashboardMetricsAsync()
    {
        try
        {
            var metrics = await _httpClient.GetFromJsonAsync<DashboardMetricsDto>("api/analytics/dashboard");
            return metrics ?? new DashboardMetricsDto();
        }
        catch
        {
            return new DashboardMetricsDto();
        }
    }

    public async Task TrackProjectEventAsync(int projectId, string eventType)
    {
        try
        {
            await _httpClient.PostAsync($"api/projects/{projectId}/track/{eventType}", null);
        }
        catch
        {
            // Silently fail for analytics
        }
    }

    public async Task<string> StartSessionAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync("api/analytics/session/start", null);
            var result = await response.Content.ReadFromJsonAsync<dynamic>();
            return result?.sessionId ?? Guid.NewGuid().ToString();
        }
        catch
        {
            return Guid.NewGuid().ToString();
        }
    }

    public async Task TrackPageViewAsync(string sessionId, string pageUrl, string pageTitle)
    {
        try
        {
            var request = new { SessionId = sessionId, PageUrl = pageUrl, PageTitle = pageTitle };
            await _httpClient.PostAsJsonAsync("api/analytics/page-view", request);
        }
        catch
        {
            // Silently fail for analytics
        }
    }

    public async Task TrackEventAsync(string eventType, string? eventData = null, string? sessionId = null)
    {
        try
        {
            var request = new { EventType = eventType, EventData = eventData, SessionId = sessionId };
            await _httpClient.PostAsJsonAsync("api/analytics/event", request);
        }
        catch
        {
            // Silently fail for analytics
        }
    }

    public async Task<bool> SubmitContactAsync(ContactLead contactLead)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/contact", contactLead);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}