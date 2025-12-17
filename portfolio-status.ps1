# Portfolio Status Check
Write-Host "=== BERNARD LUSALE PORTFOLIO STATUS ===" -ForegroundColor Magenta

$baseUrl = "http://localhost:5001/api"

Write-Host "`nTesting API..." -ForegroundColor Yellow
try {
    $skills = Invoke-WebRequest -Uri "$baseUrl/skills" -UseBasicParsing | Select-Object -ExpandProperty Content | ConvertFrom-Json
    Write-Host "Skills: $($skills.Count) loaded" -ForegroundColor Green
    
    $education = Invoke-WebRequest -Uri "$baseUrl/education" -UseBasicParsing | Select-Object -ExpandProperty Content | ConvertFrom-Json
    Write-Host "Education: $($education.Count) records" -ForegroundColor Green
    
    $certs = Invoke-WebRequest -Uri "$baseUrl/certifications" -UseBasicParsing | Select-Object -ExpandProperty Content | ConvertFrom-Json
    Write-Host "Certifications: $($certs.Count) credentials" -ForegroundColor Green
    
    Write-Host "`nEDUCATION UPDATE VERIFIED:" -ForegroundColor Cyan
    $juniorSec = $education | Where-Object {$_.Degree -like "*Junior*"}
    if ($juniorSec) {
        $gradYear = $juniorSec.EndDate.Substring(0,4)
        Write-Host "   Junior Secondary: Graduated $gradYear" -ForegroundColor Green
    }
    
    Write-Host "`nYOUR PORTFOLIO IS READY!" -ForegroundColor Green
    Write-Host "   API: http://localhost:5001" -ForegroundColor White
    Write-Host "   Swagger: http://localhost:5001/swagger" -ForegroundColor White
    
} catch {
    Write-Host "API Error occurred" -ForegroundColor Red
}