using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Portfolio.Web;
using Portfolio.Web.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient
var apiBaseUrl = builder.HostEnvironment.IsDevelopment() 
    ? "http://localhost:5000/" 
    : "https://your-api-domain.com/"; // Update with your production API URL

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri(apiBaseUrl)
});

// Add services
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddSingleton<Portfolio.Shared.Services.SidebarService>();

await builder.Build().RunAsync();