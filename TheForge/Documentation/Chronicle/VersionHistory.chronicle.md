# RCH.Forge.Dashboard — Version History
**Document Type:** Chronicle  
**Purpose:** Chronological record of project changes and evolution  
**Last Updated:** 2025-01-02  
**Related Documents:** ForgeTome.md, file5.md

---

## Overview
This document tracks all changes, updates, and milestones in the RCH.Forge.Dashboard project. All entries are timestamped and ordered in reverse chronological order (newest first).

---

## [v0.1.0] — 2025-01-02

### Added
- **Project restructuring** to align with RCH UI Forge standards
  - Created folder structure per file2.md and file3.md
  - Established `/Documentation` taxonomy per file4.md
  - Established `/Source/UI` and `/Source/Services` separation
  - Established `/Resources` with `/Images` and `/Data`
  - Added README.md to all folders

- **Documentation**
  - ForgeTome.md — Project overview and onboarding guide
  - NamingCanon.md — Naming conventions and rules
  - VersionHistory.chronicle.md — This document
  - README.md files for all documentation categories

- **Project configuration**
  - Updated project name from "TheForge" to "RCH.Forge.Dashboard"
  - Added compiler options: Option Strict On, Option Explicit On, Option Infer Off
  - Configured build rules per file5.md

- **Code structure**
  - Renamed Form1 to DashboardMainForm
  - Moved UI files to `/Source/UI`
  - Prepared `/Source/Services` for service layer implementation

### Rationale
Initial restructuring to establish a compliant foundation for the RCH UI Forge ecosystem. This ensures all future development follows established standards, reduces technical debt, and improves maintainability.

### Impact
- **Breaking Change:** Project renamed from "TheForge" to "RCH.Forge.Dashboard"
- **Breaking Change:** Form1 renamed to DashboardMainForm
- **Non-Breaking:** Folder structure established (no existing code affected)
- **Non-Breaking:** Documentation added (no code changes)

---

## [v0.0.1] — 2025-01-01 (Initial State)

### Description
Initial project creation as "TheForge" with default WinForms structure.

### Structure
- Default Form1.vb
- Default project configuration
- No documentation
- No folder structure

### Notes
This version did not follow RCH UI Forge standards and required complete restructuring.

---

## Version Numbering
This project follows semantic versioning: `MAJOR.MINOR.PATCH`

- **MAJOR** — Breaking changes or architectural shifts
- **MINOR** — New features or significant updates
- **PATCH** — Bug fixes or minor improvements

---

## Change Categories
All changes are categorized as:
- **Added** — New features or capabilities
- **Changed** — Modifications to existing features
- **Deprecated** — Features marked for removal
- **Removed** — Deleted features
- **Fixed** — Bug fixes
- **Security** — Security-related changes

---

## Deprecated Features
None yet. When features are deprecated, they will be documented here and moved to `/Documentation/Chronicle/DeprecatedArchive`.

---

## Future Milestones
- v0.2.0 — Implement DashboardMainForm UI layout
- v0.3.0 — Implement ModuleLoaderService
- v0.4.0 — Implement LoggingService
- v1.0.0 — First stable release

---

**All changes must be logged here before release.**
