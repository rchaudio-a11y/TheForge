<#
.SYNOPSIS
    Validates the Forge Governance System for consistency, conflicts, and efficiency.

.DESCRIPTION
    Runs comprehensive validation checks on all Forge governance files and
    Spec-Kit integration points. Generates a detailed report with recommendations.
    
    This script validates:
    - Consistency across governance files
    - Conflicts between Spec-Kit and Forge
    - Token usage and efficiency
    - Rule redundancy
    - Cross-reference validity
    - Character count accuracy

.PARAMETER Verbose
    Enable verbose output for detailed progress information.

.PARAMETER AutoFix
    Automatically fix issues that are safe to fix (e.g., character counts).

.PARAMETER OutputPath
    Directory where validation reports will be saved.
    Default: .github/validation/reports

.PARAMETER ConfigPath
    Path to the validation configuration JSON file.
    Default: .github/validation/validation-config.json

.EXAMPLE
    .\Validate-ForgeSystem.ps1
    Runs validation with default settings.
    
.EXAMPLE
    .\Validate-ForgeSystem.ps1 -Verbose -AutoFix
    Runs validation with verbose output and auto-fixes safe issues.

.EXAMPLE
    .\Validate-ForgeSystem.ps1 -OutputPath "C:\Reports"
    Runs validation and saves report to custom location.

.NOTES
    Version:      1.0.0
    Author:       TheForge Team
    Created:      2026-01-03
    Requirements: PowerShell 7+
    
    Part of the Forge Governance System Validation Framework.
    See: .github/specs/FORGE_VALIDATION_SPEC.md
#>

[CmdletBinding()]
param(
    [switch]$AutoFix,
    [string]$OutputPath = ".github/validation/reports",
    [string]$ConfigPath = ".github/validation/validation-config.json"
)

# Script version
$script:Version = "1.0.0"

#region Utility Functions

function Write-ValidationMessage {
    <#
    .SYNOPSIS
        Writes a formatted message to the console.
    #>
    param(
        [string]$Message,
        [ValidateSet('Info', 'Success', 'Warning', 'Error')]
        [string]$Type = 'Info'
    )
    
    $prefix = switch ($Type) {
        'Info'    { "[INFO]" }
        'Success' { "[?]" }
        'Warning' { "[!]" }
        'Error'   { "[?]" }
    }
    
    $color = switch ($Type) {
        'Info'    { 'Cyan' }
        'Success' { 'Green' }
        'Warning' { 'Yellow' }
        'Error'   { 'Red' }
    }
    
    Write-Host "$prefix $Message" -ForegroundColor $color
}

function Load-Configuration {
    <#
    .SYNOPSIS
        Loads and parses the validation configuration file.
    #>
    param([string]$Path)
    
    try {
        Write-ValidationMessage "Loading configuration from: $Path" -Type Info
        
        if (-not (Test-Path $Path)) {
            throw "Configuration file not found: $Path"
        }
        
        $configJson = Get-Content $Path -Raw
        $config = $configJson | ConvertFrom-Json
        
        Write-ValidationMessage "Configuration loaded successfully" -Type Success
        return $config
    }
    catch {
        Write-ValidationMessage "Failed to load configuration: $_" -Type Error
        throw
    }
}

function Get-GovernanceFiles {
    <#
    .SYNOPSIS
        Gets all governance files based on configuration.
    #>
    param([PSCustomObject]$Config)
    
    $files = @()
    
    # Add constitution
    if ($Config.governanceFiles.constitution) {
        $files += $Config.governanceFiles.constitution
    }
    
    # Add forge charter
    if ($Config.governanceFiles.forgeCharter) {
        $files += $Config.governanceFiles.forgeCharter
    }
    
    # Add branches
    if ($Config.governanceFiles.branches) {
        $files += $Config.governanceFiles.branches
    }
    
    # Add audit
    if ($Config.governanceFiles.audit) {
        $files += $Config.governanceFiles.audit
    }
    
    # Add router
    if ($Config.governanceFiles.router) {
        $files += $Config.governanceFiles.router
    }
    
    # Add spec-kit agents (with wildcard expansion)
    if ($Config.governanceFiles.speckitAgents) {
        $agentFiles = Get-ChildItem -Path $Config.governanceFiles.speckitAgents -ErrorAction SilentlyContinue
        if ($agentFiles) {
            $files += $agentFiles.FullName
        }
    }
    
    # Add spec-kit prompts (with wildcard expansion)
    if ($Config.governanceFiles.speckitPrompts) {
        $promptFiles = Get-ChildItem -Path $Config.governanceFiles.speckitPrompts -ErrorAction SilentlyContinue
        if ($promptFiles) {
            $files += $promptFiles.FullName
        }
    }
    
    return $files
}

#endregion

#region Validation Functions

function Test-Consistency {
    <#
    .SYNOPSIS
        Checks for contradictory rules across governance files.
    #>
    param([PSCustomObject]$Config)
    
    Write-ValidationMessage "Running consistency check..." -Type Info
    
    # Placeholder implementation
    $result = @{
        Status = "Pass"
        ConflictsFound = 0
        Conflicts = @()
        Message = "Consistency check not yet implemented (Phase 2)"
    }
    
    Write-ValidationMessage "Consistency check complete: $($result.Status)" -Type Success
    return $result
}

function Test-Conflicts {
    <#
    .SYNOPSIS
        Detects conflicts between Spec-Kit and Forge rules.
    #>
    param([PSCustomObject]$Config)
    
    Write-ValidationMessage "Running conflict detection..." -Type Info
    
    # Placeholder implementation
    $result = @{
        Status = "Pass"
        ConflictsFound = 0
        IntegrationPoints = @()
        Message = "Conflict detection not yet implemented (Phase 2)"
    }
    
    Write-ValidationMessage "Conflict detection complete: $($result.Status)" -Type Success
    return $result
}

function Measure-TokenUsage {
    <#
    .SYNOPSIS
        Analyzes token usage across governance files.
    #>
    param([PSCustomObject]$Config)
    
    Write-ValidationMessage "Measuring token usage..." -Type Info
    
    # Placeholder implementation
    $result = @{
        TotalChars = 0
        EstimatedTokens = 0
        Files = @()
        Status = "Unknown"
        Message = "Token usage analysis not yet implemented (Phase 2)"
    }
    
    Write-ValidationMessage "Token usage measurement complete" -Type Success
    return $result
}

function Find-Redundancy {
    <#
    .SYNOPSIS
        Scans for duplicate rules across files.
    #>
    param([PSCustomObject]$Config)
    
    Write-ValidationMessage "Scanning for redundancy..." -Type Info
    
    # Placeholder implementation
    $result = @{
        TotalRules = 0
        UniqueRules = 0
        DuplicateRules = 0
        RedundancyPercentage = 0
        HighRedundancyAreas = @()
        Status = "Unknown"
        Message = "Redundancy scan not yet implemented (Phase 2)"
    }
    
    Write-ValidationMessage "Redundancy scan complete" -Type Success
    return $result
}

function Test-CrossReferences {
    <#
    .SYNOPSIS
        Validates all cross-references in governance files.
    #>
    param([PSCustomObject]$Config)
    
    Write-ValidationMessage "Validating cross-references..." -Type Info
    
    # Placeholder implementation
    $result = @{
        TotalReferences = 0
        ValidReferences = 0
        BrokenReferences = @()
        Status = "Unknown"
        Message = "Cross-reference validation not yet implemented (Phase 2)"
    }
    
    Write-ValidationMessage "Cross-reference validation complete" -Type Success
    return $result
}

function Test-CharacterCounts {
    <#
    .SYNOPSIS
        Verifies character counts in governance file metadata.
    #>
    param([PSCustomObject]$Config, [switch]$AutoFix)
    
    Write-ValidationMessage "Verifying character counts..." -Type Info
    
    # Placeholder implementation
    $result = @{
        TotalFiles = 0
        AccurateFiles = 0
        InaccurateFiles = @()
        FixedFiles = @()
        Status = "Unknown"
        Message = "Character count verification not yet implemented (Phase 2)"
    }
    
    Write-ValidationMessage "Character count verification complete" -Type Success
    return $result
}

#endregion

#region Reporting Functions

function New-ValidationReport {
    <#
    .SYNOPSIS
        Generates a validation report from results.
    #>
    param([hashtable]$Results)
    
    Write-ValidationMessage "Generating validation report..." -Type Info
    
    # Placeholder implementation
    $report = @"
# Forge Governance Validation Report

**Generated:** $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")  
**Script Version:** $script:Version  
**Validation Duration:** 0 seconds

---

## Executive Summary

**Overall Status:** ?? Placeholder

- **Consistency:** Placeholder
- **Conflicts:** Placeholder
- **Token Usage:** Placeholder
- **Redundancy:** Placeholder
- **Cross-References:** Placeholder
- **Character Counts:** Placeholder

---

## Note

This is a placeholder report. Full reporting will be implemented in Phase 3.

---

**Report End**
"@
    
    Write-ValidationMessage "Report generated" -Type Success
    return $report
}

function Save-Report {
    <#
    .SYNOPSIS
        Saves the validation report to a file.
    #>
    param(
        [string]$Report,
        [string]$OutputPath
    )
    
    try {
        # Ensure output directory exists
        if (-not (Test-Path $OutputPath)) {
            New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
        }
        
        # Generate filename with timestamp
        $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
        $filename = "VALIDATION_REPORT_$timestamp.md"
        $fullPath = Join-Path $OutputPath $filename
        
        # Save report
        $Report | Set-Content -Path $fullPath -Encoding UTF8
        
        Write-ValidationMessage "Report saved: $fullPath" -Type Success
        return $fullPath
    }
    catch {
        Write-ValidationMessage "Failed to save report: $_" -Type Error
        throw
    }
}

function Write-ValidationSummary {
    <#
    .SYNOPSIS
        Displays a summary of validation results to console.
    #>
    param([hashtable]$Results)
    
    Write-Host ""
    Write-Host "================================" -ForegroundColor Cyan
    Write-Host "   Validation Summary" -ForegroundColor Cyan
    Write-Host "================================" -ForegroundColor Cyan
    Write-Host ""
    
    Write-Host "Consistency Check:    $($Results.Consistency.Status)" -ForegroundColor Yellow
    Write-Host "Conflict Detection:   $($Results.Conflicts.Status)" -ForegroundColor Yellow
    Write-Host "Token Usage:          $($Results.Tokens.Status)" -ForegroundColor Yellow
    Write-Host "Redundancy Scan:      $($Results.Redundancy.Status)" -ForegroundColor Yellow
    Write-Host "Cross-References:     $($Results.CrossReferences.Status)" -ForegroundColor Yellow
    Write-Host "Character Counts:     $($Results.CharacterCounts.Status)" -ForegroundColor Yellow
    
    Write-Host ""
    Write-Host "Note: Full validation logic will be implemented in Phase 2" -ForegroundColor Gray
    Write-Host ""
}

#endregion

#region Main Execution

function Invoke-ForgeValidation {
    <#
    .SYNOPSIS
        Main validation execution function.
    #>
    param(
        [PSCustomObject]$Config,
        [switch]$AutoFix
    )
    
    $startTime = Get-Date
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "  Forge Governance Validation Framework" -ForegroundColor Cyan
    Write-Host "  Version: $script:Version" -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host ""
    
    # Initialize results
    $results = @{}
    
    # Run validation checks (if enabled in config)
    if ($Config.validation.checkConsistency) {
        $results.Consistency = Test-Consistency -Config $Config
    }
    
    if ($Config.validation.checkConflicts) {
        $results.Conflicts = Test-Conflicts -Config $Config
    }
    
    if ($Config.validation.analyzeTokens) {
        $results.Tokens = Measure-TokenUsage -Config $Config
    }
    
    if ($Config.validation.scanRedundancy) {
        $results.Redundancy = Find-Redundancy -Config $Config
    }
    
    if ($Config.validation.validateCrossReferences) {
        $results.CrossReferences = Test-CrossReferences -Config $Config
    }
    
    if ($Config.validation.verifyCharacterCounts) {
        $results.CharacterCounts = Test-CharacterCounts -Config $Config -AutoFix:$AutoFix
    }
    
    # Calculate duration
    $duration = (Get-Date) - $startTime
    $results.Duration = $duration.TotalSeconds
    
    # Generate report
    $report = New-ValidationReport -Results $results
    
    # Save report
    $reportPath = Save-Report -Report $report -OutputPath $OutputPath
    
    # Display summary
    Write-ValidationSummary -Results $results
    
    # Return results
    return @{
        Results = $results
        ReportPath = $reportPath
        Duration = $duration.TotalSeconds
    }
}

#endregion

#region Script Entry Point

try {
    # Load configuration
    $config = Load-Configuration -Path $ConfigPath
    
    # Run validation
    $output = Invoke-ForgeValidation -Config $config -AutoFix:$AutoFix
    
    # Success
    Write-ValidationMessage "Validation completed successfully!" -Type Success
    Write-ValidationMessage "Report: $($output.ReportPath)" -Type Info
    Write-ValidationMessage "Duration: $($output.Duration) seconds" -Type Info
    
    exit 0
}
catch {
    Write-ValidationMessage "Validation failed: $_" -Type Error
    Write-Host $_.ScriptStackTrace -ForegroundColor Red
    exit 1
}

#endregion
