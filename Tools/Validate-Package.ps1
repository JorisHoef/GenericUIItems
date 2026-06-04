$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $PSScriptRoot
$requiredFiles = @(
    "package.json",
    "README.md",
    "CHANGELOG.md",
    "LICENSE.md",
    "CONTRIBUTING.md",
    "Runtime/GenericUIItems.asmdef",
    "Tests/Editor/GenericUIItems.Tests.asmdef",
    "Samples~/BasicUsage/GenericUIItems.Samples.BasicUsage.asmdef"
)

$requiredDirectories = @(
    "Runtime",
    "Tests/Editor",
    "Samples~/BasicUsage",
    "Tools",
    ".github/workflows"
)

foreach ($directory in $requiredDirectories) {
    $path = Join-Path $root $directory
    if (-not (Test-Path -LiteralPath $path -PathType Container)) {
        throw "Missing required directory: $directory"
    }
}

foreach ($file in $requiredFiles) {
    $path = Join-Path $root $file
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) {
        throw "Missing required file: $file"
    }
}

$package = Get-Content -LiteralPath (Join-Path $root "package.json") -Raw | ConvertFrom-Json
if ($package.name -ne "com.jorishoef.generic-ui-items") {
    throw "Unexpected package name: $($package.name)"
}

if ($package.version -notmatch "^\d+\.\d+\.\d+$") {
    throw "Package version must be semver MAJOR.MINOR.PATCH: $($package.version)"
}

if ($package.dependencies."com.unity.ugui" -ne "1.0.0") {
    throw "Expected dependency com.unity.ugui version 1.0.0"
}

$runtimeAsmdef = Get-Content -LiteralPath (Join-Path $root "Runtime/GenericUIItems.asmdef") -Raw | ConvertFrom-Json
if ($runtimeAsmdef.name -ne "GenericUIItems") {
    throw "Unexpected runtime asmdef name: $($runtimeAsmdef.name)"
}

if ($runtimeAsmdef.references -contains "UnityEditor") {
    throw "Runtime asmdef must not reference UnityEditor"
}

$testAsmdef = Get-Content -LiteralPath (Join-Path $root "Tests/Editor/GenericUIItems.Tests.asmdef") -Raw | ConvertFrom-Json
if ($testAsmdef.references -notcontains "GenericUIItems") {
    throw "Tests asmdef must reference GenericUIItems"
}

$sampleAsmdef = Get-Content -LiteralPath (Join-Path $root "Samples~/BasicUsage/GenericUIItems.Samples.BasicUsage.asmdef") -Raw | ConvertFrom-Json
if ($sampleAsmdef.references -notcontains "GenericUIItems") {
    throw "Sample asmdef must reference GenericUIItems"
}

$forbiddenProjectScaffolding = @("Assets", "Packages", "ProjectSettings")
foreach ($directory in $forbiddenProjectScaffolding) {
    $path = Join-Path $root $directory
    if (Test-Path -LiteralPath $path -PathType Container) {
        throw "Package repository should not contain Unity project scaffolding directory: $directory"
    }
}

$generatedArtifacts = Get-ChildItem -LiteralPath $root -Recurse -Force -File |
    Where-Object { $_.Name -match "\.(unitypackage|zip|tar|tgz)$" }
if ($generatedArtifacts.Count -gt 0) {
    throw "Generated artifacts are present in the package repository."
}

Write-Host "GenericUIItems package validation passed."
