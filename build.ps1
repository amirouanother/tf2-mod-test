# PowerShell Build Script for TF2Mod
param(
    [string]$Configuration = "Release",
    [switch]$Clean,
    [switch]$Rebuild
)

$ErrorActionPreference = "Stop"
$WarningPreference = "Continue"

# Colors
function Write-Success { Write-Host $args[0] -ForegroundColor Green }
function Write-Error-Custom { Write-Host $args[0] -ForegroundColor Red }
function Write-Info { Write-Host $args[0] -ForegroundColor Cyan }

Write-Info "================================================"
Write-Info "TF2 C# Mod - Build Script"
Write-Info "================================================"

# Check if dotnet is installed
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Error-Custom "Error: dotnet CLI is not installed or not in PATH"
    Write-Info "Download from: https://dotnet.microsoft.com/download"
    exit 1
}

Write-Info "dotnet version: $(dotnet --version)"

# Clean if requested
if ($Clean) {
    Write-Info "Cleaning build artifacts..."
    dotnet clean -c $Configuration 2>&1 | Out-Null
    Write-Success "Clean completed"
}

# Restore packages
Write-Info "Restoring NuGet packages..."
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Error-Custom "Error: Package restore failed"
    exit 1
}
Write-Success "Packages restored"

# Build
if ($Rebuild) {
    Write-Info "Performing rebuild..."
    dotnet build -c $Configuration --no-restore --force
} else {
    Write-Info "Building solution..."
    dotnet build -c $Configuration --no-restore
}

if ($LASTEXITCODE -ne 0) {
    Write-Error-Custom "Error: Build failed"
    exit 1
}

Write-Success "Build completed successfully!"

# Display output location
$outputDir = "bin\$Configuration\net472"
if (Test-Path $outputDir) {
    Write-Info ""
    Write-Info "================================================"
    Write-Info "Build Output"
    Write-Info "================================================"
    Write-Success "Location: $(Resolve-Path $outputDir)"
    Write-Success "DLL: TF2Mod.dll"
    
    Get-ChildItem $outputDir -Filter "*.dll" | ForEach-Object {
        $size = [math]::Round($_.Length / 1024, 2)
        Write-Success "  - $($_.Name) ($size KB)"
    }
}

Write-Info ""
Write-Success "✓ Build completed successfully!"
