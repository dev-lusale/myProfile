using Portfolio.Shared.Models;
using Portfolio.Shared.DTOs;
using System.Net.Http.Json;

namespace Portfolio.MauiBlazor.Services;

public interface IApiService
{
    Task<List<Project>> GetProjectsAsync();
    Task<Project?> GetProjectAsync(int id);
    Task<List<Skill>> GetSkillsAsync();
    Task<List<Experience>> GetExperienceAsync();
    Task<List<Education>> GetEducationAsync();
    Task<List<Certification>> GetCertificationsAsync();
    Task<bool> SubmitContactAsync(ContactLead contactLead);
    Task<DashboardMetricsDto> GetDashboardMetricsAsync();
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

    public async Task<List<Experience>> GetExperienceAsync()
    {
        try
        {
            var experience = await _httpClient.GetFromJsonAsync<List<Experience>>("api/experience");
            return experience ?? new List<Experience>();
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
}