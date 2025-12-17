# Development Environment Setup Script
Write-Host "Starting Portfolio Platform Development Environment..." -ForegroundColor Green

# Start API in background
Write-Host "Starting API server..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd src/Portfolio.Api; dotnet run"

# Wait a moment for API to start
Start-Sleep -Seconds 3

# Start Web App
Write-Host "Starting Web application..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd src/Portfolio.Web; dotnet run"

Write-Host ""
Write-Host "Development environment started!" -ForegroundColor Green
Write-Host "API: https://localhost:7000" -ForegroundColor Cyan
Write-Host "Web: https://localhost:7001" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press any key to continue..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")