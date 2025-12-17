# Build script for Vercel deployment
Write-Host "Building Portfolio for Vercel deployment..." -ForegroundColor Cyan

# Clean previous builds
if (Test-Path "dist") {
    Remove-Item -Recurse -Force "dist"
    Write-Host "Cleaned previous build" -ForegroundColor Green
}

# Build the Blazor WebAssembly project
Write-Host "Building Blazor WebAssembly project..." -ForegroundColor Blue
dotnet publish src/Portfolio.Web/Portfolio.Web.csproj -c Release -o dist

if ($LASTEXITCODE -eq 0) {
    Write-Host "Build completed successfully!" -ForegroundColor Green
    
    # Copy wwwroot contents to root for Vercel
    if (Test-Path "dist/wwwroot") {
        Write-Host "Preparing files for Vercel..." -ForegroundColor Blue
        
        # Copy all files from wwwroot to root directory for Vercel
        Copy-Item -Path "dist/wwwroot/*" -Destination "." -Recurse -Force
        
        Write-Host "Files prepared in root directory" -ForegroundColor Green
        Write-Host ""
        Write-Host "Ready for Vercel deployment!" -ForegroundColor Green
        Write-Host "Run: vercel --prod" -ForegroundColor Yellow
        Write-Host "Or push to GitHub for automatic deployment" -ForegroundColor Yellow
    } else {
        Write-Host "wwwroot directory not found in build output" -ForegroundColor Red
    }
} else {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}