# Portfolio Platform Build Script
Write-Host "Building Portfolio Platform..." -ForegroundColor Green

# Build Shared Library
Write-Host "Building Shared Library..." -ForegroundColor Yellow
dotnet build src/Portfolio.Shared/Portfolio.Shared.csproj
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Build API
Write-Host "Building API..." -ForegroundColor Yellow
dotnet build src/Portfolio.Api/Portfolio.Api.csproj
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# Build Web App
Write-Host "Building Web App..." -ForegroundColor Yellow
dotnet build src/Portfolio.Web/Portfolio.Web.csproj
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "To run the applications:" -ForegroundColor Cyan
Write-Host "1. API: cd src/Portfolio.Api && dotnet run" -ForegroundColor White
Write-Host "2. Web: cd src/Portfolio.Web && dotnet run" -ForegroundColor White
Write-Host ""
Write-Host "API will be available at: https://localhost:7000" -ForegroundColor White
Write-Host "Web will be available at: https://localhost:7001" -ForegroundColor White