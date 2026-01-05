# File Structure Guide

**Document Type:** Reference Documentation  
**Purpose:** Complete guide to all files in TheForge workspace with descriptions and usage  
**Created:** 2025-01-04  
**Last Updated:** 2025-01-04  
**Status:** Active  
**Character Count:** 29231  
**Related:** Workspace-Summary.md, ForgeCharter.md, SPECKIT_SETUP.md

---

## Overview

This guide provides a complete reference to all governance files, Spec-Kit files, and their usage within the TheForge ecosystem. Use this as your navigation map for understanding when and how to use each file.

---

## ?? Complete File Structure

```
C:\Users\rchau\source\repos\TheForge\
?
??? ?? SPECKIT_SETUP.md                          [Setup Guide]
??? ?? README.md                                 [Workspace Overview]
?
??? TheForge\                                     [Core Project]
?   ??? Prompts\                                 [Governance Files]
?   ?   ??? ?? ForgeCharter.md                   [MASTER - Governance Rules]
?   ?   ??? ?? Branch-Architecture.md            [Architecture Rules]
?   ?   ??? ?? Branch-Coding.md                  [Coding Rules]
?   ?   ??? ?? Branch-Documentation.md           [Documentation Rules]
?   ?   ??? ?? ForgeAudit.md                     [Compliance Checker]
?   ?   ??? ?? TagRegistry.md                    [Tag Definitions]
?   ?   ??? Tasks\
?   ?   ?   ??? ?? IssueSummary.md               [Issue Pattern Instructions]
?   ?   ?   ??? ?? Improvments_In_RAG.md         [RAG Improvements]
?   ?   ??? Skills\                              [Agent Personalities]
?   ?   ?   ??? ?? Claude Opus 4.5 Prompt Pack.md
?   ?   ?   ??? ?? GPT-5.1-Codex-Max Prompt Pack.md
?   ?   ?   ??? ?? Free - Claude Haiku 4.5.md
?   ?   ?   ??? ?? Free - GPT-4o.md
?   ?   ?   ??? ?? Free - GPT-5 Mini.md
?   ?   ?   ??? ?? Forge-Agents.md.md
?   ?   ?   ??? ?? file1.md
?   ?   ??? github\                              [GitHub Configuration]
?   ?   ?   ??? ?? copilot-instructions.md
?   ?   ?   ??? ?? CONSTITUTION.md
?   ?   ?   ??? ?? CONSTITUTION_QUICKREF.md
?   ?   ?   ??? ?? CONSTITUTION_SUMMARY.md
?   ?   ?   ??? ?? CONSTITUTION_VALIDATION.md
?   ?   ?   ??? ?? CONSTITUTION_ARCHITECTURE_ANALYSIS.md
?   ?   ?   ??? ?? CONSTITUTION_UPDATE_*.md      [5 files]
?   ?   ?   ??? ?? FORGE_ALIGNMENT_PLAN.md
?   ?   ?   ??? validation\
?   ?   ?       ??? ?? Validate-ForgeSystem.ps1
?   ?   ?       ??? ?? validation-config.json
?   ?   ?       ??? reports\                     [13+ validation reports]
?   ?   ?       ??? templates\
?   ?   ?           ??? ?? .gitkeep
?   ?   ?           ??? ?? FORGE_REVISION_TEMPLATE.md
?   ?   ?           ??? ?? VALIDATION_REPORT_TEMPLATE.md
?   ?   ??? specify\                             [Spec-Kit Files]
?   ?       ??? features\                        [Feature Specifications]
?   ?       ?   ??? add-readme-files\
?   ?       ?       ??? ?? spec.md
?   ?       ?       ??? ?? plan.md
?   ?       ?       ??? ?? tasks.md
?   ?       ??? memory\                          [Project Memory]
?   ?       ?   ??? ?? constitution.md
?   ?       ??? scripts\powershell\              [PowerShell Scripts]
?   ?       ?   ??? ?? check-prerequisites.ps1
?   ?       ?   ??? ?? common.ps1
?   ?       ?   ??? ?? create-new-feature.ps1
?   ?       ?   ??? ?? setup-plan.ps1
?   ?       ?   ??? ?? update-agent-context.ps1
?   ?       ??? templates\                       [Spec-Kit Templates]
?   ?           ??? ?? agent-file-template.md
?   ?           ??? ?? checklist-template.md
?   ?           ??? ?? plan-template.md
?   ?           ??? ?? spec-template.md
?   ?           ??? ?? tasks-template.md
?   ?
?   ??? Documentation\                           [Documentation Hub]
?       ??? Reference\                           [Quick Reference]
?       ?   ??? ?? README.md                     [Reference Hub Index]
?       ?   ??? ?? Workspace-Summary.md          [Workspace Guide]
?       ?   ??? ?? File-Structure-Guide.md       [This File]
?       ?   ??? ?? Unified-Workflow-Guide.md     [Workflow Documentation]
?       ?   ??? ?? Forge-Improvement-Suggestions-Overview.md
?       ?   ??? ?? Forge-Improvement-Suggestions-High.md
?       ?   ??? ?? Forge-Improvement-Suggestions-Medium.md
?       ?   ??? ?? Forge-Improvement-Suggestions-Low.md
?       ?   ??? ?? Forge-Improvement-Suggestions-Tracking.md
?       ??? Technical\                           [Technical Docs]
?       ?   ??? ?? SpecKit-Tutorial-AddREADMEs.md
?       ??? Chronicle\                           [Development History]
?       ?   ??? ?? ComplianceJourney-Log.md
?       ?   ??? DevelopmentLog\
?       ?       ??? ?? IssueSummary.md           [Actual Issue Database]
?       ??? Codex\                               [API Documentation]
?       ??? Grimoire\                            [Configuration Guides]
?       ??? Tomes\                               [Tutorials]
?
??? SampleForgeModule\                           [Example Module]
?   ??? ?? SampleForgeModule.vbproj
?
??? NewDatabaseGenerator\                        [External Submodule]
    ??? NewDatabaseGenerator\
    ?   ??? ?? NewDatabaseGenerator.vbproj
    ??? RCHAutomation.Controls\
        ??? ?? RCHAutomation.Controls.vbproj
```

**Note:** Hidden directories `.github/` and `.specify/` (workspace root) are backup copies. Working files are in `TheForge/Prompts/github/` and `TheForge/Prompts/specify/`.

---

## ??? Governance Files (Core System)

### ?? ForgeCharter.md
**Location:** `TheForge/Prompts/ForgeCharter.md`  
**Type:** Master Governance Document  
**Priority:** CRITICAL - Always Referenced

**Purpose:**  
The supreme authority for all Forge governance. Contains universal rules that apply to all projects, modules, and documentation.

**Key Sections:**
- Section 1-8: Core governance principles
- Section 9: Metadata headers (mandatory for all docs)
- Section 10: Naming conventions
- Section 11: File organization
- Section 12: Compliance enforcement

**When to Use:**
- ? ALWAYS - Every task must reference ForgeCharter
- ? Before starting any new feature
- ? When creating any new file
- ? When making architectural decisions
- ? When in doubt about any rule

**Used By:**
- All AI agents (via copilot-instructions.md)
- All branch files (as authority)
- ForgeAudit (for compliance checking)
- Developers (as reference)

---

### ?? Branch-Architecture.md
**Location:** `TheForge/Prompts/Branch-Architecture.md`  
**Type:** Specialized Governance - Structure Rules  
**Priority:** HIGH

**Purpose:**  
Defines project structure, folder organization, and module layout rules.

**Key Rules:**
- Module organization standards
- Folder naming conventions
- Project structure requirements
- Component placement rules

**When to Use:**
- ? Creating new modules
- ? Organizing project structure
- ? Defining folder hierarchies
- ? Planning multi-project solutions
- ? Refactoring project layout

**Used By:**
- copilot-instructions.md (routes structure tasks here)
- Spec-Kit plan workflow (for file organization)
- Developers (for structure decisions)

**Example Tasks:**
- Creating new project structure
- Adding new modules
- Reorganizing folders
- Defining component locations

---

### ?? Branch-Coding.md
**Location:** `TheForge/Prompts/Branch-Coding.md`  
**Type:** Specialized Governance - Code Rules  
**Priority:** HIGH

**Purpose:**  
Defines coding standards, naming conventions, and forbidden terms.

**Key Rules:**
- Class naming conventions
- Method naming standards
- Variable naming rules
- Forbidden terms (Helper, Manager, Utility, etc.)
- Code organization patterns

**When to Use:**
- ? Writing new code
- ? Creating classes/methods
- ? Naming variables/functions
- ? Code refactoring
- ? Code reviews

**Used By:**
- copilot-instructions.md (routes code tasks here)
- Spec-Kit implement workflow (for code generation)
- Developers (for coding standards)

**Example Tasks:**
- Creating new VB.NET classes
- Naming methods and properties
- Code generation
- Refactoring existing code

---

### ?? Branch-Documentation.md
**Location:** `TheForge/Prompts/Branch-Documentation.md`  
**Type:** Specialized Governance - Documentation Rules  
**Priority:** HIGH

**Purpose:**  
Defines documentation standards, taxonomy, and structure rules.

**Key Rules:**
- Documentation taxonomy (Codex, Grimoire, Tomes, etc.)
- Metadata header requirements
- Markdown formatting standards
- Cross-reference guidelines
- Documentation placement rules

**When to Use:**
- ? Creating any documentation
- ? Writing README files
- ? Creating tutorials
- ? Documenting APIs
- ? Writing technical guides

**Used By:**
- copilot-instructions.md (routes doc tasks here)
- Spec-Kit implement workflow (for documentation)
- ForgeAudit (for doc compliance)
- Developers (for writing docs)

**Example Tasks:**
- Creating README.md files
- Writing API documentation
- Creating tutorials
- Documenting configuration

---

### ?? ForgeAudit.md
**Location:** `TheForge/Prompts/ForgeAudit.md`  
**Type:** Compliance Verification System  
**Priority:** CRITICAL

**Purpose:**  
Automated compliance checker that validates files against ForgeCharter rules.

**Capabilities:**
- ? Check metadata headers (ForgeCharter Section 9)
- ? Auto-fix Character Count (TBD ? actual)
- ? Auto-update Last Updated dates
- ? Validate document structure
- ? Report violations with severity
- ? Calculate compliance score

**When to Use:**
- ? After creating any documentation
- ? Before committing changes
- ? During code reviews
- ? After implementing features
- ? Periodic compliance audits

**Command:**
```
Run ForgeAudit 2.0 on [filename]
```

**Used By:**
- Developers (pre-commit checks)
- CI/CD pipelines (automated checks)
- AI agents (validation step)

**Output:**
- Compliance report
- Auto-fixed files
- Violation details
- Compliance score (target: 100%)

---

### ?? TagRegistry.md
**Location:** `TheForge/Prompts/TagRegistry.md`  
**Type:** Reference Document  
**Priority:** MEDIUM

**Purpose:**  
Centralized registry of all tags used across the system.

**Contains:**
- Tag definitions
- Usage guidelines
- Tag categories
- Cross-references

**When to Use:**
- ? Looking up tag meanings
- ? Adding new tags
- ? Standardizing terminology
- ? Cross-referencing features

---

## ?? Task Files

### ?? IssueSummary.md
**Location:** `TheForge/Prompts/Tasks/IssueSummary.md`  
**Type:** Task Instructions  
**Priority:** MEDIUM

**Purpose:**  
Instructions for creating/updating the IssueSummary database in Chronicle/DevelopmentLog.

**Contains:**
- Task definition
- Requirements for summarizing issues
- Output format specifications
- Categorization guidelines

**When to Use:**
- ? When AI needs to generate/update IssueSummary
- ? Referenced by ForgeAudit for pattern detection

**Note:** Actual issue data is in `TheForge/Documentation/Chronicle/DevelopmentLog/IssueSummary.md`

---

### ?? Improvments_In_RAG.md
**Location:** `TheForge/Prompts/Tasks/Improvments_In_RAG.md`  
**Type:** Task Documentation  
**Priority:** LOW

**Purpose:**  
Documents improvements and enhancements for RAG (Retrieval-Augmented Generation) systems.

**When to Use:**
- ? Improving AI retrieval accuracy
- ? Enhancing context window usage
- ? Optimizing document chunking

**Note:** Filename has typo ("Improvments" should be "Improvements")

---

## ?? Agent Personality Files

### ?? Claude Opus 4.5 Prompt Pack.md
**Location:** `TheForge/Prompts/Skills/Claude Opus 4.5 Prompt Pack.md`  
**Type:** AI Agent Configuration  
**Priority:** MEDIUM

**Purpose:**  
Defines the "Lore Agent" personality and capabilities.

**Specialties:**
- Architectural analysis
- Documentation quality
- Naming canon enforcement
- Philosophical alignment
- Design trade-offs

**When to Use:**
- ? Architecture reviews
- ? Documentation planning
- ? Design decisions
- ? Naming discussions

---

### ?? GPT-5.1-Codex-Max Prompt Pack.md
**Location:** `TheForge/Prompts/Skills/GPT-5.1-Codex-Max Prompt Pack.md`  
**Type:** AI Agent Configuration  
**Priority:** MEDIUM

**Purpose:**  
Defines the "Codex Agent" personality and capabilities.

**Specialties:**
- Code generation
- .slnx editing
- Multi-project solutions
- Deterministic output
- Build validation

**When to Use:**
- ? Code generation
- ? Project file editing
- ? Solution structure
- ? Build system tasks

---

### ?? Free Agent Prompt Packs
**Location:** `TheForge/Prompts/Skills/`  
**Type:** AI Agent Configurations (Free Tier)  
**Priority:** LOW

**Files:**
- `Free - Claude Haiku 4.5.md` - Free tier Claude configuration
- `Free - GPT-4o.md` - Free tier GPT-4o configuration  
- `Free - GPT-5 Mini.md` - Free tier GPT-5 Mini configuration

**Purpose:**  
Alternative agent configurations for free-tier AI models with reduced capabilities.

**When to Use:**
- ? Testing with free-tier models
- ? Cost-conscious development
- ? Light governance tasks

---

### ?? Forge-Agents.md.md
**Location:** `TheForge/Prompts/Skills/Forge-Agents.md.md`  
**Type:** Agent Documentation  
**Priority:** LOW

**Purpose:**  
Documentation or configuration for Forge-specific agent behaviors.

**Note:** File has double .md extension (may need correction)

---

### ?? file1.md
**Location:** `TheForge/Prompts/Skills/file1.md`  
**Type:** Unknown/Placeholder  
**Priority:** LOW

**Purpose:**  
Placeholder or temporary file (may need review/removal)

---

## ?? GitHub Configuration (TheForge/Prompts/github/)

**Location:** `TheForge/Prompts/github/`  
**Type:** GitHub and Validation Configuration  
**Priority:** HIGH

**Purpose:**  
Active GitHub configuration files including copilot routing, constitution documentation, and validation system.

---

### ?? copilot-instructions.md
**Location:** `TheForge/Prompts/github/copilot-instructions.md`  
**Type:** Router Configuration  
**Priority:** CRITICAL

**Purpose:**  
Routes all AI tasks to correct Forge branch files and enables governance enforcement.

**Responsibilities:**
- Load ForgeCharter.md
- Enable Spec-Kit workflow
- Identify task type
- Delegate to correct branch
- Never interpret or override rules

**Routes To:**
- Branch-Coding.md (code tasks)
- Branch-Architecture.md (structure tasks)
- Branch-Documentation.md (documentation tasks)
- ForgeAudit.md (compliance checking)
- Spec-Kit workflows (feature commands)

**When to Use:**
- ? Automatically loaded by GitHub Copilot
- ? Referenced by all AI agents

---

### ?? CONSTITUTION.md and Related Files
**Location:** `TheForge/Prompts/github/`  
**Type:** Constitution Documentation Suite  
**Priority:** HIGH

**Files:**
- `CONSTITUTION.md` - Main constitution document
- `CONSTITUTION_QUICKREF.md` - Quick reference guide
- `CONSTITUTION_SUMMARY.md` - Executive summary
- `CONSTITUTION_VALIDATION.md` - Validation report
- `CONSTITUTION_ARCHITECTURE_ANALYSIS.md` - Architecture analysis
- `CONSTITUTION_UPDATE_*.md` (5 files) - Update history

**Purpose:**  
Project constitution and supporting documentation for governance alignment.

**When to Use:**
- ? Understanding project principles
- ? Governance decision-making
- ? Architecture planning
- ? Validation reporting

---

### ?? FORGE_ALIGNMENT_PLAN.md
**Location:** `TheForge/Prompts/github/FORGE_ALIGNMENT_PLAN.md`  
**Type:** Strategic Planning  
**Priority:** MEDIUM

**Purpose:**  
Strategic plan for aligning Forge governance with constitution and external systems.

**When to Use:**
- ? Long-term governance planning
- ? System integration strategy
- ? Alignment reviews

---

### ?? validation/
**Location:** `TheForge/Prompts/github/validation/`  
**Type:** Validation System  
**Priority:** HIGH

**Contains:**
- `Validate-ForgeSystem.ps1` - PowerShell validation script
- `validation-config.json` - Configuration
- `reports/` - Validation reports (13+ files)
- `templates/` - Report templates

**Purpose:**  
Automated system for validating Forge compliance and generating reports.

**When to Use:**
- ? Periodic compliance audits
- ? Pre-release validation
- ? Governance enforcement

---

#### ?? Validate-ForgeSystem.ps1
**Location:** `TheForge/Prompts/github/validation/Validate-ForgeSystem.ps1`  
**Type:** PowerShell Script  
**Priority:** HIGH

**Purpose:**  
Automated validation script that checks Forge system compliance.

**When to Use:**
- ? Run periodic system validation
- ? Generate compliance reports
- ? Detect governance violations

---

#### ?? validation-config.json
**Location:** `TheForge/Prompts/github/validation/validation-config.json`  
**Type:** Configuration File  
**Priority:** MEDIUM

**Purpose:**  
Configuration for validation system behavior and thresholds.

---

#### ?? validation/reports/
**Location:** `TheForge/Prompts/github/validation/reports/`  
**Type:** Validation Reports  
**Priority:** LOW

**Purpose:**  
Historical validation reports (13+ files with timestamps).

**When to Use:**
- ? Review past validation results
- ? Track compliance trends
- ? Audit history

---

#### ?? validation/templates/
**Location:** `TheForge/Prompts/github/validation/templates/`  
**Type:** Report Templates  
**Priority:** LOW

**Files:**
- `.gitkeep` - Git directory placeholder
- `FORGE_REVISION_TEMPLATE.md` - Revision documentation template
- `VALIDATION_REPORT_TEMPLATE.md` - Validation report template

**Purpose:**  
Templates for generating validation and revision documentation.

---

## ?? Spec-Kit Files (TheForge/Prompts/specify/)

**Location:** `TheForge/Prompts/specify/`  
**Type:** Spec-Kit Workflow Files  
**Priority:** HIGH

**Note:** For Spec-Kit CLI documentation, see official Spec-Kit repository at https://github.com/github/spec-kit

---

### ?? specify/features/
**Location:** `TheForge/Prompts/specify/features/`  
**Purpose:** Active feature development workspace

**Current Feature: add-readme-files**

---

#### ?? spec.md
**Location:** `TheForge/Prompts/specify/features/add-readme-files/spec.md`  
**Status:** ? Complete  
**Purpose:** Requirements and user stories

**Contains:**
- User Story 1 (P1): Developer README files
- Acceptance criteria
- Forge compliance requirements
- Constraints
- Success criteria

**Created By:** Manual Spec-Kit specify workflow

---

#### ?? plan.md
**Location:** `TheForge/Prompts/specify/features/add-readme-files/plan.md`  
**Status:** ? Complete  
**Purpose:** Technical implementation plan

**Contains:**
- Tech stack (Markdown, Forge metadata)
- File structure (2 READMEs)
- Dependencies (ForgeCharter Section 9)
- 4-phase strategy
- 3-hour timeline estimate
- Risk assessment

**Created By:** Manual Spec-Kit plan workflow

---

#### ?? tasks.md
**Location:** `TheForge/Prompts/specify/features/add-readme-files/tasks.md`  
**Status:** ? Complete  
**Purpose:** Actionable task breakdown

**Contains:**
- 12 tasks organized in 3 phases
- Task IDs (T001-T012)
- Parallelizable tasks marked [P]
- Story labels [US1]
- Forge compliance checkpoints

**Created By:** Manual Spec-Kit tasks workflow

---

### ?? specify/memory/
**Location:** `TheForge/Prompts/specify/memory/`  
**Purpose:** Project memory and constitution

---

#### ?? constitution.md
**Location:** `TheForge/Prompts/specify/memory/constitution.md`  
**Type:** Project Constitution  
**Purpose:** Project-wide principles and constraints

**Contains:**
- Project principles
- Forge compliance requirements
- Naming conventions
- Architecture constraints
- Decision rationale

**Used By:**
- Spec-Kit workflow (manual execution)
- Developers (for context)

**When Created:**
- ? First time Spec-Kit constitution workflow runs
- ? Updated when principles change

---

### ?? specify/scripts/powershell/
**Location:** `TheForge/Prompts/specify/scripts/powershell/`  
**Purpose:** PowerShell utility scripts

---

#### ?? check-prerequisites.ps1
**Purpose:** Verify environment setup and dependencies

**When to Use:**
- ? Before running Spec-Kit workflows
- ? Troubleshooting setup issues
- ? Validating installation

**What It Does:**
- Checks Python installation
- Verifies uv package manager
- Validates Git
- Reports feature directory locations

---

#### ?? common.ps1
**Purpose:** Shared utility functions for other scripts

**When to Use:**
- ? Imported by other scripts automatically

**Contains:**
- Common functions
- Error handling
- Path utilities
- JSON parsing

---

#### ?? create-new-feature.ps1
**Purpose:** Scaffold new feature directory structure

**When to Use:**
- ? Creating new feature workspace
- ? Initializing feature folders

**What It Does:**
- Creates feature directory
- Sets up folder structure
- Initializes placeholder files

---

#### ?? setup-plan.ps1
**Purpose:** Initialize plan.md from template

**When to Use:**
- ? After specification complete
- ? Before manual planning

**What It Does:**
- Copies plan template
- Populates feature name
- Sets up initial structure

---

#### ?? update-agent-context.ps1
**Purpose:** Update agent context files

**When to Use:**
- ? After modifying features
- ? Syncing agent state

**What It Does:**
- Updates context files
- Syncs feature metadata
- Refreshes agent knowledge

---

### ?? specify/templates/
**Location:** `TheForge/Prompts/specify/templates/`  
**Purpose:** Spec-Kit file templates

---

#### ?? agent-file-template.md
**Purpose:** Template for creating new agents  
**Used By:** Developers (manual)  
**Output:** New agent files

**Contains:**
- Agent metadata structure
- Handoff configuration
- Outline format
- Instructions format

---

#### ?? checklist-template.md
**Purpose:** Structure for QA checklists  
**Used By:** Spec-Kit checklist workflow  
**Output:** Checklist files

**Contains:**
- Quality criteria
- Validation steps
- Success metrics
- Test scenarios

---

#### ?? plan-template.md
**Purpose:** Structure for implementation plans  
**Used By:** Spec-Kit plan workflow  
**Output:** `features/[name]/plan.md`

**Contains:**
- Tech stack section
- File structure plan
- Dependencies list
- Implementation strategy
- Timeline estimates

---

#### ?? spec-template.md
**Purpose:** Structure for specification documents  
**Used By:** Spec-Kit specify workflow  
**Output:** `features/[name]/spec.md`

**Contains:**
- User story format
- Acceptance criteria structure
- Priority organization
- Constraint sections

---

#### ?? tasks-template.md
**Purpose:** Structure for task breakdowns  
**Used By:** Spec-Kit tasks workflow  
**Output:** `features/[name]/tasks.md`

**Contains:**
- Phase organization
- Task checklist format
- Parallelization markers
- Story labels
- Task numbering

---

## ?? Documentation Files

### ?? README.md (Reference Hub)
**Location:** `TheForge/Documentation/Reference/README.md`  
**Type:** Master Index  
**Priority:** HIGH

**Purpose:**  
Central navigation hub for all Spec-Kit and Forge resources.

**Contains:**
- Quick navigation links
- Command reference
- File location map
- Workflow integration guide
- Troubleshooting section

---

### ?? Workspace-Summary.md
**Location:** `TheForge/Documentation/Reference/Workspace-Summary.md`  
**Type:** Workspace Guide  
**Priority:** HIGH

**Purpose:**  
Complete summary of workspace structure, projects, and repositories.

**Contains:**
- Workspace overview
- Git repositories
- Project details
- Directory structure
- Current active work
- Compliance status

---

### ?? File-Structure-Guide.md
**Location:** `TheForge/Documentation/Reference/File-Structure-Guide.md`  
**Type:** File Reference  
**Priority:** HIGH

**Purpose:**  
This document - complete file structure with descriptions and usage.

---

### ?? Unified-Workflow-Guide.md
**Location:** `TheForge/Documentation/Reference/Unified-Workflow-Guide.md`  
**Type:** Workflow Documentation  
**Priority:** HIGH

**Purpose:**  
Complete end-to-end workflow for feature development integrating Forge governance and Spec-Kit.

**Contains:**
- 7-step workflow (Constitution ? Commit)
- Integration points (Spec-Kit ? Forge)
- Responsibilities by role
- Example feature walkthrough (add-readme-files)
- Quick reference commands

**When to Use:**
- ? Starting any new feature
- ? Understanding overall workflow
- ? Training new team members
- ? Reference during feature development

---

### ?? Forge-Improvement-Suggestions (Split Series)
**Location:** `TheForge/Documentation/Reference/`  
**Type:** Governance Improvement Documentation  
**Priority:** MEDIUM

**Files:**
1. `Forge-Improvement-Suggestions-Overview.md` - Index and summary
2. `Forge-Improvement-Suggestions-High.md` - Critical priorities (2 suggestions)
3. `Forge-Improvement-Suggestions-Medium.md` - Important improvements (4 suggestions)
4. `Forge-Improvement-Suggestions-Low.md` - Enhancement suggestions (4 suggestions)
5. `Forge-Improvement-Suggestions-Tracking.md` - Implementation tracking

**Purpose:**  
Documents 10 prioritized suggestions for improving TheForge governance system.

**Total Suggestions:** 10 (split to comply with ForgeCharter 10,000 character limit)

**When to Use:**
- ? Planning governance improvements
- ? Tracking system refinements
- ? Prioritizing technical debt
- ? Decision-making for enhancements

**Status:** Split from single file to meet ForgeCharter Section 9.4 compliance

---

### ?? SpecKit-Tutorial-AddREADMEs.md
**Location:** `TheForge/Documentation/Technical/SpecKit-Tutorial-AddREADMEs.md`  
**Type:** Tutorial  
**Priority:** MEDIUM

**Purpose:**  
Step-by-step tutorial for using Spec-Kit to add README files while maintaining Forge compliance.

**Contains:**
- Complete workflow walkthrough
- Spec-Kit commands explained
- Forge integration examples
- Troubleshooting guide
- Success criteria

---

### ?? IssueSummary.md (Actual Database)
**Location:** `TheForge/Documentation/Chronicle/DevelopmentLog/IssueSummary.md`  
**Type:** Issue Pattern Database  
**Priority:** MEDIUM

**Purpose:**  
Actual database of recurring development issues and resolutions for governance rule derivation.

**Contains:**
- 10 issue categories by frequency
- Pattern descriptions
- Symptoms
- Resolution patterns
- Usage in governance
- Maintenance guidelines

**When to Use:**
- ? Referenced by ForgeAudit for pattern detection
- ? Updated when new patterns emerge
- ? Monthly governance review

**Related:** Instructions in `TheForge/Prompts/Tasks/IssueSummary.md`

---

### ?? ComplianceJourney-Log.md
**Location:** `TheForge/Documentation/Chronicle/ComplianceJourney-Log.md`  
**Type:** Development Log  
**Priority:** LOW

**Purpose:**  
Chronicles the journey toward Forge compliance and governance maturity.

---

## ?? File Statistics

| Category | Count | Location |
|----------|-------|----------|
| **Governance Files** | 5 | `TheForge/Prompts/` |
| **Branch Files** | 3 | `TheForge/Prompts/Branch-*.md` |
| **Agent Personalities** | 7 | `TheForge/Prompts/Skills/` |
| **Task Files** | 2 | `TheForge/Prompts/Tasks/` |
| **GitHub Config** | 15+ | `TheForge/Prompts/github/` |
| **Spec-Kit Files** | 20+ | `TheForge/Prompts/specify/` |
| **Spec-Kit Templates** | 5 | `TheForge/Prompts/specify/templates/` |
| **PowerShell Scripts** | 5 | `TheForge/Prompts/specify/scripts/powershell/` |
| **Validation System** | 15+ | `TheForge/Prompts/github/validation/` |
| **Reference Docs** | 10 | `TheForge/Documentation/Reference/` |
| **Technical Docs** | 1+ | `TheForge/Documentation/Technical/` |
| **Chronicle Docs** | 2+ | `TheForge/Documentation/Chronicle/` |
| **Active Features** | 1 | `TheForge/Prompts/specify/features/` |

---

## ?? Notes

- All paths relative to: `C:\Users\rchau\source\repos\TheForge\`
- **Working Directories:** 
  - `TheForge/Prompts/github/` - Active GitHub configuration
  - `TheForge/Prompts/specify/` - Active Spec-Kit files
- **Backup Directories:** 
  - `.github/` (workspace root, hidden) - Backup copy, not used
  - `.specify/` (workspace root, hidden) - Backup copy, not used
- ForgeCharter.md is ALWAYS the ultimate authority
- Branch files specialize, never override ForgeCharter
- Spec-Kit workflow executed manually in this system
- Files with issues:
  - `Improvments_In_RAG.md` - typo in filename
  - `Forge-Agents.md.md` - double .md extension
  - `file1.md` - unclear purpose (may need review)

---

**Last Updated:** 2025-01-04  
**Maintained By:** The Forge System  
**Status:** ? Complete and Current

---

**End of File Structure Guide**
