# RCH.Forge.Dashboard � Naming Canon
**Document Type:** Lore  
**Purpose:** Define naming conventions for the Dashboard project  
**Last Updated:** 2025-01-02  
**Related Documents:** ForgeTome.md, file5.md, File6.md

---

## Overview
This document defines the authoritative naming conventions for the RCH.Forge.Dashboard project. All code, files, and documentation must follow these rules to ensure determinism, clarity, and maintainability.

---

## Global Naming Rules ()
- Names must reflect purpose and eliminate ambiguity
- No abbreviations unless industry-standard
- No overloaded or vague names
- No "Helper", "Manager", "Utils", or similar catch-all terms
- Names must be explicit, descriptive, and deterministic
- Forge-themed naming is encouraged where appropriate

---

## Project Naming
**Format:** `RCH.Forge.[ModuleName]`

**Example:**
- `RCH.Forge.Dashboard` ?
- `TheForge` ? (does not follow convention)
- `DashboardApp` ? (missing RCH.Forge prefix)

---

## Class Naming

### Forms
**Format:** `[Purpose]Form`

**Examples:**
- `DashboardMainForm` ?
- `ModuleTestForm` ?
- `Form1` ? (default name)
- `MainWindow` ? (not descriptive enough)

### Controls
**Format:** `[Purpose]Control`

**Examples:**
- `ModuleListControl` ?
- `LogPanelControl` ?
- `TestAreaControl` ?
- `UserControl1` ? (default name)

### Services
**Format:** `[Purpose]Service`

**Examples:**
- `ModuleLoaderService` ?
- `LoggingService` ?
- `DashboardStateService` ?
- `ModuleManager` ? (avoid "Manager")
- `LogHelper` ? (avoid "Helper")

### Interfaces
**Format:** `I[Purpose]Service` or `I[Capability]`

**Examples:**
- `IModuleLoaderService` ?
- `ILoggingService` ?
- `IModuleProvider` ?

---

## File Naming

### Code Files
**Format:** Match the class name exactly

**Examples:**
- `DashboardMainForm.vb` ?
- `ModuleLoaderService.vb` ?
- `Form1.vb` ? (default name)

### Documentation Files
**Format:** `[Purpose].[Type].md`

**Examples:**
- `ForgeTome.md` ?
- `VersionHistory.chronicle.md` ?
- `NamingCanon.md` ?
- `README.md` ?
- `doc1.md` ? (not descriptive)

---

## Control Naming (UI Elements)

### Buttons
**Format:** `btn[Action][Target]`

**Examples:**
- `btnLoadModule` ?
- `btnClearLog` ?
- `btnStart` ?
- `Button1` ? (default name)

### Panels
**Format:** `pnl[Purpose]`

**Examples:**
- `pnlModuleList` ?
- `pnlLogOutput` ?
- `pnlTestArea` ?
- `Panel1` ? (default name)

### TextBoxes
**Format:** `txt[Purpose]`

**Examples:**
- `txtModuleName` ?
- `txtLogOutput` ?
- `TextBox1` ? (default name)

### ListBoxes / ListViews
**Format:** `lst[Content]` or `lv[Content]`

**Examples:**
- `lstModules` ?
- `lvAvailableModules` ?
- `ListBox1` ? (default name)

---

## Forge-Themed Naming (Encouraged)

### Vocabulary
When appropriate, use Forge-themed terms:
- **Forge** � The ecosystem or creation environment
- **Anvil** � Where components are shaped
- **Smith** � A creator or builder
- **Codex** � Technical references
- **Chronicle** � Historical records
- **Tome** � Instructional guides
- **Lore** � Philosophy and principles
- **Grimoire** � Advanced or experimental knowledge
- **Scriptorium** � Documentation engine

### Examples
- `ForgeModuleLoader` ?
- `SmithConfiguration` ?
- `AnvilTestHarness` ?

---

## Variable Naming

### Local Variables
**Format:** `camelCase`, descriptive

**Examples:**
- `moduleName` ?
- `selectedModule` ?
- `logEntries` ?
- `temp` ? (not descriptive)
- `x` ? (not descriptive)

### Private Fields
**Format:** `_camelCase`

**Examples:**
- `_moduleLoader` ?
- `_loggingService` ?
- `_state` ? (too vague)

### Constants
**Format:** `PascalCase` or `UPPER_SNAKE_CASE`

**Examples:**
- `MaxModuleCount` ?
- `DEFAULT_LOG_CAPACITY` ?
- `CONST1` ? (not descriptive)

---

## Method Naming

**Format:** `[Verb][Noun]`

**Examples:**
- `LoadModule()` ?
- `ClearLog()` ?
- `GetAvailableModules()` ?
- `Process()` ? (not descriptive)
- `DoStuff()` ? (vague)

---

## Event Handler Naming

**Format:** `[ControlName]_[Event]`

**Examples:**
- `btnLoadModule_Click` ?
- `lstModules_SelectedIndexChanged` ?
- `Button1_Click` ? (default control name)

---

## Summary
These naming conventions ensure that all code in the RCH.Forge.Dashboard project is deterministic, explicit, and maintainable. Adherence to these rules is mandatory for all contributions.

**Remember:**
- Be explicit
- Be descriptive
- Be deterministic
- Avoid defaults
- Embrace Forge vocabulary when appropriate

---

**The name is the first documentation.**
