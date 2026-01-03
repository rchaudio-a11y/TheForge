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
                    
                    # Store file data
                    $fileInfo = @{
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
        
        # Sort files by size (largest first)
        $result.Files = $result.Files | Sort-Object Characters -Descending
        
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
        
        # Add summary statistics
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
    
    # Return success (script will exit naturally)
    $LASTEXITCODE = 0
}
catch {
    Write-ValidationMessage "Validation failed: $_" -Type Error
    Write-Host $_.ScriptStackTrace -ForegroundColor Red
    
    # Return failure (script will exit naturally)
    $LASTEXITCODE = 1
}

#endregion
