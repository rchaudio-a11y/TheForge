# File Structure Guide

**Document Type:** Reference Documentation  
**Purpose:** Complete guide to all files in TheForge workspace with descriptions and usage  
**Created:** 2025-01-04  
**Last Updated:** 2025-01-04  
**Status:** Active  
**Character Count:** TBD  
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
??? .github\                                      [GitHub Configuration]
?   ??? ?? copilot-instructions.md               [Router - Task Routing]
?   ??? ?? CONSTITUTION.md                       [Project Constitution]
?   ??? ?? CONSTITUTION_QUICKREF.md              [Constitution Quick Reference]
?   ??? ?? CONSTITUTION_SUMMARY.md               [Constitution Summary]
?   ??? ?? CONSTITUTION_VALIDATION.md            [Constitution Validation Report]
?   ??? ?? CONSTITUTION_ARCHITECTURE_ANALYSIS.md [Architecture Analysis]
?   ??? ?? CONSTITUTION_UPDATE_*.md              [Constitution Updates (5 files)]
?   ??? ?? FORGE_ALIGNMENT_PLAN.md               [Forge Alignment Strategy]
?   ??? validation\                              [Validation System]
?       ??? ?? Validate-ForgeSystem.ps1          [PowerShell Validator]
?       ??? ?? validation-config.json            [Validation Config]
?       ??? reports\                             [Validation Reports]
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
?   ?   ?   ??? ?? IssueSummary.md               [Issue Patterns]
?   ?   ??? Skills\                              [Agent Personalities]
?   ?   ?   ??? ?? Claude Opus 4.5 Prompt Pack.md
?   ?   ?   ??? ?? GPT-5.1-Codex-Max Prompt Pack.md
?   ?   ??? SpecKit\                             [Spec-Kit Files]
?   ?       ??? agents\                          [Spec-Kit Agents (9 files)]
?   ?       ?   ??? ?? speckit.analyze.agent.md
?   ?       ?   ??? ?? speckit.checklist.agent.md
?   ?       ?   ??? ?? speckit.clarify.agent.md
?   ?       ?   ??? ?? speckit.constitution.agent.md
?   ?       ?   ??? ?? speckit.implement.agent.md
?   ?       ?   ??? ?? speckit.plan.agent.md
?   ?       ?   ??? ?? speckit.specify.agent.md
?   ?       ?   ??? ?? speckit.tasks.agent.md
?   ?       ?   ??? ?? speckit.taskstoissues.agent.md
?   ?       ??? prompts\                         [CLI Templates (9 files)]
?   ?       ?   ??? ?? speckit.analyze.prompt.md
?   ?       ?   ??? ?? speckit.checklist.prompt.md
?   ?       ?   ??? ?? speckit.clarify.prompt.md
?   ?       ?   ??? ?? speckit.constitution.prompt.md
?   ?       ?   ??? ?? speckit.implement.prompt.md
?   ?       ?   ??? ?? speckit.plan.prompt.md
?   ?       ?   ??? ?? speckit.specify.prompt.md
?   ?       ?   ??? ?? speckit.tasks.prompt.md
?   ?       ?   ??? ?? speckit.taskstoissues.prompt.md
?   ?       ??? templates\                       [Spec-Kit Templates]
?   ?       ?   ??? ?? agent-file-template.md
?   ?       ?   ??? ?? checklist-template.md
?   ?       ?   ??? ?? plan-template.md
?   ?       ?   ??? ?? spec-template.md
?   ?       ?   ??? ?? tasks-template.md
?   ?       ??? scripts\                         [PowerShell Scripts]
?   ?       ?   ??? powershell\
?   ?       ?       ??? ?? check-prerequisites.ps1
?   ?       ?       ??? ?? common.ps1
?   ?       ?       ??? ?? create-new-feature.ps1
?   ?       ?       ??? ?? setup-plan.ps1
?   ?       ?       ??? ?? update-agent-context.ps1
?   ?       ??? memory\                          [Project Memory]
?   ?       ?   ??? ?? constitution.md
?   ?       ??? features\                        [Feature Specifications]
?   ?           ??? add-readme-files\
?   ?               ??? ?? spec.md
?   ?               ??? ?? plan.md
?   ?               ??? ?? tasks.md
?   ?
?   ??? Documentation\                           [Documentation Hub]
?       ??? Reference\                           [Quick Reference]
?       ?   ??? ?? README.md                     [Reference Hub Index]
?       ?   ??? ?? Workspace-Summary.md          [Workspace Guide]
?       ?   ??? ?? File-Structure-Guide.md       [This File]
?       ??? Technical\                           [Technical Docs]
?       ?   ??? ?? SpecKit-Tutorial-AddREADMEs.md
?       ??? Chronicle\                           [Development History]
?       ?   ??? ?? ComplianceJourney-Log.md
?       ?   ??? DevelopmentLog\
?       ?       ??? ?? IssueSummary.md
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
- Spec-Kit plan agent (for file organization)
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
- Spec-Kit implement agent (for code generation)
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
- Spec-Kit implement agent (for documentation)
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

### ?? IssueSummary.md
**Location:** `TheForge/Prompts/Tasks/IssueSummary.md`  
**Type:** Issue Pattern Database  
**Priority:** MEDIUM

**Purpose:**  
Maintains patterns of common issues and violations for ForgeAudit.

**Contains:**
- Common violation patterns
- Auto-fix rules
- Issue categorization
- Historical patterns

**When to Use:**
- ? Referenced by ForgeAudit automatically
- ? Updated when new patterns discovered
- ? Synced across locations

**Note:** Also exists in `TheForge/Documentation/Chronicle/DevelopmentLog/IssueSummary.md` (synced)

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

## ?? Spec-Kit Agent Files

All agent files located in: `TheForge/Prompts/SpecKit/agents/`

### ?? speckit.constitution.agent.md
**Purpose:** Establish project principles and constraints  
**Command:** `/speckit.constitution`  
**Step:** 1 (First step)

**When to Use:**
- ? Starting new project
- ? Starting major feature
- ? Defining project boundaries

**Inputs:**
- ForgeCharter.md

**Outputs:**
- `TheForge/Prompts/SpecKit/memory/constitution.md`

**What It Does:**
- Reviews ForgeCharter governance
- Documents project principles
- Establishes constraints
- Sets foundation for decisions

---

### ?? speckit.specify.agent.md
**Purpose:** Create feature specifications with user stories  
**Command:** `/speckit.specify`  
**Step:** 2

**When to Use:**
- ? Starting new feature
- ? Defining requirements
- ? Documenting user stories

**Inputs:**
- constitution.md
- ForgeCharter.md (Section 9)

**Outputs:**
- `features/[feature-name]/spec.md`

**What It Does:**
- Parses user stories
- Extracts acceptance criteria
- Creates structured specification
- Organizes by priority

---

### ?? speckit.plan.agent.md
**Purpose:** Generate technical implementation plans  
**Command:** `/speckit.plan`  
**Step:** 3

**When to Use:**
- ? After specification complete
- ? Before implementation
- ? Planning technical approach

**Inputs:**
- spec.md
- ForgeCharter.md
- Branch-*.md files

**Outputs:**
- `features/[feature-name]/plan.md`

**What It Does:**
- Analyzes requirements
- Defines tech stack
- Plans file structure
- Identifies dependencies
- Creates implementation strategy
- Includes Forge compliance steps

---

### ?? speckit.tasks.agent.md
**Purpose:** Generate actionable task breakdowns  
**Command:** `/speckit.tasks`  
**Step:** 4

**When to Use:**
- ? After plan complete
- ? Before implementation
- ? Converting plan to tasks

**Inputs:**
- spec.md
- plan.md
- ForgeCharter.md

**Outputs:**
- `features/[feature-name]/tasks.md`

**What It Does:**
- Reads spec and plan
- Generates task checklist
- Organizes by phase
- Marks parallelizable tasks
- Adds story labels
- Includes compliance checkpoints

---

### ?? speckit.implement.agent.md
**Purpose:** Execute implementation tasks  
**Command:** `/speckit.implement`  
**Step:** 5 (Main implementation)

**When to Use:**
- ? After tasks generated
- ? During development
- ? Executing each task

**Inputs:**
- tasks.md
- ForgeCharter.md
- Branch-*.md files (as needed)

**Outputs:**
- Actual project files (code, docs, etc.)

**What It Does:**
- Guides through each task
- Generates content
- Adds Forge metadata
- Validates compliance
- Marks tasks complete

---

### ?? speckit.clarify.agent.md
**Purpose:** Ask structured clarifying questions  
**Command:** `/speckit.clarify`  
**Step:** Optional (before plan)

**When to Use:**
- ? Requirements unclear
- ? Before planning
- ? De-risking ambiguity

**Inputs:**
- spec.md (partial or draft)

**Outputs:**
- Clarification questions
- Risk analysis

**What It Does:**
- Identifies ambiguities
- Generates questions
- Suggests alternatives
- Reduces implementation risk

---

### ?? speckit.analyze.agent.md
**Purpose:** Cross-artifact consistency checking  
**Command:** `/speckit.analyze`  
**Step:** Optional (after tasks)

**When to Use:**
- ? After tasks generated
- ? Before implementation
- ? Checking consistency

**Inputs:**
- spec.md
- plan.md
- tasks.md

**Outputs:**
- Analysis report
- Inconsistency findings
- Alignment recommendations

**What It Does:**
- Checks spec/plan/tasks alignment
- Identifies gaps
- Reports inconsistencies
- Suggests fixes

---

### ?? speckit.checklist.agent.md
**Purpose:** Generate quality assurance checklists  
**Command:** `/speckit.checklist`  
**Step:** Optional (after plan)

**When to Use:**
- ? After planning
- ? Before implementation
- ? QA planning

**Inputs:**
- plan.md

**Outputs:**
- Quality checklist file

**What It Does:**
- Generates QA checklist
- Creates validation steps
- Defines success criteria
- Plans testing approach

---

### ?? speckit.taskstoissues.agent.md
**Purpose:** Convert tasks to GitHub Issues  
**Command:** `/speckit.taskstoissues`  
**Step:** Optional (after tasks)

**When to Use:**
- ? Managing in GitHub
- ? Team collaboration
- ? Tracking progress

**Inputs:**
- tasks.md

**Outputs:**
- GitHub Issues (via API)

**What It Does:**
- Parses tasks.md
- Creates GitHub Issues
- Links dependencies
- Adds labels

---

## ?? Spec-Kit Prompt Files

All prompt files located in: `TheForge/Prompts/SpecKit/prompts/`

These files are CLI templates used when running Spec-Kit commands via PowerShell (not via Copilot Chat).

| File | Purpose | Used By |
|------|---------|---------|
| `speckit.analyze.prompt.md` | Analysis template | CLI `specify analyze` |
| `speckit.checklist.prompt.md` | Checklist template | CLI `specify checklist` |
| `speckit.clarify.prompt.md` | Questions template | CLI `specify clarify` |
| `speckit.constitution.prompt.md` | Constitution template | CLI `specify constitution` |
| `speckit.implement.prompt.md` | Implementation guide | CLI `specify implement` |
| `speckit.plan.prompt.md` | Planning template | CLI `specify plan` |
| `speckit.specify.prompt.md` | Spec template | CLI `specify` |
| `speckit.tasks.prompt.md` | Task breakdown template | CLI `specify tasks` |
| `speckit.taskstoissues.prompt.md` | Issues template | CLI `specify taskstoissues` |

**CLI Command Format:**
```powershell
uvx --from git+https://github.com/github/spec-kit.git specify [command]
```

---

## ?? Spec-Kit Template Files

All templates located in: `TheForge/Prompts/SpecKit/templates/`

### ?? spec-template.md
**Purpose:** Structure for specification documents  
**Used By:** speckit.specify agent  
**Output:** `features/[name]/spec.md`

**Contains:**
- User story format
- Acceptance criteria structure
- Priority organization
- Constraint sections

---

### ?? plan-template.md
**Purpose:** Structure for implementation plans  
**Used By:** speckit.plan agent  
**Output:** `features/[name]/plan.md`

**Contains:**
- Tech stack section
- File structure plan
- Dependencies list
- Implementation strategy
- Timeline estimates

---

### ?? tasks-template.md
**Purpose:** Structure for task breakdowns  
**Used By:** speckit.tasks agent  
**Output:** `features/[name]/tasks.md`

**Contains:**
- Phase organization
- Task checklist format
- Parallelization markers
- Story labels
- Task numbering

---

### ?? checklist-template.md
**Purpose:** Structure for QA checklists  
**Used By:** speckit.checklist agent  
**Output:** Checklist files

**Contains:**
- Quality criteria
- Validation steps
- Success metrics
- Test scenarios

---

### ?? agent-file-template.md
**Purpose:** Template for creating new agents  
**Used By:** Developers (manual)  
**Output:** New agent files

**Contains:**
- Agent metadata structure
- Handoff configuration
- Outline format
- Instructions format

---

## ?? Spec-Kit PowerShell Scripts

All scripts located in: `TheForge/Prompts/SpecKit/scripts/powershell/`

### ?? check-prerequisites.ps1
**Purpose:** Verify environment setup and dependencies

**When to Use:**
- ? Before running Spec-Kit commands
- ? Troubleshooting setup issues
- ? Validating installation

**What It Does:**
- Checks Python installation
- Verifies uv package manager
- Validates Git
- Reports feature directory locations

---

### ?? common.ps1
**Purpose:** Shared utility functions for other scripts

**When to Use:**
- ? Imported by other scripts automatically

**Contains:**
- Common functions
- Error handling
- Path utilities
- JSON parsing

---

### ?? create-new-feature.ps1
**Purpose:** Scaffold new feature directory structure

**When to Use:**
- ? Creating new feature workspace
- ? Initializing feature folders

**What It Does:**
- Creates feature directory
- Sets up folder structure
- Initializes placeholder files

---

### ?? setup-plan.ps1
**Purpose:** Initialize plan.md from template

**When to Use:**
- ? After specification complete
- ? Before manual planning

**What It Does:**
- Copies plan template
- Populates feature name
- Sets up initial structure

---

### ?? update-agent-context.ps1
**Purpose:** Update agent context files

**When to Use:**
- ? After modifying features
- ? Syncing agent state

**What It Does:**
- Updates context files
- Syncs feature metadata
- Refreshes agent knowledge

---

## ?? Spec-Kit Memory

### ?? constitution.md
**Location:** `TheForge/Prompts/SpecKit/memory/constitution.md`  
**Type:** Project Memory - Constitution

**Purpose:**  
Stores project-level principles and constraints established via `/speckit.constitution`.

**When Created:**
- ? First time `/speckit.constitution` runs
- ? Updated when principles change

**Contains:**
- Project principles
- Forge compliance requirements
- Naming conventions
- Architecture constraints
- Decision rationale

**Used By:**
- All Spec-Kit agents (as reference)
- Developers (for context)

---

## ?? Spec-Kit Feature Workspace

### Current Feature: add-readme-files
**Location:** `TheForge/Prompts/SpecKit/features/add-readme-files/`

#### ?? spec.md
**Status:** ? Complete  
**Created By:** `/speckit.specify`  
**Purpose:** Requirements and user stories

**Contains:**
- User Story 1 (P1): Developer README files
- Acceptance criteria
- Forge compliance requirements
- Constraints
- Success criteria

---

#### ?? plan.md
**Status:** ? Complete  
**Created By:** `/speckit.plan`  
**Purpose:** Technical implementation plan

**Contains:**
- Tech stack (Markdown, Forge metadata)
- File structure (2 READMEs)
- Dependencies (ForgeCharter Section 9)
- 4-phase strategy
- 3-hour timeline estimate
- Risk assessment

---

#### ?? tasks.md
**Status:** ? Complete  
**Created By:** `/speckit.tasks`  
**Purpose:** Actionable task breakdown

**Contains:**
- 12 tasks organized in 3 phases
- Task IDs (T001-T012)
- Parallelizable tasks marked [P]
- Story labels [US1]
- Forge compliance checkpoints

---

## ??? Configuration Files

### ?? copilot-instructions.md
**Location:** `.github/copilot-instructions.md`  
**Type:** Router Configuration  
**Priority:** CRITICAL

**Purpose:**  
Routes all tasks to correct Forge branch and enables Spec-Kit agents.

**Responsibilities:**
- Load ForgeCharter.md
- Enable Spec-Kit agents
- Identify task type
- Delegate to correct branch
- Never interpret or override rules

**Routes To:**
- Branch-Coding.md (code tasks)
- Branch-Architecture.md (structure tasks)
- Branch-Documentation.md (documentation tasks)
- ForgeAudit.md (compliance checking)
- Spec-Kit agents (workflow commands)

---

### ?? SPECKIT_SETUP.md
**Location:** `SPECKIT_SETUP.md` (workspace root)  
**Type:** Setup Documentation  
**Priority:** HIGH

**Purpose:**  
Documents Spec-Kit installation, configuration, and usage.

**Contains:**
- Installation summary
- File structure created
- Usage instructions
- CLI commands
- Integration notes
- Update procedures

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

## ?? Usage Quick Reference

### By Task Type

| Task Type | Primary File(s) | Supporting Files |
|-----------|----------------|------------------|
| **Starting Feature** | `/speckit.constitution`, `/speckit.specify` | ForgeCharter.md |
| **Planning Feature** | `/speckit.plan` | spec.md, Branch-*.md |
| **Breaking Down Tasks** | `/speckit.tasks` | spec.md, plan.md |
| **Implementing** | `/speckit.implement` | tasks.md, Branch-*.md |
| **Writing Code** | Branch-Coding.md | ForgeCharter.md |
| **Structuring Project** | Branch-Architecture.md | ForgeCharter.md |
| **Writing Docs** | Branch-Documentation.md | ForgeCharter.md Section 9 |
| **Checking Compliance** | ForgeAudit.md | All files |
| **De-risking** | `/speckit.clarify` | spec.md |
| **Validating Consistency** | `/speckit.analyze` | spec.md, plan.md, tasks.md |

---

### By Role

#### For Developers
1. Review ForgeCharter.md
2. Use Spec-Kit workflow (constitution ? implement)
3. Apply Branch-* rules during implementation
4. Run ForgeAudit before committing

#### For AI Agents
1. Load copilot-instructions.md (router)
2. Route to appropriate branch
3. Reference ForgeCharter.md (always)
4. Apply Spec-Kit agents for workflow
5. Validate with ForgeAudit.md

#### For Project Managers
1. Review Workspace-Summary.md
2. Track progress via features/ workspace
3. Review compliance via ForgeAudit reports

---

## ?? File Statistics

| Category | Count | Location |
|----------|-------|----------|
| **Governance Files** | 5 | `TheForge/Prompts/` |
| **Spec-Kit Agents** | 9 | `TheForge/Prompts/SpecKit/agents/` |
| **Spec-Kit Prompts** | 9 | `TheForge/Prompts/SpecKit/prompts/` |
| **Spec-Kit Templates** | 5 | `TheForge/Prompts/SpecKit/templates/` |
| **PowerShell Scripts** | 5 | `TheForge/Prompts/SpecKit/scripts/powershell/` |
| **Agent Personalities** | 2 | `TheForge/Prompts/Skills/` |
| **Reference Docs** | 3+ | `TheForge/Documentation/Reference/` |
| **Active Features** | 1 | `TheForge/Prompts/SpecKit/features/` |

---

## ?? Workflow Summary

```
Step 1: Constitution
   ??? /speckit.constitution
       ??? memory/constitution.md

Step 2: Specify
   ??? /speckit.specify
       ??? features/[name]/spec.md

Step 3: Plan
   ??? /speckit.plan
       ??? features/[name]/plan.md

Step 4: Tasks
   ??? /speckit.tasks
       ??? features/[name]/tasks.md

Step 5: Implement
   ??? /speckit.implement
       ??? Project files (code, docs, etc.)

Step 6: Audit
   ??? Run ForgeAudit 2.0
       ??? Compliance report + auto-fixes
```

**All steps reference:** ForgeCharter.md, Branch-*.md (as needed)

---

## ?? Notes

- All paths relative to: `C:\Users\rchau\source\repos\TheForge\`
- Files in `.github/` and `.specify/` maintained for tool compatibility
- Copies in `TheForge/Prompts/SpecKit/` for visibility and review
- ForgeCharter.md is ALWAYS the ultimate authority
- Branch files specialize, never override ForgeCharter
- Spec-Kit handles planning, Forge handles governance

---

**Last Updated:** 2025-01-04  
**Maintained By:** The Forge System  
**Status:** ? Complete and Current

---

**End of File Structure Guide**
