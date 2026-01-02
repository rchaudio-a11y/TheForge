# Architecture Rules Summary - The Forge

**Document Type:** Technical Reference  
**Created:** 2025-01-02  
**Character Count:** 6,892  
**Status:** Final  
**Source:** File6.md (Prime Directives) + file2.md (Project Layout)  
**Related:** CodeRules-Summary.md, DocumentationRules-Summary.md

---

## Overview

This document summarizes The Forge architecture rules from File6.md (Prime Directives) and file2.md (Project Layout Template). These rules define **what** to build and **how** to structure projects.

---

## Part 1: Prime Directives (File6.md)

### Universal Principles

These 10 directives apply to **EVERY** Forge project, regardless of domain, language, or purpose.

---

### 1. Determinism ?
**All systems must behave predictably and consistently.**

**Rules:**
- ? No hidden state
- ? No implicit behavior
- ? No magic strings or numbers
- ? All behavior explicit and discoverable
- ? All modules independently testable
- ? UI deterministic and predictable

**Why:** Predictability reduces bugs and aids debugging.

---

### 2. Modularity ??
**All components must be self-contained and reusable.**

**Rules:**
- ? Single, clear purpose per module
- ? Modules must not depend on UI
- ? Expose explicit, documented interfaces
- ? No leaked internal state
- ? Reusable across projects
- ? No circular dependencies

**Why:** Modularity enables testing, reuse, and maintainability.

---

### 3. Onboarding-First Design ??
**Every project must be understandable within minutes.**

**Rules:**
- ? Every folder has README.md
- ? Every module has "How to Use" section
- ? Every public class has purpose summary
- ? Every project has quickstart guide
- ? Documentation reduces cognitive load

**Why:** Fast onboarding = productive contributors.

---

### 4. Naming Canon (Global) ???
**Names must reflect purpose and eliminate ambiguity.**

**Rules:**
- ? No abbreviations (unless industry-standard)
- ? No overloaded/vague names
- ? No "Helper", "Manager", "Utils" catch-alls
- ? Explicit, descriptive, deterministic names
- ? Forge-themed naming encouraged

**Examples:**
- ? Bad: `DataHelper`, `ModuleManager`, `Utils`
- ? Good: `ModuleLoaderService`, `LoggingService`, `ModuleMetadata`

**Why:** Clear names reduce cognitive load and ambiguity.

---

### 5. Architecture Discipline ???
**Architectural clarity is mandatory.**

**Rules:**
- ? UI layer must be thin
- ? Services stateless when possible
- ? State centralized and explicit
- ? Dependencies injected (not created ad-hoc)
- ? No business logic in event handlers
- ? No implicit side effects

**Why:** Clean architecture enables testing and maintainability.

---

### 6. Documentation Philosophy ??
**Documentation must serve maintainers and future contributors.**

**Rules:**
- ? Updated with every change
- ? Concise, structured, Forge-themed
- ? Never contradicts code
- ? Explains *why*, not just *what*
- ? Reduces cognitive load

**Why:** Good docs = faster onboarding and fewer questions.

---

### 7. Code Quality Standards ?
**All code must be clean, readable, and intentional.**

**Rules:**
- ? No unused code
- ? No commented-out code
- ? No TODOs without owners
- ? No warnings allowed
- ? All public APIs documented
- ? No horizontal scrolling to read code

**Why:** Quality code is maintainable code.

---

### 8. Versioning & Change Discipline ??
**Every change must be traceable and justified.**

**Rules:**
- ? All changes logged in Chronicle
- ? Every release includes rationale
- ? Breaking changes documented in advance
- ? Deprecated features ? `/Chronicle/DeprecatedArchive`

**Why:** Traceability aids debugging and auditing.

---

### 9. UX/UI Consistency ??
**User interfaces must be predictable and stable.**

**Rules:**
- ? All controls have explicit names
- ? No default control names (Button1, TextBox1)
- ? Layout grid-aligned
- ? No flickering, jumping, or unexpected resizing
- ? No hidden or implicit behaviors

**Why:** Consistent UI = better user experience.

---

### 10. Forge Ethos ??
**The guiding philosophy of The Forge ecosystem.**

**Build for:**
- ? Clarity
- ? Maintainers
- ? Reuse
- ? Future-proofing
- ? Onboarding
- ? Determinism
- ? Elegance

**Why:** These principles ensure long-term success.

---

## Part 2: Project Layout (file2.md)

### Standard Folder Structure

```
/ProjectName
    /Documentation       ? All docs (follows file4.md taxonomy)
    /Source
        /Core            ? Foundational logic, shared utilities
        /Modules         ? Self-contained feature modules
        /UI              ? Forms, controls, views
    /Resources
        /Images          ? Icons, UI assets, branding
        /Data            ? JSON, XML, CSV, datasets
        /Localization    ? Language files, resource dictionaries
    /Tests
        /Unit            ? Unit tests
        /Integration     ? Integration tests
```

---

### Folder Rules

#### **/Documentation**
- Holds all project documentation
- Must follow file4.md taxonomy (Codex, Chronicle, Tomes, Lore, etc.)
- ? NO code files here

**Examples:**
- `ForgeTome.md`
- `VersionHistory.chronicle.md`
- `NamingCanon.md`

---

#### **/Source/Core**
- Foundational logic, shared utilities, base classes
- ? NO UI code

**Examples:**
- Validation services
- Base models
- Shared interfaces

---

#### **/Source/Modules**
- Reusable components, feature modules, subsystems
- Each module self-contained
- Can be used across projects

**Examples:**
- `AudioEngine`
- `LayoutManager`
- `ControlSurface`

---

#### **/Source/UI**
- Forms, controls, views, UI-specific logic
- Follows Designer file rules (file8.md)

**Examples:**
- `MainForm.vb`
- `CustomControl.vb`
- `DashboardMainForm.vb`

---

#### **/Resources/Images**
- Icons, UI assets, branding materials
- Descriptive filenames
- Vector formats when possible

**Examples:**
- `Logo.Dashboard.png`
- `Icon.ModuleList.svg`

---

#### **/Resources/Data**
- JSON, XML, CSV, sample datasets
- Well-formed and validated
- Include schema documentation

**Examples:**
- `SampleModuleManifest.json`
- `TestConfiguration.xml`

---

#### **/Resources/Localization**
- Language files, resource dictionaries
- Organized by locale

**Examples:**
- `Strings.en-US.resx`
- `Strings.es-ES.resx`

---

#### **/Tests**
- Optional folder for automated testing
- `/Unit` for component tests
- `/Integration` for interaction tests

---

### README.md Requirements

**EVERY folder MUST have README.md with:**
1. **Purpose** - What this folder contains
2. **Rules** - What belongs here (and what doesn't)
3. **Examples** - Sample filenames
4. **Cross-references** - Links to related docs

**Template:**
```markdown
# {Folder Name}

**Purpose:** {One-line description}

**Rules:**
- {Rule 1}
- {Rule 2}

**Examples:**
- {Example file 1}
- {Example file 2}

**Cross-references:**
- See /Documentation/Codex for API docs
```

---

## Architecture Patterns

### Layered Architecture

```
???????????????????????????????
?          UI Layer           ? ? Thin, orchestration only
?  (Forms, Controls, Views)   ?
???????????????????????????????
              ?
???????????????????????????????
?       Service Layer         ? ? Business logic, stateless
?  (Services, Managers)       ?
???????????????????????????????
              ?
???????????????????????????????
?        Core Layer           ? ? Shared utilities, models
?  (Models, Interfaces)       ?
???????????????????????????????
```

### Dependency Flow

- ? UI ? Services ? Core
- ? UI ? Core
- ? Core ? UI (NEVER)
- ? Services ? UI (NEVER)

---

## Key Architectural Decisions

### 1. Thin UI Layer
**Event handlers = orchestration only**
- Gather inputs
- Call services
- Update display
- NO business logic

### 2. Stateless Services
**Preferred pattern:**
- Pure functions when possible
- Explicit state when needed
- No hidden dependencies

### 3. Dependency Injection
**All dependencies:**
- Injected via constructor or initialization
- NOT created ad-hoc
- NOT hidden as globals

### 4. Module Independence
**Each module:**
- Has single purpose
- Exposes clear interface
- No UI dependencies
- Testable in isolation

---

## Naming Conventions

### File Naming
- ? `ModuleLoaderService.vb`
- ? `ModuleMetadata.vb`
- ? `DashboardMainForm.vb`
- ? `Helper.vb`
- ? `Utils.vb`
- ? `Manager.vb`

### Namespace Naming
- ? `TheForge.Services.Implementations`
- ? `TheForge.Models`
- ? `TheForge.UI.Controls`
- ? `TheForge.Helpers`
- ? `TheForge.Utils`

### Control Naming
- ? `moduleListControl`
- ? `btnSave`
- ? `txtSearch`
- ? `Button1`
- ? `TextBox2`

---

## Quality Checklist

### ? Architecture Compliance
- [ ] UI layer is thin (orchestration only)
- [ ] Business logic in services
- [ ] No UI dependencies in Core/Modules
- [ ] Dependencies injected, not created
- [ ] All modules independently testable

### ? Structure Compliance
- [ ] Follows standard folder layout
- [ ] All folders have README.md
- [ ] Files in correct folders
- [ ] No circular dependencies

### ? Code Quality
- [ ] No unused code
- [ ] No commented-out code
- [ ] No warnings
- [ ] All public APIs documented
- [ ] Meaningful names (no Helper, Manager, Utils)

### ? Documentation
- [ ] Updated with changes
- [ ] Explains why, not just what
- [ ] Reduces cognitive load
- [ ] Doesn't contradict code

---

## Common Violations & Fixes

| Violation | Location | Fix |
|-----------|----------|-----|
| Business logic in event handler | UI layer | Move to service |
| Magic strings in code | Anywhere | Extract to const/config |
| Circular dependency | Modules | Introduce interface, invert dependency |
| Helper/Manager/Utils class | Anywhere | Rename to specific purpose |
| Missing README | Folder | Create with template |
| Default control names | UI | Rename meaningfully |

---

## Summary

**The Forge architecture ensures:**
1. **Determinism** - Predictable behavior
2. **Modularity** - Self-contained components
3. **Clarity** - Onboarding-first design
4. **Quality** - Clean, maintainable code
5. **Consistency** - Standard structure

**Follow File6.md Prime Directives + file2.md Layout = Success! ??**

---

**Related Documentation:**
- Prompts/File6.md (Prime Directives - source)
- Prompts/file2.md (Project Layout - source)
- CodeRules-Summary.md (file8.md summary)
- DocumentationRules-Summary.md (file4.md + file7.md summary)
