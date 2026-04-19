param(
    [switch]$d,
    [switch]$o
)

if ($d) {
    if (Test-Path "TestResults") {
        Write-Host "Removing old TestResults folder..."
        Remove-Item "TestResults" -Recurse -Force
    }
}

Write-Host "Running tests with coverage..."
dotnet test --collect:"XPlat Code Coverage"

Write-Host "Generating report..."
reportgenerator -reports:TestResults/**/coverage.cobertura.xml -targetdir:TestResults/coverage-report

if ($o) {
    Write-Host "Opening report..."
    Start-Process "TestResults/coverage-report/index.html"
}

Write-Host "Done!"
