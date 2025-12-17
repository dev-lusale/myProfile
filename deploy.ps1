# Portfolio Deployment Script
param(
    [Parameter(Mandatory=$false)]
    [string]$Environment = "Development",
    
    [Parameter(Mandatory=$false)]
    [switch]$Docker = $false,
    
    [Parameter(Mandatory=$false)]
    [switch]$Build = $true
)

Write-Host "🚀 Deploying Bernard Lusale Portfolio..." -ForegroundColor Green
Write-Host "Environment: $Environment" -ForegroundColor Yellow

if ($Build) {
    Write-Host "📦 Building projects..." -ForegroundColor Blue
    
    # Build API
    Write-Host "Building API..." -ForegroundColor Cyan
    dotnet build src/Portfolio.Api/Portfolio.Api.csproj -c Release
    if ($LASTEXITCODE -ne 0) { 
        Write-Host "❌ API build failed!" -ForegroundColor Red
        exit 1 
    }
    
    # Build Web
    Write-Host "Building Web..." -ForegroundColor Cyan
    dotnet build src/Portfolio.Web/Portfolio.Web.csproj -c Release
    if ($LASTEXITCODE -ne 0) { 
        Write-Host "❌ Web build failed!" -ForegroundColor Red
        exit 1 
    }
    
    Write-Host "✅ Build completed successfully!" -ForegroundColor Green
}

if ($Docker) {
    Write-Host "🐳 Starting Docker deployment..." -ForegroundColor Blue
    
    # Check if .env exists
    if (-not (Test-Path ".env")) {
        Write-Host "⚠️  .env file not found. Creating from template..." -ForegroundColor Yellow
        Copy-Item ".env.example" ".env"
        Write-Host "📝 Please update .env file with your actual values before continuing." -ForegroundColor Yellow
        Write-Host "Press any key to continue after updating .env..." -ForegroundColor Yellow
        $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    }
    
    # Start Docker Compose
    docker-compose up -d
    if ($LASTEXITCODE -ne 0) { 
        Write-Host "❌ Docker deployment failed!" -ForegroundColor Red
        exit 1 
    }
    
    Write-Host "✅ Docker deployment completed!" -ForegroundColor Green
    Write-Host "🌐 Web: http://localhost" -ForegroundColor Cyan
    Write-Host "🔌 API: http://localhost:5000" -ForegroundColor Cyan
} else {
    Write-Host "💻 Local deployment..." -ForegroundColor Blue
    
    # Publish projects
    Write-Host "Publishing API..." -ForegroundColor Cyan
    dotnet publish src/Portfolio.Api/Portfolio.Api.csproj -c Release -o ./publish/api
    
    Write-Host "Publishing Web..." -ForegroundColor Cyan
    dotnet publish src/Portfolio.Web/Portfolio.Web.csproj -c Release -o ./publish/web
    
    Write-Host "✅ Projects published to ./publish/ folder" -ForegroundColor Green
    Write-Host "📁 API: ./publish/api/" -ForegroundColor Cyan
    Write-Host "📁 Web: ./publish/web/" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "🎉 Deployment completed successfully!" -ForegroundColor Green
Write-Host "📧 Email notifications configured for: bernardlusale20@gmail.com" -ForegroundColor Yellow
Write-Host "📖 See DEPLOYMENT_GUIDE.md for detailed instructions" -ForegroundColor Yellow