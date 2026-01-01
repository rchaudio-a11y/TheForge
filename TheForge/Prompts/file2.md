# RCH UI Forge — Project File Layout Template  
**Document Type:** Scriptorium  
**Purpose:** Define the folder structure for a new project  
**Last Updated:** 2025-12-31  
**Related Documents:** file4.md (Documentation Taxonomy), file5.md (Configuration Rules)

---

## Overview
This template defines the standard folder structure for any new RCH UI Forge project.  
It does not generate documentation content; it only defines the layout and rules for the project’s physical structure.

All documentation content must follow the taxonomy defined in **file4.md**.  
All naming and configuration rules must follow **file5.md**.

---

## Folder Structure
/ProjectName
    /Documentation
    /Source
        /Core
        /Modules
        /UI
    /Resources
        /Images
        /Data
        /Localization
    /Tests
        /Unit
        /Integration
---

## Folder Purposes & Rules

### **/Documentation**
Holds all project documentation.  
Must follow the taxonomy defined in **file4.md**.  
No code files belong here.

**Examples:**  
- ForgeTome.md  
- VersionHistory.chronicle.md  
- NamingCanon.md  

---

### **/Source**
Contains all source code for the project.

#### **/Core**
Foundational logic, shared utilities, base classes.  
No UI code.

**Examples:**  
- Validation services  
- Base models  
- Shared interfaces  

#### **/Modules**
Reusable components, feature modules, or subsystems.  
Each module should be self-contained.

**Examples:**  
- AudioEngine  
- LayoutManager  
- ControlSurface  

#### **/UI**
Forms, controls, views, and UI-specific logic.

**Examples:**  
- MainForm.vb  
- CustomControl.vb  

---

### **/Resources**
Static assets used by the project.

#### **/Images**
Icons, UI assets, branding.

#### **/Data**
JSON, XML, CSV, or sample datasets.

#### **/Localization**
Language files, resource dictionaries.

---

### **/Tests**
Optional folder for automated testing.

#### **/Unit**
Unit tests for individual components.

#### **/Integration**
Tests that validate interactions between modules.

---

## README.md Requirements
Every folder listed above must contain a README.md describing:

- **Purpose**  
- **Rules for what belongs here**  
- **Examples of valid files**  
- **Cross-references** (e.g., “See /Documentation/Codex for API docs”)  

---

## Requirements Summary
- No placeholder code unless explicitly requested.  
- All documentation must follow **file4.md**.  
- All naming and configuration must follow **file5.md**.  
- Structure must be deterministic, modular, and Forge-themed.  