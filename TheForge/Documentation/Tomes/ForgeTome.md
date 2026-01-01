# RCH.Forge.Dashboard — Forge Tome
**Document Type:** Tome  
**Purpose:** High-level overview and onboarding guide for the Dashboard project  
**Last Updated:** 2025-01-02  
**Related Documents:** NamingCanon.md, VersionHistory.chronicle.md, file3.md

---

## Overview
The **RCH.Forge.Dashboard** is a WinForms application designed to test, orchestrate, and interact with modules in the RCH UI Forge ecosystem. It serves as a centralized test harness and demonstration environment.

**Key Features:**
- Dynamic module loading and testing
- Real-time log output and diagnostics
- Modular, extensible architecture
- Deterministic UI behavior

---

## Purpose
The Dashboard provides:
1. A test environment for Forge modules
2. A demonstration platform for showcasing module capabilities
3. A development tool for debugging and validation
4. A reference implementation of Forge architecture principles

---

## Getting Started

### Prerequisites
- .NET 8.0 or later
- Windows OS with WinForms support
- Visual Studio 2022 or later (recommended)

### Building the Project
1. Open the solution in Visual Studio
2. Restore NuGet packages (if any)
3. Build the solution (Ctrl+Shift+B)
4. Run the Dashboard (F5)

### First Run
On first launch, the Dashboard displays:
- **Left Panel:** Module list (initially empty)
- **Bottom Panel:** Log output with startup messages
- **Center Panel:** Test area (awaiting module selection)

---

## Architecture

### UI Layer (`/Source/UI`)
- `DashboardMainForm.vb` — Main application window
- Panel-based layout for modularity
- No business logic in UI

### Services Layer (`/Source/Services`)
- `ModuleLoaderService.vb` — Handles module discovery and loading
- `LoggingService.vb` — Centralized logging and diagnostics
- `DashboardStateService.vb` — Manages application state

### Resources (`/Resources`)
- Images, icons, and UI assets
- Sample data and configuration files

---

## Key Principles
This Dashboard adheres to the **Forge Prime Directives** (File6.md):
- **Determinism:** All behavior is explicit and predictable
- **Modularity:** Components are self-contained and reusable
- **Onboarding-First:** Documentation reduces cognitive load
- **Architecture Discipline:** Clear separation of UI and logic

---

## Documentation Structure
The Dashboard documentation follows the RCH UI Forge Documentation Taxonomy (file4.md):
- **/Codex** — Technical API references
- **/Chronicle** — Version history and release notes
- **/Tomes** — User guides and tutorials (this document)
- **/Lore** — Design philosophy and naming conventions
- **/Grimoire** — Experimental features and research
- **/Scriptorium** — Templates and generated docs

---

## Next Steps
1. Review the **NamingCanon.md** for naming conventions
2. Explore the **DashboardMainForm** layout and structure
3. Add your first module using the Module Loader
4. Check the **VersionHistory.chronicle.md** for recent changes

---

## Support & Contribution
For questions, issues, or contributions:
- See `/Documentation/Tomes` for detailed guides
- See `/Documentation/Lore` for design principles
- See `/Documentation/Chronicle` for change history

---

**Welcome to the Forge.**
