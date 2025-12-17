using Blazored.LocalStorage;

namespace Portfolio.Web.Services;

public interface IAnalyticsService
{
    Task InitializeAsync();
    Task TrackPageViewAsync(string pageUrl, string pageTitle);
    Task TrackEventAsync(string eventType, string? eventData = null);
    string SessionId { get; }
}

public class AnalyticsService : IAnalyticsService
{
    private readonly IApiService _apiService;
    private readonly ILocalStorageService _localStorage;
    private string _sessionId = string.Empty;

    public string SessionId => _sessionId;

    public AnalyticsService(IApiService apiService, ILocalStorageService localStorage)
    {
        _apiService = apiService;
        _localStorage = localStorage;
    }

    public async Task InitializeAsync()
    {
        // Get or create session ID
        _sessionId = await _localStorage.GetItemAsync<string>("sessionId") ?? string.Empty;
        
        if (string.IsNullOrEmpty(_sessionId))
        {
            _sessionId = await _apiService.StartSessionAsync();
            await _localStorage.SetItemAsync("sessionId", _sessionId);
        }

        // Check if session is still valid (within 30 minutes)
        var lastActivity = await _localStorage.GetItemAsync<DateTime?>("lastActivity");
        if (lastActivity == null || DateTime.UtcNow.Subtract(lastActivity.Value).TotalMinutes > 30)
        {
            _sessionId = await _apiService.StartSessionAsync();
            await _localStorage.SetItemAsync("sessionId", _sessionId);
        }

        await _localStorage.SetItemAsync("lastActivity", DateTime.UtcNow);
    }

    public async Task TrackPageViewAsync(string pageUrl, string pageTitle)
    {
        if (string.IsNullOrEmpty(_sessionId))
            await InitializeAsync();

        await _apiService.TrackPageViewAsync(_sessionId, pageUrl, pageTitle);
        await _localStorage.SetItemAsync("lastActivity", DateTime.UtcNow);
    }

    public async Task TrackEventAsync(string eventType, string? eventData = null)
    {
        if (string.IsNullOrEmpty(_sessionId))
            await InitializeAsync();

        await _apiService.TrackEventAsync(eventType, eventData, _sessionId);
        await _localStorage.SetItemAsync("lastActivity", DateTime.UtcNow);
    }
}