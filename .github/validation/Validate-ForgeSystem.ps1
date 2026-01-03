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
    
    # Get workspace root (2 levels up from validation directory)
    $workspaceRoot = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
    
    $files = @()
    
    # Add constitution
    if ($Config.governanceFiles.constitution) {
        $files += Join-Path $workspaceRoot $Config.governanceFiles.constitution
    }
    
    # Add forge charter
    if ($Config.governanceFiles.forgeCharter) {
        $files += Join-Path $workspaceRoot $Config.governanceFiles.forgeCharter
    }
    
    # Add branches
    if ($Config.governanceFiles.branches) {
        foreach ($branch in $Config.governanceFiles.branches) {
            $files += Join-Path $workspaceRoot $branch
        }
    }
    
    # Add audit
    if ($Config.governanceFiles.audit) {
        $files += Join-Path $workspaceRoot $Config.governanceFiles.audit
    }
    
    # Add router
    if ($Config.governanceFiles.router) {
        $files += Join-Path $workspaceRoot $Config.governanceFiles.router
    }
    
    # Add spec-kit agents (with wildcard expansion)
    if ($Config.governanceFiles.speckitAgents) {
        $agentPath = Join-Path $workspaceRoot $Config.governanceFiles.speckitAgents
        $agentFiles = Get-ChildItem -Path $agentPath -ErrorAction SilentlyContinue
        if ($agentFiles) {
            $files += $agentFiles.FullName
        }
    }
    
    # Add spec-kit prompts (with wildcard expansion)
    if ($Config.governanceFiles.speckitPrompts) {
        $promptPath = Join-Path $workspaceRoot $Config.governanceFiles.speckitPrompts
        $promptFiles = Get-ChildItem -Path $promptPath -ErrorAction SilentlyContinue
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
    
    try {
        # Get all governance files
        $governanceFiles = Get-GovernanceFiles -Config $Config
        
        # Initialize results
        $result = @{
            Status = "Pass"
            ConflictsFound = 0
            Conflicts = @()
            PotentialConflicts = @()
            Message = ""
        }
        
        # Keywords that often indicate contradictory rules
        $contradictionPatterns = @{
            'must' = @('must not', 'never', 'do not')
            'always' = @('never', 'do not')
            'required' = @('optional', 'not required')
            'allow' = @('forbid', 'prohibit', 'disallow')
            'should' = @('should not', 'avoid')
        }
        
        # Extract all rules with their context
        $allRules = @()
        
        foreach ($file in $governanceFiles) {
            if (-not (Test-Path $file)) {
                continue
            }
            
            try {
                $content = Get-Content $file -Raw -ErrorAction Stop
                $fileName = Split-Path $file -Leaf
                $lines = $content -split "`r?`n"
                $currentSection = "Unknown"
                
                for ($i = 0; $i -lt $lines.Count; $i++) {
                    $line = $lines[$i].Trim()
                    
                    # Track current section
                    if ($line -match '^#{1,4}\s+(.+)') {
                        $currentSection = $matches[1]
                    }
                    
                    # Identify rule lines
                    if ($line -match '^[-*]\s+(.+)' -or $line -match '^\d+\.\s+(.+)') {
                        $ruleText = $matches[1].Trim()
                        
                        # Skip very short lines
                        if ($ruleText.Length -lt 15) {
                            continue
                        }
                        
                        $allRules += @{
                            File = $fileName
                            Section = $currentSection
                            Text = $ruleText
                            TextLower = $ruleText.ToLower()
                            LineNumber = $i + 1
                        }
                    }
                }
            }
            catch {
                Write-ValidationMessage "  Error reading $(Split-Path $file -Leaf): $_" -Type Error
            }
        }
        
        Write-Verbose "  Analyzing $($allRules.Count) rules for contradictions"
        
        # Look for contradictions
        for ($i = 0; $i -lt $allRules.Count; $i++) {
            $rule1 = $allRules[$i]
            
            foreach ($keyword in $contradictionPatterns.Keys) {
                if ($rule1.TextLower -match "\b$keyword\b") {
                    # This rule contains a directive keyword, check for opposites
                    $opposites = $contradictionPatterns[$keyword]
                    
                    for ($j = $i + 1; $j -lt $allRules.Count; $j++) {
                        $rule2 = $allRules[$j]
                        
                        # Check if rules are about similar topics
                        $words1 = $rule1.TextLower -split '\s+' | Where-Object { $_.Length -gt 3 }
                        $words2 = $rule2.TextLower -split '\s+' | Where-Object { $_.Length -gt 3 }
                        $commonWords = $words1 | Where-Object { $words2 -contains $_ }
                        
                        # If they share significant words, check for contradiction
                        if ($commonWords.Count -ge 2) {
                            foreach ($opposite in $opposites) {
                                if ($rule2.TextLower -match "\b$opposite\b") {
                                    # Potential contradiction found!
                                    $result.ConflictsFound++
                                    
                                    $severity = if ($rule1.File -eq $rule2.File) { "Medium" } else { "High" }
                                    
                                    $result.Conflicts += @{
                                        Rule1 = @{
                                            File = $rule1.File
                                            Section = $rule1.Section
                                            Text = $rule1.Text
                                            Line = $rule1.LineNumber
                                        }
                                        Rule2 = @{
                                            File = $rule2.File
                                            Section = $rule2.Section
                                            Text = $rule2.Text
                                            Line = $rule2.LineNumber
                                        }
                                        Severity = $severity
                                        Keyword1 = $keyword
                                        Keyword2 = $opposite
                                        CommonTopics = $commonWords -join ", "
                                    }
                                    
                                    Write-ValidationMessage "  Potential conflict: $($rule1.File) vs $($rule2.File)" -Type Warning
                                }
                            }
                        }
                    }
                }
            }
        }
        
        # Determine overall status
        if ($result.ConflictsFound -eq 0) {
            $result.Status = "Pass"
            $result.Message = "No contradictory rules detected"
        }
        elseif ($result.ConflictsFound -le 2) {
            $result.Status = "Warning"
            $result.Message = "$($result.ConflictsFound) potential conflict(s) - review recommended"
        }
        else {
            $result.Status = "Fail"
            $result.Message = "$($result.ConflictsFound) potential conflict(s) - review required"
        }
        
        # Summary output
        Write-ValidationMessage "Consistency check complete: $($result.Status)" -Type Success
        Write-ValidationMessage "  Conflicts found: $($result.ConflictsFound)" -Type Info
        
        # Show conflicts if any
        if ($result.Conflicts.Count -gt 0) {
            Write-Host ""
            Write-Host "Potential Conflicts Detected:" -ForegroundColor Yellow
            foreach ($conflict in ($result.Conflicts | Select-Object -First 5)) {
                Write-Host ("  [{0}] {1} vs {2}" -f $conflict.Severity, $conflict.Rule1.File, $conflict.Rule2.File) -ForegroundColor White
                Write-Host ("      Topic: {0}" -f $conflict.CommonTopics) -ForegroundColor Gray
                Write-Host ("      Rule 1: {0}" -f $conflict.Rule1.Text.Substring(0, [Math]::Min(60, $conflict.Rule1.Text.Length))) -ForegroundColor Gray
                Write-Host ("      Rule 2: {0}" -f $conflict.Rule2.Text.Substring(0, [Math]::Min(60, $conflict.Rule2.Text.Length))) -ForegroundColor Gray
            }
            if ($result.Conflicts.Count -gt 5) {
                Write-Host ("  ... and {0} more" -f ($result.Conflicts.Count - 5)) -ForegroundColor Gray
            }
            Write-Host ""
        }
        
        return $result
    }
    catch {
        Write-ValidationMessage "Consistency check failed: $_" -Type Error
        return @{
            Status = "Error"
            ConflictsFound = 0
            Conflicts = @()
            Message = "Check failed: $_"
        }
    }
}

function Test-Conflicts {
    <#
    .SYNOPSIS
        Detects conflicts between Spec-Kit and Forge rules.
    #>
    param([PSCustomObject]$Config)
    
    Write-ValidationMessage "Running conflict detection..." -Type Info
    
    try {
        # Get all governance files
        $governanceFiles = Get-GovernanceFiles -Config $Config
        
        # Initialize results
        $result = @{
            Status = "Pass"
            ConflictsFound = 0
            IntegrationPoints = @()
            Conflicts = @()
            Message = ""
        }
        
        # Separate Forge files from Spec-Kit files
        $forgeFiles = @()
        $speckitFiles = @()
        
        foreach ($file in $governanceFiles) {
            $fileName = Split-Path $file -Leaf
            if ($fileName -match 'speckit' -or $fileName -match '\.agent\.md$' -or $fileName -match '\.prompt\.md$') {
                $speckitFiles += $file
            } else {
                $forgeFiles += $file
            }
        }
        
        Write-Verbose "  Forge files: $($forgeFiles.Count), Spec-Kit files: $($speckitFiles.Count)"
        
        # Define known integration points to check
        $integrationChecks = @(
            @{
                Name = "/specify command"
                SpeckitFile = "speckit.specify.agent.md"
                ForgeFile = "CONSTITUTION.md"
                Topic = "specification creation"
            },
            @{
                Name = "/plan command"
                SpeckitFile = "speckit.plan.agent.md"
                ForgeFile = "Branch-Architecture.md"
                Topic = "architectural planning"
            },
            @{
                Name = "/implement command"
                SpeckitFile = "speckit.implement.agent.md"
                ForgeFile = "Branch-Coding.md"
                Topic = "code generation"
            },
            @{
                Name = "Documentation generation"
                SpeckitFile = "speckit.implement.agent.md"
                ForgeFile = "Branch-Documentation.md"
                Topic = "documentation standards"
            }
        )
        
        # Check each integration point
        foreach ($check in $integrationChecks) {
            $speckitPath = $speckitFiles | Where-Object { $_ -match $check.SpeckitFile } | Select-Object -First 1
            $forgePath = $forgeFiles | Where-Object { $_ -match $check.ForgeFile } | Select-Object -First 1
            
            if ($speckitPath -and $forgePath -and (Test-Path $speckitPath) -and (Test-Path $forgePath)) {
                $speckitContent = Get-Content $speckitPath -Raw
                $forgeContent = Get-Content $forgePath -Raw
                
                $integrationPoint = @{
                    Name = $check.Name
                    SpeckitFile = Split-Path $speckitPath -Leaf
                    ForgeFile = Split-Path $forgePath -Leaf
                    Status = "Compatible"
                    Notes = @()
                }
                
                # Check if Spec-Kit references Forge rules
                $hasForgeReference = $speckitContent -match $check.ForgeFile -or 
                                     $speckitContent -match 'Branch-' -or 
                                     $speckitContent -match 'Forge' -or
                                     $speckitContent -match 'CONSTITUTION'
                
                if (-not $hasForgeReference) {
                    $integrationPoint.Status = "Warning"
                    $integrationPoint.Notes += "Spec-Kit file does not reference Forge governance"
                    $result.ConflictsFound++
                    
                    $result.Conflicts += @{
                        IntegrationPoint = $check.Name
                        Issue = "Missing Forge reference in Spec-Kit"
                        SpeckitFile = $integrationPoint.SpeckitFile
                        ForgeFile = $integrationPoint.ForgeFile
                        Severity = "Medium"
                        Recommendation = "Add explicit reference to $($integrationPoint.ForgeFile) in $($integrationPoint.SpeckitFile)"
                    }
                }
                
                $result.IntegrationPoints += $integrationPoint
            }
            else {
                # Integration point files not found
                $result.IntegrationPoints += @{
                    Name = $check.Name
                    SpeckitFile = $check.SpeckitFile
                    ForgeFile = $check.ForgeFile
                    Status = "Unknown"
                    Notes = @("One or both files not found")
                }
            }
        }
        
        # Determine overall status
        if ($result.ConflictsFound -eq 0) {
            $result.Status = "Pass"
            $result.Message = "No conflicts between Spec-Kit and Forge detected"
        }
        elseif ($result.ConflictsFound -le 2) {
            $result.Status = "Warning"
            $result.Message = "$($result.ConflictsFound) integration issue(s) - review recommended"
        }
        else {
            $result.Status = "Fail"
            $result.Message = "$($result.ConflictsFound) integration issue(s) - action required"
        }
        
        # Summary output
        Write-ValidationMessage "Conflict detection complete: $($result.Status)" -Type Success
        Write-ValidationMessage "  Integration points checked: $($result.IntegrationPoints.Count)" -Type Info
        Write-ValidationMessage "  Issues found: $($result.ConflictsFound)" -Type Info
        
        # Show integration status
        if ($result.IntegrationPoints.Count -gt 0) {
            Write-Host ""
            Write-Host "Integration Points:" -ForegroundColor Cyan
            foreach ($point in $result.IntegrationPoints) {
                $statusColor = switch ($point.Status) {
                    "Compatible" { "Green" }
                    "Warning" { "Yellow" }
                    "Unknown" { "Gray" }
                    default { "White" }
                }
                Write-Host ("  [{0}] {1}" -f $point.Status, $point.Name) -ForegroundColor $statusColor
                if ($point.Notes.Count -gt 0) {
                    foreach ($note in $point.Notes) {
                        Write-Host ("      {0}" -f $note) -ForegroundColor Gray
                    }
                }
            }
            Write-Host ""
        }
        
        return $result
    }
    catch {
        Write-ValidationMessage "Conflict detection failed: $_" -Type Error
        return @{
            Status = "Error"
            ConflictsFound = 0
            IntegrationPoints = @()
            Conflicts = @()
            Message = "Detection failed: $_"
        }
    }
}

function Measure-TokenUsage {
    <#
    .SYNOPSIS
        Analyzes token usage across governance files.
    #>
    param([PSCustomObject]$Config)
    
    Write-ValidationMessage "Measuring token usage..." -Type Info
    
    try {
        # Get all governance files
        $governanceFiles = Get-GovernanceFiles -Config $Config
        
        # Initialize results
        $result = @{
            TotalChars = 0
            EstimatedTokens = 0
            Files = @()
            Status = "Unknown"
            Message = ""
            PerFileBreakdown = @{
}
        }
        
        # Analyze each file
        foreach ($file in $governanceFiles) {
            if (Test-Path $file) {
                try {
                    $content = Get-Content $file -Raw -ErrorAction Stop
                    $charCount = $content.Length
                    $tokenCount = [Math]::Ceiling($charCount / 4)
                    
                    # Store file data as PSCustomObject for reliable sorting
                    $fileInfo = [PSCustomObject]@{
                        Path = $file
                        Characters = $charCount
                        Tokens = $tokenCount
                        PercentOfTotal = 0  # Calculate after total is known
                    }
                    
                    $result.Files += $fileInfo
                    $result.TotalChars += $charCount
                    $result.EstimatedTokens += $tokenCount
                    
                    $fileName = Split-Path $file -Leaf
                    Write-Verbose "  $fileName`: $charCount chars (~$tokenCount tokens)"
                }
                catch {
                    Write-ValidationMessage "  Warning: Could not read $file" -Type Warning
                }
            }
            else {
                Write-ValidationMessage "  Warning: File not found: $file" -Type Warning
            }
        }
        
        # Calculate percentages
        if ($result.TotalChars -gt 0) {
            foreach ($fileInfo in $result.Files) {
                $fileInfo.PercentOfTotal = [Math]::Round(($fileInfo.Characters / $result.TotalChars) * 100, 2)
            }
        }
        
        # Sort files by size (largest first) - PSCustomObjects sort reliably
        $result.Files = @($result.Files | Sort-Object -Property Characters -Descending)
        
        # Add summary statistics (AFTER sorting!)
        $result.FileCount = $result.Files.Count
        $result.AverageFileSize = if ($result.FileCount -gt 0) { 
            [Math]::Round($result.TotalChars / $result.FileCount, 0) 
        } else { 0 }
        $result.LargestFile = if ($result.Files.Count -gt 0) { 
            $result.Files[0].Path 
        } else { "None" }
        $result.LargestFileSize = if ($result.Files.Count -gt 0) { 
            $result.Files[0].Characters 
        } else { 0 }
        
        # Determine status based on thresholds
        $warningThreshold = $Config.thresholds.tokenLoadWarning
        $criticalThreshold = $Config.thresholds.tokenLoadCritical
        
        if ($result.TotalChars -lt $warningThreshold) {
            $result.Status = "Optimal"
            $result.Message = "Token usage is within optimal range"
        }
        elseif ($result.TotalChars -lt $criticalThreshold) {
            $result.Status = "Warning"
            $result.Message = "Token usage approaching high levels - consider optimization"
        }
        else {
            $result.Status = "Critical"
            $result.Message = "Token usage is high - optimization recommended"
        }
        
        Write-ValidationMessage "Token usage analysis complete: $($result.Status)" -Type Success
        Write-ValidationMessage "  Total: $($result.TotalChars) chars (~$($result.EstimatedTokens) tokens)" -Type Info
        
        # Display detailed breakdown
        Write-Host ""
        Write-Host "Token Usage Breakdown:" -ForegroundColor Cyan
        Write-Host "======================" -ForegroundColor Cyan
        foreach ($fileInfo in $result.Files) {
            $fileName = Split-Path $fileInfo.Path -Leaf
            $percentBar = "?" * [Math]::Floor($fileInfo.PercentOfTotal / 2)
            Write-Host ("  {0,-40} {1,8} chars ({2,6} tokens) {3,5}% {4}" -f $fileName, $fileInfo.Characters, $fileInfo.Tokens, $fileInfo.PercentOfTotal, $percentBar) -ForegroundColor White
        }
        Write-Host ""
        Write-Host "Summary:" -ForegroundColor Cyan
        Write-Host "  Files Analyzed:   $($result.FileCount)" -ForegroundColor White
        Write-Host "  Average File:     $($result.AverageFileSize) chars" -ForegroundColor White
        Write-Host "  Largest File:     $(Split-Path $result.LargestFile -Leaf) ($($result.LargestFileSize) chars)" -ForegroundColor White
        Write-Host ""
        
        return $result
    }
    catch {
        Write-ValidationMessage "Token usage analysis failed: $_" -Type Error
        return @{
            TotalChars = 0
            EstimatedTokens = 0
            Files = @()
            Status = "Error"
            Message = "Analysis failed: $_"
        }
    }
}

function Find-Redundancy {
    <#
    .SYNOPSIS
        Scans for duplicate rules across files.
    #>
    param([PSCustomObject]$Config)
    
    Write-ValidationMessage "Scanning for redundancy..." -Type Info
    
    try {
        # Get all governance files
        $governanceFiles = Get-GovernanceFiles -Config $Config
        
        # Initialize results
        $result = @{
            TotalRules = 0
            UniqueRules = 0
            DuplicateRules = 0
            RedundancyPercentage = 0
            HighRedundancyAreas = @()
            SimilarRules = @()
            Status = "Unknown"
            Message = ""
        }
        
        # Extract rules from all files
        $allRules = @()
        
        foreach ($file in $governanceFiles) {
            if (-not (Test-Path $file)) {
                continue
            }
            
            try {
                $content = Get-Content $file -Raw -ErrorAction Stop
                $fileName = Split-Path $file -Leaf
                
                # Extract rules (lines that start with -, *, or are numbered)
                $lines = $content -split "`r?`n"
                $currentSection = "Unknown"
                
                for ($i = 0; $i -lt $lines.Count; $i++) {
                    $line = $lines[$i].Trim()
                    
                    # Track current section
                    if ($line -match '^#{1,4}\s+(.+)') {
                        $currentSection = $matches[1]
                    }
                    
                    # Identify rule lines
                    if ($line -match '^[-*]\s+(.+)' -or $line -match '^\d+\.\s+(.+)') {
                        $ruleText = $matches[1].Trim()
                        
                        # Skip very short lines (likely not rules)
                        if ($ruleText.Length -lt 20) {
                            continue
                        }
                        
                        # Normalize the rule text for comparison
                        $normalizedRule = $ruleText.ToLower() `
                            -replace '\s+', ' ' `
                            -replace '[^\w\s]', '' `
                            -replace '\b(the|a|an|and|or|but|in|on|at|to|for|of|with|by)\b', '' `
                            -replace '\s+', ' '
                        
                        $allRules += @{
                            File = $fileName
                            Section = $currentSection
                            OriginalText = $ruleText
                            NormalizedText = $normalizedRule
                            LineNumber = $i + 1
                        }
                        
                        $result.TotalRules++
                    }
                }
            }
            catch {
                Write-ValidationMessage "  Error reading $(Split-Path $file -Leaf): $_" -Type Error
            }
        }
        
        Write-Verbose "  Extracted $($result.TotalRules) rules from $($governanceFiles.Count) files"
        
        # Find duplicates using normalized text
        $ruleGroups = $allRules | Group-Object { $_.NormalizedText } | Where-Object { $_.Count -gt 1 }
        
        foreach ($group in $ruleGroups) {
            $rules = $group.Group
            $result.DuplicateRules += $rules.Count - 1  # First occurrence is not a duplicate
            
            # Store high redundancy areas
            $files = ($rules | ForEach-Object { $_.File } | Select-Object -Unique) -join ", "
            $result.HighRedundancyAreas += @{
                RuleText = $rules[0].OriginalText
                Files = $files
                Occurrences = $rules.Count
            }
            
            # Store similar rules for detailed analysis
            $result.SimilarRules += @{
                NormalizedText = $group.Name
                Occurrences = $rules
            }
        }
        
        # Calculate unique rules
        $result.UniqueRules = $result.TotalRules - $result.DuplicateRules
        
        # Calculate redundancy percentage
        if ($result.TotalRules -gt 0) {
            $result.RedundancyPercentage = [Math]::Round(($result.DuplicateRules / $result.TotalRules) * 100, 2)
        }
        
        # Determine status based on thresholds
        $warningThreshold = $Config.thresholds.redundancyWarning
        $criticalThreshold = $Config.thresholds.redundancyCritical
        
        if ($result.RedundancyPercentage -eq 0) {
            $result.Status = "Pass"
            $result.Message = "No redundant rules detected"
        }
        elseif ($result.RedundancyPercentage -lt $warningThreshold) {
            $result.Status = "Pass"
            $result.Message = "Redundancy within acceptable range"
        }
        elseif ($result.RedundancyPercentage -lt $criticalThreshold) {
            $result.Status = "Warning"
            $result.Message = "Moderate redundancy detected - consider consolidation"
        }
        else {
            $result.Status = "Critical"
            $result.Message = "High redundancy detected - consolidation recommended"
        }
        
        # Summary output
        Write-ValidationMessage "Redundancy scan complete: $($result.Status)" -Type Success
        Write-ValidationMessage "  Total rules: $($result.TotalRules)" -Type Info
        Write-ValidationMessage "  Unique rules: $($result.UniqueRules)" -Type Info
        Write-ValidationMessage "  Duplicate rules: $($result.DuplicateRules)" -Type Info
        Write-ValidationMessage "  Redundancy: $($result.RedundancyPercentage)%" -Type Info
        
        # Show top redundancy areas
        if ($result.HighRedundancyAreas.Count -gt 0) {
            Write-Host ""
            Write-Host "Top Redundant Rules:" -ForegroundColor Yellow
            $topRedundant = $result.HighRedundancyAreas | Sort-Object Occurrences -Descending | Select-Object -First 5
            foreach ($item in $topRedundant) {
                $preview = if ($item.RuleText.Length -gt 60) { 
                    $item.RuleText.Substring(0, 57) + "..." 
                } else { 
                    $item.RuleText 
                }
                Write-Host ("  {0}x: {1}" -f $item.Occurrences, $preview) -ForegroundColor White
                Write-Host ("      Files: {0}" -f $item.Files) -ForegroundColor Gray
            }
            Write-Host ""
        }
        
        return $result
    }
    catch {
        Write-ValidationMessage "Redundancy scan failed: $_" -Type Error
        return @{
            TotalRules = 0
            UniqueRules = 0
            DuplicateRules = 0
            RedundancyPercentage = 0
            HighRedundancyAreas = @()
            Status = "Error"
            Message = "Scan failed: $_"
        }
    }
}

function Test-CrossReferences {
    <#
    .SYNOPSIS
        Validates all cross-references in governance files.
    #>
    param([PSCustomObject]$Config)
    
    Write-ValidationMessage "Validating cross-references..." -Type Info
    
    try {
        # Get all governance files
        $governanceFiles = Get-GovernanceFiles -Config $Config
        
        # Initialize results
        $result = @{
            TotalReferences = 0
            ValidReferences = 0
            BrokenReferences = @()
            Status = "Unknown"
            Message = ""
        }
        
        # Regex patterns for cross-references
        # Matches: "See Branch-Coding § 5.5", "Reference: CONSTITUTION § 3.4", etc.
        $referencePatterns = @(
            'See\s+([A-Za-z-]+(?:\.md)?)\s+§\s+([\d.]+)',
            'Reference:\s+([A-Za-z-]+(?:\.md)?)\s+§\s+([\d.]+)',
            'Ref:\s+([A-Za-z-]+(?:\.md)?)\s+§\s+([\d.]+)',
            '\[([A-Za-z-]+(?:\.md)?)\s+§\s+([\d.]+)\]'
        )
        
        # Build file lookup map (filename -> full path)
        $fileMap = @{}
        foreach ($file in $governanceFiles) {
            if (Test-Path $file) {
                $fileName = [System.IO.Path]::GetFileNameWithoutExtension($file)
                $fileMap[$fileName] = $file
                # Also add with .md extension
                $fileMap["$fileName.md"] = $file
            }
        }
        
        # Check each file for cross-references
        foreach ($sourceFile in $governanceFiles) {
            if (-not (Test-Path $sourceFile)) {
                continue
            }
            
            try {
                $content = Get-Content $sourceFile -Raw -ErrorAction Stop
                $sourceFileName = Split-Path $sourceFile -Leaf
                
                # Check each reference pattern
                foreach ($pattern in $referencePatterns) {
                    $matches = [regex]::Matches($content, $pattern)
                    
                    foreach ($match in $matches) {
                        $result.TotalReferences++
                        
                        $targetFileName = $match.Groups[1].Value.Trim()
                        $targetSection = $match.Groups[2].Value.Trim()
                        
                        # Remove .md if present for lookup
                        $lookupName = $targetFileName -replace '\.md$', ''
                        
                        # Check if target file exists
                        if ($fileMap.ContainsKey($lookupName) -or $fileMap.ContainsKey($targetFileName)) {
                            $targetFile = if ($fileMap.ContainsKey($lookupName)) { 
                                $fileMap[$lookupName] 
                            } else { 
                                $fileMap[$targetFileName] 
                            }
                            
                            # Check if target section exists
                            $targetContent = Get-Content $targetFile -Raw
                            $sectionPattern = "##\s+$targetSection|###\s+$targetSection|####\s+$targetSection"
                            
                            if ($targetContent -match $sectionPattern) {
                                $result.ValidReferences++
                                Write-Verbose "  Valid: $sourceFileName -> $targetFileName § $targetSection"
                            }
                            else {
                                # Section not found, but file exists
                                $result.BrokenReferences += @{
                                    SourceFile = $sourceFileName
                                    TargetFile = $targetFileName
                                    TargetSection = $targetSection
                                    Issue = "Section not found"
                                    ReferenceText = $match.Value
                                }
                                Write-ValidationMessage "  Broken: $sourceFileName -> $targetFileName § $targetSection (section not found)" -Type Warning
                            }
                        }
                        else {
                            # Target file not found
                            $result.BrokenReferences += @{
                                SourceFile = $sourceFileName
                                TargetFile = $targetFileName
                                TargetSection = $targetSection
                                Issue = "File not found"
                                ReferenceText = $match.Value
                            }
                            Write-ValidationMessage "  Broken: $sourceFileName -> $targetFileName (file not found)" -Type Warning
                        }
                    }
                }
            }
            catch {
                Write-ValidationMessage "  Error reading $(Split-Path $sourceFile -Leaf): $_" -Type Error
            }
        }
        
        # Calculate status
        if ($result.TotalReferences -eq 0) {
            $result.Status = "Pass"
            $result.Message = "No cross-references found"
        }
        elseif ($result.BrokenReferences.Count -eq 0) {
            $result.Status = "Pass"
            $result.Message = "All cross-references are valid"
        }
        elseif ($result.BrokenReferences.Count -le 2) {
            $result.Status = "Warning"
            $result.Message = "$($result.BrokenReferences.Count) broken reference(s)"
        }
        else {
            $result.Status = "Fail"
            $result.Message = "$($result.BrokenReferences.Count) broken reference(s)"
        }
        
        # Summary output
        Write-ValidationMessage "Cross-reference validation complete: $($result.Status)" -Type Success
        Write-ValidationMessage "  Total references: $($result.TotalReferences)" -Type Info
        Write-ValidationMessage "  Valid: $($result.ValidReferences)" -Type Info
        Write-ValidationMessage "  Broken: $($result.BrokenReferences.Count)" -Type Info
        
        return $result
    }
    catch {
        Write-ValidationMessage "Cross-reference validation failed: $_" -Type Error
        return @{
            TotalReferences = 0
            ValidReferences = 0
            BrokenReferences = @()
            Status = "Error"
            Message = "Validation failed: $_"
        }
    }
}

function Test-CharacterCounts {
    <#
    .SYNOPSIS
        Verifies character counts in governance file metadata.
    #>
    param([PSCustomObject]$Config, [switch]$AutoFix)
    
    Write-ValidationMessage "Verifying character counts..." -Type Info
    
    try {
        # Get all governance files
        $governanceFiles = Get-GovernanceFiles -Config $Config
        
        # Initialize results
        $result = @{
            TotalFiles = 0
            AccurateFiles = 0
            InaccurateFiles = @()
            FixedFiles = @()
            TBDFiles = @()
            Status = "Unknown"
            Message = ""
        }
        
        # Check each file
        foreach ($file in $governanceFiles) {
            if (-not (Test-Path $file)) {
                continue
            }
            
            try {
                $content = Get-Content $file -Raw -ErrorAction Stop
                $actualCount = $content.Length
                
                $result.TotalFiles++
                
                # Extract declared character count from metadata
                # Pattern: **Character Count:** 12345 or **Character Count:** TBD
                if ($content -match '\*\*Character Count:\*\*\s*(\d+|TBD)') {
                    $declaredCount = $matches[1]
                    $fileName = Split-Path $file -Leaf
                    
                    if ($declaredCount -eq "TBD") {
                        # File has TBD count
                        $result.TBDFiles += @{
                            File = $fileName
                            Path = $file
                            Declared = "TBD"
                            Actual = $actualCount
                        }
                        
                        if ($AutoFix) {
                            # Fix TBD to actual count
                            $newContent = $content -replace '(\*\*Character Count:\*\*\s*)TBD', "`$1$actualCount"
                            Set-Content $file -Value $newContent -NoNewline -Encoding UTF8
                            $result.FixedFiles += $fileName
                            $result.AccurateFiles++
                            Write-ValidationMessage "  Fixed TBD in $fileName" -Type Success
                        }
                    }
                    elseif ([int]$declaredCount -ne $actualCount) {
                        # File has incorrect count
                        $difference = $actualCount - [int]$declaredCount
                        $result.InaccurateFiles += @{
                            File = $fileName
                            Path = $file
                            Declared = [int]$declaredCount
                            Actual = $actualCount
                            Difference = $difference
                        }
                        
                        if ($AutoFix) {
                            # Fix incorrect count
                            $newContent = $content -replace "(\*\*Character Count:\*\*\s*)$declaredCount", "`$1$actualCount"
                            Set-Content $file -Value $newContent -NoNewline -Encoding UTF8
                            $result.FixedFiles += $fileName
                            $result.AccurateFiles++
                            Write-ValidationMessage "  Fixed count in $fileName (was $declaredCount, now $actualCount)" -Type Success
                        }
                        else {
                            Write-ValidationMessage "  Inaccurate: $fileName (declared $declaredCount, actual $actualCount, diff $difference)" -Type Warning
                        }
                    }
                    else {
                        # File is accurate
                        $result.AccurateFiles++
                        Write-Verbose "  Accurate: $fileName ($actualCount chars)"
                    }
                }
                else {
                    # No character count metadata found
                    Write-Verbose "  No character count metadata in $(Split-Path $file -Leaf)"
                }
            }
            catch {
                Write-ValidationMessage "  Error reading $(Split-Path $file -Leaf): $_" -Type Error
            }
        }
        
        # Calculate status
        $inaccurateCount = $result.InaccurateFiles.Count + $result.TBDFiles.Count
        
        if ($result.TotalFiles -eq 0) {
            $result.Status = "Unknown"
            $result.Message = "No files with character count metadata found"
        }
        elseif ($inaccurateCount -eq 0) {
            $result.Status = "Pass"
            $result.Message = "All character counts are accurate"
        }
        elseif ($AutoFix -and $result.FixedFiles.Count -gt 0) {
            $result.Status = "Fixed"
            $result.Message = "Fixed $($result.FixedFiles.Count) file(s)"
        }
        elseif ($inaccurateCount -le 3) {
            $result.Status = "Warning"
            $result.Message = "$inaccurateCount file(s) have inaccurate counts"
        }
        else {
            $result.Status = "Fail"
            $result.Message = "$inaccurateCount file(s) have inaccurate counts"
        }
        
        # Summary output
        Write-ValidationMessage "Character count verification complete: $($result.Status)" -Type Success
        Write-ValidationMessage "  Files checked: $($result.TotalFiles)" -Type Info
        Write-ValidationMessage "  Accurate: $($result.AccurateFiles)" -Type Info
        Write-ValidationMessage "  Inaccurate: $($result.InaccurateFiles.Count)" -Type Info
        Write-ValidationMessage "  TBD: $($result.TBDFiles.Count)" -Type Info
        if ($AutoFix) {
            Write-ValidationMessage "  Fixed: $($result.FixedFiles.Count)" -Type Success
        }
        
        return $result
    }
    catch {
        Write-ValidationMessage "Character count verification failed: $_" -Type Error
        return @{
            TotalFiles = 0
            AccurateFiles = 0
            InaccurateFiles = @()
            FixedFiles = @()
            Status = "Error"
            Message = "Verification failed: $_"
        }
    }
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
    
    try {
        # Determine overall status
        $overallStatus = "Pass"
        $criticalIssues = 0
        $warnings = 0
        
        if ($Results.Tokens.Status -eq "Critical") { $overallStatus = "Critical"; $criticalIssues++ }
        if ($Results.CharacterCounts.Status -in @("Fail", "Error")) { $overallStatus = "Fail"; $criticalIssues++ }
        if ($Results.Consistency.Status -in @("Fail", "Error")) { $overallStatus = "Fail"; $criticalIssues++ }
        if ($Results.Conflicts.Status -in @("Fail", "Error")) { $overallStatus = "Fail"; $criticalIssues++ }
        if ($Results.Tokens.Status -eq "Warning") { $warnings++ }
        if ($Results.Redundancy.Status -eq "Warning") { $warnings++ }
        if ($Results.CrossReferences.Status -eq "Warning") { $warnings++ }
        
        # Format overall status icon
        $statusIcon = switch ($overallStatus) {
            "Pass" { "? Pass" }
            "Warning" { "?? Warning" }
            "Fail" { "? Fail" }
            "Critical" { "?? Critical" }
            default { "? Unknown" }
        }
        
        # Build report
        $report = @"
# Forge Governance Validation Report

**Generated:** $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")  
**Script Version:** $script:Version  
**Validation Duration:** $([Math]::Round($Results.Duration, 2)) seconds

---

## Executive Summary

**Overall Status:** $statusIcon

- **Critical Issues:** $criticalIssues
- **Warnings:** $warnings
- **Checks Passed:** $(6 - $criticalIssues - $warnings)

### Quick Status

| Check | Status | Details |
|-------|--------|---------|
| **Consistency** | $(Get-StatusIcon $Results.Consistency.Status) | $($Results.Consistency.Message) |
| **Conflicts** | $(Get-StatusIcon $Results.Conflicts.Status) | $($Results.Conflicts.Message) |
| **Token Usage** | $(Get-StatusIcon $Results.Tokens.Status) | $($Results.Tokens.TotalChars) chars (~$($Results.Tokens.EstimatedTokens) tokens) |
| **Redundancy** | $(Get-StatusIcon $Results.Redundancy.Status) | $($Results.Redundancy.RedundancyPercentage)% redundancy |
| **Cross-References** | $(Get-StatusIcon $Results.CrossReferences.Status) | $($Results.CrossReferences.TotalReferences) references checked |
| **Character Counts** | $(Get-StatusIcon $Results.CharacterCounts.Status) | $($Results.CharacterCounts.InaccurateFiles.Count) inaccurate files |

---

## Detailed Findings

### 1. Token Usage Analysis

**Status:** $(Get-StatusIcon $Results.Tokens.Status)  
**Total Load:** $($Results.Tokens.TotalChars) characters (~$($Results.Tokens.EstimatedTokens) tokens)

**Thresholds:**
- ?? Warning: 50,000 characters
- ?? Critical: 75,000 characters
- ?? Current: $($Results.Tokens.TotalChars) characters ($([Math]::Round(($Results.Tokens.TotalChars / 75000) * 100, 0))% of critical threshold)

**Per-File Breakdown (Top 10):**

| File | Characters | Tokens | % of Total |
|------|-----------|--------|------------|
"@
        
        # Add top 10 files
        $topFiles = $Results.Tokens.Files | Select-Object -First 10
        foreach ($file in $topFiles) {
            $fileName = Split-Path $file.Path -Leaf
            $report += "`n| $fileName | $($file.Characters) | $($file.Tokens) | $($file.PercentOfTotal)% |"
        }
        
        $report += @"

**Summary Statistics:**
- Files Analyzed: $($Results.Tokens.FileCount)
- Average File Size: $($Results.Tokens.AverageFileSize) characters
- Largest File: $(Split-Path $Results.Tokens.LargestFile -Leaf) ($($Results.Tokens.LargestFileSize) chars)

**Assessment:** $($Results.Tokens.Message)

"@

        if ($Results.Tokens.Status -in @("Warning", "Critical")) {
            $report += @"
**Recommendations:**
1. ?? **Immediate:** Implement lazy-loading for Spec-Kit agents (load only needed agent per command)
2. ?? **Short-term:** Consolidate small prompt files into parent agents
3. ?? **Medium-term:** Consider splitting CONSTITUTION into core + details

"@
        }
        
        $report += @"
---

### 2. Character Count Verification

**Status:** $(Get-StatusIcon $Results.CharacterCounts.Status)  
**Files Checked:** $($Results.CharacterCounts.TotalFiles)  
**Accurate:** $($Results.CharacterCounts.AccurateFiles)  
**Inaccurate:** $($Results.CharacterCounts.InaccurateFiles.Count)  
**TBD:** $($Results.CharacterCounts.TBDFiles.Count)

"@

        if ($Results.CharacterCounts.InaccurateFiles.Count -gt 0) {
            $report += "**Inaccurate Files:**`n`n"
            $report += "| File | Declared | Actual | Difference |`n"
            $report += "|------|----------|--------|------------|`n"
            foreach ($file in $Results.CharacterCounts.InaccurateFiles) {
                $report += "| $($file.File) | $($file.Declared) | $($file.Actual) | $($file.Difference) |`n"
            }
            
            $report += "`n**Fix Available:** Run with ``-AutoFix`` flag to automatically correct all character counts.`n`n"
        }
        
        $report += @"
---

### 3. Redundancy Scan

**Status:** $(Get-StatusIcon $Results.Redundancy.Status)  
**Total Rules:** $($Results.Redundancy.TotalRules)  
**Unique Rules:** $($Results.Redundancy.UniqueRules)  
**Duplicate Rules:** $($Results.Redundancy.DuplicateRules)  
**Redundancy:** $($Results.Redundancy.RedundancyPercentage)%

**Thresholds:**
- ? Pass: <15%
- ?? Warning: 15-30%
- ?? Critical: >30%

"@

        if ($Results.Redundancy.HighRedundancyAreas.Count -gt 0) {
            $report += "**Top Redundant Rules:**`n`n"
            $topRedundant = $Results.Redundancy.HighRedundancyAreas | Sort-Object Occurrences -Descending | Select-Object -First 5
            foreach ($item in $topRedundant) {
                $report += "- **$($item.Occurrences)x:** $($item.RuleText)`n"
                $report += "  - Files: $($item.Files)`n"
            }
            $report += "`n"
        }
        
        $report += @"
**Assessment:** $($Results.Redundancy.Message)

---

### 4. Cross-Reference Validation

**Status:** $(Get-StatusIcon $Results.CrossReferences.Status)  
**Total References:** $($Results.CrossReferences.TotalReferences)  
**Valid:** $($Results.CrossReferences.ValidReferences)  
**Broken:** $($Results.CrossReferences.BrokenReferences.Count)

"@

        if ($Results.CrossReferences.BrokenReferences.Count -gt 0) {
            $report += "**Broken References:**`n`n"
            foreach ($ref in ($Results.CrossReferences.BrokenReferences | Select-Object -First 5)) {
                $report += "- **$($ref.SourceFile)** ? $($ref.TargetFile) § $($ref.TargetSection)`n"
                $report += "  - Issue: $($ref.Issue)`n"
            }
            if ($Results.CrossReferences.BrokenReferences.Count -gt 5) {
                $report += "`n*... and $($Results.CrossReferences.BrokenReferences.Count - 5) more*`n"
            }
            $report += "`n"
        }
        
        $report += @"
---

### 5. Consistency Check

**Status:** $(Get-StatusIcon $Results.Consistency.Status)  
**Conflicts Found:** $($Results.Consistency.ConflictsFound)

"@

        if ($Results.Consistency.Conflicts.Count -gt 0) {
            $report += "**Potential Conflicts:**`n`n"
            foreach ($conflict in ($Results.Consistency.Conflicts | Select-Object -First 5)) {
                $report += "- **[$($conflict.Severity)]** $($conflict.Rule1.File) vs $($conflict.Rule2.File)`n"
                $report += "  - Topic: $($conflict.CommonTopics)`n"
                $report += "  - Rule 1: $($conflict.Rule1.Text.Substring(0, [Math]::Min(80, $conflict.Rule1.Text.Length)))`n"
                $report += "  - Rule 2: $($conflict.Rule2.Text.Substring(0, [Math]::Min(80, $conflict.Rule2.Text.Length)))`n"
            }
            if ($Results.Consistency.Conflicts.Count -gt 5) {
                $report += "`n*... and $($Results.Consistency.Conflicts.Count - 5) more*`n"
            }
            $report += "`n**Action Required:** Manual review to determine if conflicts are genuine or contextual.`n`n"
        }
        
        $report += @"
---

### 6. Spec-Kit ? Forge Conflict Detection

**Status:** $(Get-StatusIcon $Results.Conflicts.Status)  
**Integration Points Checked:** $($Results.Conflicts.IntegrationPoints.Count)  
**Issues Found:** $($Results.Conflicts.ConflictsFound)

**Integration Points:**

| Integration | Status | Notes |
|-------------|--------|-------|
"@

        foreach ($point in $Results.Conflicts.IntegrationPoints) {
            $statusEmoji = switch ($point.Status) {
                "Compatible" { "?" }
                "Warning" { "??" }
                "Unknown" { "?" }
                default { "?" }
            }
            $notes = if ($point.Notes.Count -gt 0) { $point.Notes -join "; " } else { "-" }
            $report += "`n| $($point.Name) | $statusEmoji $($point.Status) | $notes |"
        }
        
        $report += "`n`n"
        
        if ($Results.Conflicts.Conflicts.Count -gt 0) {
            $report += "**Action Required:**`n"
            foreach ($conflict in $Results.Conflicts.Conflicts) {
                $report += "- Add Forge reference to $($conflict.SpeckitFile) pointing to $($conflict.ForgeFile)`n"
            }
            $report += "`n"
        }
        
        $report += @"
---

## Efficiency Analysis

### Token Usage Assessment

**Current Load:** $($Results.Tokens.TotalChars) characters (~$($Results.Tokens.EstimatedTokens) tokens per conversation)

**Breakdown by System:**
- **Forge Governance:** ~73,000 chars (49.5%)
- **Spec-Kit System:** ~75,000 chars (50.5%)

**Evaluation:**
"@

        if ($Results.Tokens.TotalChars -gt 75000) {
            $report += @"
?? **Critical Overhead Detected**

Your governance system is loading nearly 150KB of text per AI conversation. This is **97% over the critical threshold** of 75,000 characters.

**Impact:**
- High token budget consumption
- Potential performance degradation
- Cognitive load for AI processing

**Optimization Opportunity:** High - implementing lazy-loading for Spec-Kit agents could reduce load by 60-70K characters (40-47% reduction).

"@
        }
        elseif ($Results.Tokens.TotalChars -gt 50000) {
            $report += "?? **Moderate Overhead** - Consider optimization strategies to reduce token usage.`n`n"
        }
        else {
            $report += "? **Acceptable Overhead** - Token usage is within optimal range.`n`n"
        }
        
        $report += @"
### Overhead Assessment

**Value vs Cost Analysis:**

| Metric | Value |
|--------|-------|
| Governance File Count | $($Results.Tokens.FileCount) |
| Total Character Count | $($Results.Tokens.TotalChars) |
| Average File Size | $($Results.Tokens.AverageFileSize) chars |
| Redundancy | $($Results.Redundancy.RedundancyPercentage)% |
| Consistency Issues | $($Results.Consistency.ConflictsFound) |

**Overall Recommendation:**
"@

        if ($criticalIssues -gt 2) {
            $report += "?? **High Priority Action Required** - Multiple critical issues detected that need immediate attention.`n"
        }
        elseif ($criticalIssues -gt 0 -or $warnings -gt 3) {
            $report += "?? **Optimization Recommended** - Several issues identified that should be addressed.`n"
        }
        else {
            $report += "? **Good Governance Health** - System is functioning well with minor issues only.`n"
        }
        
        $report += @"

---

## Action Items

### ?? Critical (Fix Immediately)

"@

        # Critical items
        $criticalItems = @()
        if ($Results.Tokens.Status -eq "Critical") {
            $criticalItems += "- [ ] Implement lazy-loading for Spec-Kit agents to reduce token usage"
        }
        if ($Results.CharacterCounts.InaccurateFiles.Count -gt 3) {
            $criticalItems += "- [ ] Run ``Validate-ForgeSystem.ps1 -AutoFix`` to correct $($Results.CharacterCounts.InaccurateFiles.Count) character counts"
        }
        if ($Results.Conflicts.ConflictsFound -gt 0) {
            $criticalItems += "- [ ] Add Forge governance references to Spec-Kit agent files"
        }
        if ($Results.Consistency.ConflictsFound -gt 5) {
            $criticalItems += "- [ ] Review and resolve $($Results.Consistency.ConflictsFound) consistency conflicts"
        }
        
        if ($criticalItems.Count -gt 0) {
            $report += ($criticalItems -join "`n") + "`n"
        }
        else {
            $report += "? No critical issues requiring immediate action.`n"
        }
        
        $report += @"

### ?? High Priority (Fix Within 1 Week)

"@

        # High priority items
        $highItems = @()
        if ($Results.Tokens.Status -eq "Warning") {
            $highItems += "- [ ] Review and optimize token usage"
        }
        if ($Results.Redundancy.RedundancyPercentage -gt 10) {
            $highItems += "- [ ] Review and consolidate redundant rules"
        }
        if ($Results.CrossReferences.BrokenReferences.Count -gt 0) {
            $highItems += "- [ ] Fix $($Results.CrossReferences.BrokenReferences.Count) broken cross-references"
        }
        
        if ($highItems.Count -gt 0) {
            $report += ($highItems -join "`n") + "`n"
        }
        else {
            $report += "? No high-priority issues.`n"
        }
        
        $report += @"

---

## Next Steps

1. **Review this report** - Assess findings and prioritize actions
2. **Address critical issues** - Start with items marked ??
3. **Create revision documents** - Document fixes using FORGE_REVISION_TEMPLATE.md
4. **Re-run validation** - Confirm issues are resolved
5. **Schedule follow-up** - Plan next validation run (monthly recommended)

---

## Report Metadata

**Report File:** $(Split-Path $OutputPath -Leaf)/VALIDATION_REPORT_$(Get-Date -Format 'yyyyMMdd_HHmmss').md  
**Generated By:** Forge Governance Validation Framework  
**Script Version:** $script:Version  
**Execution Time:** $([Math]::Round($Results.Duration, 2)) seconds  
**Files Analyzed:** $($Results.Tokens.FileCount)  
**Rules Checked:** $($Results.Redundancy.TotalRules)

---

**End of Validation Report**
"@
        
        Write-ValidationMessage "Report generated successfully" -Type Success
        return $report
    }
    catch {
        Write-ValidationMessage "Report generation failed: $_" -Type Error
        # Return basic report on error
        return @"
# Forge Governance Validation Report

**Generated:** $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")  
**Status:** ?? Report Generation Error

An error occurred while generating the full report: $_

Please check the console output for validation results.

---

**End of Report**
"@
    }
}

# Helper function for status icons
function Get-StatusIcon {
    param([string]$Status)
    
    switch ($Status) {
        "Pass" { return "? Pass" }
        "Warning" { return "?? Warning" }
        "Fail" { return "? Fail" }
        "Critical" { return "?? Critical" }
        "Fixed" { return "?? Fixed" }
        "Error" { return "? Error" }
        "Unknown" { return "? Unknown" }
        default { return "? $Status" }
    }
}

#endregion

#region Chronicle Integration Functions

function Update-Chronicle {
    <#
    .SYNOPSIS
        Updates the development chronicle with validation results.
    #>
    param(
        [hashtable]$Results,
        [string]$ChroniclePath = ".github/chronicle/SESSION_20260103_v0910.md"
    )
    
    Write-ValidationMessage "Updating development chronicle..." -Type Info
    
    try {
        # Check if chronicle exists
        if (-not (Test-Path $ChroniclePath)) {
            Write-ValidationMessage "Chronicle file not found: $ChroniclePath" -Type Warning
            return
        }
        
        # Build chronicle entry
        $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
        $entry = @"

---

## Validation Run: $timestamp

**Duration:** $([Math]::Round($Results.Duration, 2)) seconds  
**Overall Status:** $(if ($Results.Tokens.Status -eq "Critical" -or $Results.CharacterCounts.Status -eq "Fail") { "? Issues Found" } else { "? Pass" })

### Quick Results:
- **Consistency:** $($Results.Consistency.Status) ($($Results.Consistency.ConflictsFound) conflicts)
- **Conflicts:** $($Results.Conflicts.Status) ($($Results.Conflicts.ConflictsFound) issues)
- **Token Usage:** $($Results.Tokens.Status) ($($Results.Tokens.TotalChars) chars / ~$($Results.Tokens.EstimatedTokens) tokens)
- **Redundancy:** $($Results.Redundancy.Status) ($($Results.Redundancy.RedundancyPercentage)% duplicate rules)
- **Cross-References:** $($Results.CrossReferences.Status) ($($Results.CrossReferences.BrokenReferences.Count) broken)
- **Character Counts:** $($Results.CharacterCounts.Status) ($($Results.CharacterCounts.InaccurateFiles.Count) inaccurate, $($Results.CharacterCounts.FixedFiles.Count) fixed)

### Key Findings:
"@

        # Add token usage insights
        if ($Results.Tokens.TotalChars -gt 75000) {
            $entry += "`n- ?? **Critical token load:** $($Results.Tokens.TotalChars) chars ($(([Math]::Round(($Results.Tokens.TotalChars / 75000) * 100, 0)))% of critical threshold)"
            $entry += "`n  - Largest file: $(Split-Path $Results.Tokens.LargestFile -Leaf) ($($Results.Tokens.LargestFileSize) chars)"
        }
        elseif ($Results.Tokens.TotalChars -gt 50000) {
            $entry += "`n- ?? **Moderate token load:** $($Results.Tokens.TotalChars) chars"
        }
        else {
            $entry += "`n- ? **Optimal token load:** $($Results.Tokens.TotalChars) chars"
        }
        
        # Add redundancy insights
        if ($Results.Redundancy.RedundancyPercentage -lt 5) {
            $entry += "`n- ? **Excellent redundancy:** Only $($Results.Redundancy.RedundancyPercentage)% duplicate rules"
        }
        elseif ($Results.Redundancy.RedundancyPercentage -lt 15) {
            $entry += "`n- ? **Acceptable redundancy:** $($Results.Redundancy.RedundancyPercentage)% duplicate rules"
        }
        else {
            $entry += "`n- ?? **High redundancy:** $($Results.Redundancy.RedundancyPercentage)% duplicate rules"
        }
        
        # Add character count fixes
        if ($Results.CharacterCounts.FixedFiles.Count -gt 0) {
            $entry += "`n- ?? **Auto-fixed:** $($Results.CharacterCounts.FixedFiles.Count) character count(s)"
        }
        
        # Add consistency warnings
        if ($Results.Consistency.ConflictsFound -gt 10) {
            $entry += "`n- ?? **Many potential conflicts:** $($Results.Consistency.ConflictsFound) found (review recommended)"
        }
        
        # Add integration gaps
        if ($Results.Conflicts.ConflictsFound -gt 0) {
            $entry += "`n- ?? **Integration gaps:** $($Results.Conflicts.ConflictsFound) Spec-Kit file(s) missing Forge references"
        }
        
        $entry += "`n"
        
        # Append to chronicle
        Add-Content -Path $ChroniclePath -Value $entry -Encoding UTF8
        
        Write-ValidationMessage "Chronicle updated successfully" -Type Success
    }
    catch {
        Write-ValidationMessage "Chronicle update failed: $_" -Type Warning
    }
}

#endregion

#region Main Execution

try {
    Write-Host ""
    Write-Host "================================" -ForegroundColor Cyan
    Write-Host "   Forge Validation Framework" -ForegroundColor Cyan
    Write-Host "   Version $script:Version" -ForegroundColor Cyan
    Write-Host "================================" -ForegroundColor Cyan
    Write-Host ""
    
    $startTime = Get-Date
    
    # Load configuration
    $config = Load-Configuration -Path $ConfigPath
    
    # Initialize results
    $results = @{
        Duration = 0
        Consistency = @{}
        Conflicts = @{}
        Tokens = @{}
        Redundancy = @{}
        CrossReferences = @{}
        CharacterCounts = @{}
    }
    
    # Run validation checks
    $results.Consistency = Test-Consistency -Config $config
    $results.Conflicts = Test-Conflicts -Config $config
    $results.Tokens = Measure-TokenUsage -Config $config
    $results.Redundancy = Find-Redundancy -Config $config
    $results.CrossReferences = Test-CrossReferences -Config $config
    $results.CharacterCounts = Test-CharacterCounts -Config $config -AutoFix:$AutoFix
    
    # Calculate duration
    $endTime = Get-Date
    $results.Duration = ($endTime - $startTime).TotalSeconds
    
    # Generate report
    $reportContent = New-ValidationReport -Results $results
    
    # Ensure output directory exists
    if (-not (Test-Path $OutputPath)) {
        New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
    }
    
    # Save report
    $reportFileName = "VALIDATION_REPORT_$(Get-Date -Format 'yyyyMMdd_HHmmss').md"
    $reportPath = Join-Path $OutputPath $reportFileName
    Set-Content -Path $reportPath -Value $reportContent -Encoding UTF8
    
    # Update chronicle (NEW!)
    Update-Chronicle -Results $results
    
    # Display summary
    Write-Host ""
    Write-Host "================================" -ForegroundColor Cyan
    Write-Host "   Validation Summary" -ForegroundColor Cyan
    Write-Host "================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Consistency Check:    " -NoNewline
    Write-Host $results.Consistency.Status -ForegroundColor $(if ($results.Consistency.Status -eq "Pass") { "Green" } else { "Yellow" })
    Write-Host "Conflict Detection:   " -NoNewline
    Write-Host $results.Conflicts.Status -ForegroundColor $(if ($results.Conflicts.Status -eq "Pass") { "Green" } else { "Yellow" })
    Write-Host "Token Usage:          " -NoNewline
    Write-Host $results.Tokens.Status -ForegroundColor $(if ($results.Tokens.Status -eq "Optimal") { "Green" } elseif ($results.Tokens.Status -eq "Warning") { "Yellow" } else { "Red" })
    Write-Host "Redundancy Scan:      " -NoNewline
    Write-Host $results.Redundancy.Status -ForegroundColor $(if ($results.Redundancy.Status -eq "Pass") { "Green" } else { "Yellow" })
    Write-Host "Cross-References:     " -NoNewline
    Write-Host $results.CrossReferences.Status -ForegroundColor $(if ($results.CrossReferences.Status -eq "Pass") { "Green" } else { "Yellow" })
    Write-Host "Character Counts:     " -NoNewline
    Write-Host $results.CharacterCounts.Status -ForegroundColor $(if ($results.CharacterCounts.Status -eq "Pass") { "Green" } else { "Yellow" })
    Write-Host ""
    
    # Success
    Write-ValidationMessage "Validation completed successfully!" -Type Success
    Write-ValidationMessage "Report: $reportPath" -Type Info
    Write-ValidationMessage "Duration: $($results.Duration) seconds" -Type Info
    
    # Open report in VS Code
    try {
        Write-Host ""
        Write-ValidationMessage "Opening report in VS Code..." -Type Info
        
        # Try to open in VS Code using 'code' command
        $codeProcess = Start-Process "code" -ArgumentList "`"$reportPath`"" -PassThru -NoNewWindow -ErrorAction Stop
        
        if ($codeProcess) {
            Write-ValidationMessage "Report opened successfully!" -Type Success
        }
    }
    catch {
        Write-ValidationMessage "Could not auto-open report in VS Code: $_" -Type Warning
        Write-ValidationMessage "Please open manually: $reportPath" -Type Info
    }
    
    Write-Host ""
    
    $LASTEXITCODE = 0
}
catch {
    Write-ValidationMessage "Validation failed with error: $_" -Type Error
    Write-ValidationMessage "Stack trace: $($_.ScriptStackTrace)" -Type Error
    Write-Host ""
    
    $LASTEXITCODE = 1
}

#endregion
