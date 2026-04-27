<#
.SYNOPSIS
    Deploy Survival Game WebGL build to GitHub Pages via docs/ folder.

.DESCRIPTION
    After you build the WebGL output from Unity, run this script to copy
    the build into docs/ and push to the main branch. GitHub Pages serves
    from docs/ on main.

.PARAMETER BuildPath
    Path to the Unity WebGL build output folder (contains index.html, Build/, etc.).
    Defaults to .\BUILD\WebGL
#>
param(
    [string]$BuildPath = ".\BUILD\WebGL"
)

$ErrorActionPreference = "Stop"
$RepoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path

# ── 1. Validate build ───────────────────────────────────────────────
if (-not (Test-Path "$BuildPath\index.html")) {
    Write-Error "❌  No index.html found in '$BuildPath'. Build the WebGL target in Unity first."
    exit 1
}
Write-Host "✅  Build found at: $BuildPath" -ForegroundColor Green

# ── 2. Clean & copy to docs/ ────────────────────────────────────────
$DocsDir = Join-Path $RepoRoot "docs"
if (Test-Path $DocsDir) {
    Write-Host "🧹  Cleaning old docs/ ..." -ForegroundColor Yellow
    Remove-Item -Recurse -Force $DocsDir
}

Write-Host "📦  Copying build to docs/ ..." -ForegroundColor Cyan
Copy-Item -Recurse -Force $BuildPath $DocsDir

# Add .nojekyll so GitHub Pages doesn't ignore underscore-prefixed files
New-Item -ItemType File -Path "$DocsDir\.nojekyll" -Force | Out-Null

Write-Host "✅  docs/ ready" -ForegroundColor Green

# ── 3. Git commit & push ────────────────────────────────────────────
Push-Location $RepoRoot
try {
    git add docs/
    $status = git status --porcelain docs/
    if ([string]::IsNullOrWhiteSpace($status)) {
        Write-Host "ℹ️  No changes to deploy." -ForegroundColor Yellow
        exit 0
    }

    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm"
    git commit -m "deploy: WebGL build $timestamp"
    git push origin main
    Write-Host "`n🚀  Deployed! Check https://hoagluan58.github.io/survival-game-mobile/" -ForegroundColor Green
}
finally {
    Pop-Location
}
