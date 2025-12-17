using Microsoft.Extensions.Logging;
using Portfolio.MauiBlazor.Services;

namespace Portfolio.MauiBlazor;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        // Configure HttpClient
        builder.Services.AddScoped(sp => new HttpClient 
        { 
            BaseAddress = new Uri("http://localhost:5001/") // API base URL
        });

        // Add services
        builder.Services.AddScoped<IApiService, ApiService>();
        builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
        builder.Services.AddSingleton<Portfolio.Shared.Services.SidebarService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}