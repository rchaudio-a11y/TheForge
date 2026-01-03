# RCH UI Forge — Dashboard Project Template  
**Document Type:** Scriptorium  
**Purpose:** Define the structure and layout of the Dashboard project  
**Last Updated:** 2025-12-31  
**Related Documents:** file4.md (Documentation Taxonomy), file5.md (Configuration Rules)

---

## Overview
The Dashboard project is a WinForms or WPF application used to test, orchestrate, and interact with modules in the RCH UI Forge ecosystem.  
It serves as a centralized test harness and demonstration environment.

This template defines:
- The required folder structure  
- The required UI layout  
- The architectural rules  
- The documentation expectations  

---

## Project Structure

/RCH.Forge.Dashboard
    /Documentation
    /Source
        /UI
        /Services
        /Modules   (optional)
    /Resources
        /Images
        /Data


---

## Folder Purposes & Rules

### **/Documentation**
Contains all Dashboard-specific documentation.  
Must follow the taxonomy defined in **file4.md**.

**Examples:**  
- DashboardTome.md  
- DashboardVersionHistory.chronicle.md  
- DashboardNamingCanon.md  

---

### **/Source**
Contains all source code for the Dashboard.

#### **/UI**
Forms, controls, and UI logic.

**Examples:**  
- DashboardMainForm.vb  
- ModuleListControl.vb  
- LogPanelControl.vb  

#### **/Services**
Non-UI logic that supports the Dashboard.

**Examples:**  
- ModuleLoaderService.vb  
- LoggingService.vb  
- DashboardStateService.vb  

#### **/Modules** (optional)
Dashboard-specific modules or test harness components.  
Not for production modules — those belong in their own projects.

---

### **/Resources**
Static assets used by the Dashboard.

#### **/Images**
Icons, UI assets, branding.

#### **/Data**
Sample data, configuration files, JSON, XML.

---

## DashboardMainForm Layout

The main form must include the following regions:

### **Left Panel — Module List**
- Displays available modules  
- Allows selecting a module to test  
- Must support dynamic population  

### **Bottom Panel — Log Output**
- Displays logs, events, and diagnostic messages  
- Must support clear, copy, and filter operations  

### **Center Panel — Test Area**
- Hosts module-specific UI  
- Must support dynamic loading/unloading  
- Must be visually isolated from the Dashboard UI  

### **Optional — Status Bar**
- Displays system state  
- May show module count, active module, or status messages  

---

## Architectural Rules

- **No business logic in the UI layer.**  
  All logic must be delegated to `/Services`.

- **Module loading must be abstracted.**  
  Use interfaces or service classes to load modules.

- **Dashboard must run standalone.**  
  It must not require external modules to start.

- **UI must be deterministic and extendable.**  
  No hidden behaviors or implicit state.

- **No default Form1.**  
  The main form must be named:  
  `DashboardMainForm`

---

## Documentation Requirements

The Dashboard project must include:

- A **DashboardTome.md** (overview + onboarding)  
- A **DashboardNamingCanon.md** (naming rules)  
- A **DashboardVersionHistory.chronicle.md** (change log)  

All documentation must follow the taxonomy in **file4.md**.

---

## Summary

This template defines the required structure and rules for the RCH UI Forge Dashboard project.  
It ensures consistency, modularity, and clarity across all Forge solutions.
