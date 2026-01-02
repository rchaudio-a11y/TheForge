# RCH UI Forge — Standard Configuration Rules  
**Document Type:** Canon  
**Purpose:** Define global configuration rules for all projects  
**Last Updated:** 2025-12-31  
**Related Documents:** file2.md (Project Layout), file3.md (Dashboard Template), file4.md (Documentation Taxonomy)

---

## Overview
This document defines the mandatory configuration rules for all RCH UI Forge projects.  
These rules ensure consistency, determinism, clarity, and maintainability across the entire ecosystem.

All projects must follow these standards unless explicitly overridden by a higher-level Forge directive.

---

## Naming Rules
- All projects must use the prefix:  
  **RCH.Forge.[ModuleName]**
- Names must be explicit, descriptive, and deterministic.
- No default names such as:  
  - Form1  
  - Class1  
  - Module1  
- All files must reflect their purpose.

**Examples:**  
- RCH.Forge.AudioEngine  
- RCH.Forge.LayoutManager  
- RCH.Forge.Dashboard  

---

## Coding Standards
- **Language:** VB.NET (unless explicitly overridden).  
- **Compiler Options:**  
  - Option Strict On  
  - Option Explicit On  
  - Option Infer Off  
- No implicit conversions.  
- No hidden state or side effects.  
- All classes must have explicit names and documented purposes.  
- UI code must not contain business logic.  
- Services must be isolated and testable.

---

## Documentation Requirements
Every project must include the following documents inside `/Documentation`:

### **ForgeTome.md**
A high-level overview of the project, including purpose, scope, and onboarding.

### **NamingCanon.md**
Defines naming conventions specific to the project.

### **VersionHistory.chronicle.md**
A chronological log of changes, following the Chronicle rules in file4.md.

All documentation must follow the taxonomy defined in **file4.md**.

---

## Build Rules
- No warnings allowed during build.  
- All dependencies must be explicit.  
- No circular references between projects.  
- All modules must be independently buildable.  
- All resources must be referenced deterministically.

---

## Testing Rules
If a `/Tests` folder exists, it must contain:

### **/Unit**
Unit tests for individual components.

### **/Integration**
Tests validating interactions between modules.

Tests must:
- Be deterministic  
- Avoid external dependencies  
- Follow Arrange–Act–Assert  

---

## Summary
These configuration rules form the foundation of the RCH UI Forge ecosystem.  
All projects must adhere to these standards to ensure clarity, maintainability, and architectural consistency.
