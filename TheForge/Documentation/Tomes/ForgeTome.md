# RCH.Forge.Dashboard — Forge Tome
**Document Type:** Tome  
**Purpose:** High-level overview and onboarding guide for the Dashboard project  
**Last Updated:** 2025-01-02 (v0.9.8)  
**Current Version:** v0.9.7 (92% compliance)  
**Related Documents:** DevelopmentLog.index.md, Phase1_Completion_Report.md, file2.md, File6.md

---

## Overview
The **RCH.Forge.Dashboard** is a WinForms application designed to discover, load, execute, and manage modules in the RCH UI Forge ecosystem. It serves as a centralized module orchestration platform with hot-reload capabilities, dependency resolution, and comprehensive logging.

**Key Features:**
- Dynamic module discovery and loading via reflection
- Hot-reload capability for rapid module development
- Dependency resolution with circular dependency detection
- Configuration file support (.config files)
- Real-time log output with filtering and search
- Modular UserControl-based UI architecture (v0.8.1)
- Deterministic programmatic layout
- Performance-optimized operations (v0.9.1)

**Current Status:**
- Version: v0.9.7
- Compliance: 92%
- Phase 1 Complete: ? (Cleanup & Documentation)
- Phase 2 In Progress: ?? (Structure)

---

## Purpose
The Dashboard provides:
1. **Module Testing Environment** - Load and execute modules in isolation
2. **Development Tool** - Hot-reload for rapid iteration
3. **Dependency Orchestration** - Automatic dependency validation and ordering
4. **Configuration Management** - Load module-specific .config files
5. **Diagnostic Platform** - Real-time logging with filtering
6. **Reference Implementation** - Demonstrates Forge architecture principles

---

## Getting Started

### Prerequisites
- .NET 8.0 Windows (net8.0-windows)
- Windows OS with WinForms support
- Visual Studio 2022 or later (recommended)

### Building the Project
1. Open `TheForge.sln` in Visual Studio
2. Restore NuGet packages (if any)
3. Build the solution (Ctrl+Shift+B)
4. Run the Dashboard (F5)

**Build Output:** `RCH.Forge.Dashboard.dll` in `bin\Debug\net8.0-windows\`

### First Run
On first launch, the Dashboard displays:
- **Left Panel:** Module list (initially empty - no modules discovered)
- **Right Panel:** Module details (shows metadata when module selected)
- **Bottom Panel:** Log output with startup messages and filtering controls
- **Center Panel:** Test area (placeholder for future module UI hosting)

**Initial Log Messages:**
```
[INFO] Dashboard started
[INFO] Discovering modules...
[INFO] Modules directory created
[INFO] Found 0 DLL file(s) in Modules directory
[INFO] Discovered 0 module(s)
```

---

## Architecture

### Project Structure (v0.9.7)

```
TheForge/
??? Source/
?   ??? Core/                    ? v0.9.7 (Foundational models)
?   ?   ??? ModuleMetadata.vb
?   ?   ??? ModuleConfiguration.vb
?   ?   ??? README.md
?   ??? Models/                  (Reserved for auxiliary models)
?   ??? Modules/
?   ?   ??? Interfaces/
?   ?       ??? IModule.vb
?   ?       ??? IModuleConfiguration.vb
?   ?       ??? ModuleDependencyAttribute.vb
?   ?       ??? README.md
?   ??? Services/
?   ?   ??? Implementations/
?   ?   ?   ??? ModuleLoaderService.vb
?   ?   ?   ??? LoggingService.vb
?   ?   ?   ??? README.md
?   ?   ??? Interfaces/
?   ?       ??? IModuleLoaderService.vb
?   ?       ??? ILoggingService.vb
?   ?       ??? README.md
?   ??? UI/
?       ??? Controls/            ? v0.8.1 (UserControl architecture)
?       ?   ??? ModuleListControl.vb
?       ?   ??? ModuleDetailsControl.vb
?       ?   ??? LogOutputControl.vb
?       ?   ??? TestAreaControl.vb
?       ?   ??? README.md
?       ??? DashboardMainForm.vb
?       ??? DashboardMainForm.Designer.vb
?       ??? README.md
??? Resources/
?   ??? Images/
?   ??? Data/
??? Documentation/
?   ??? Chronicle/               ? Development history
?   ?   ??? DevelopmentLog/      (v0.1.0 - v0.9.7 entries)
?   ?   ??? DevelopmentLog.index.md
?   ?   ??? Phase1_Completion_Report.md
?   ?   ??? Roadmap_v092_to_v100.md
?   ??? Tomes/                   (This file)
?   ??? ... (other categories)
??? Modules/                     ? Runtime module directory
    ??? (Module DLLs go here)
```

---

### UI Layer (`/Source/UI`)

**DashboardMainForm.vb** (Main Form)
- Entry point form
- Orchestrates all UserControls
- Manages service instances
- Event-driven communication with controls
- **Architecture:** Programmatic layout (v0.8.1) for determinism

**UserControls** (v0.8.1 - Modular Architecture)
- **ModuleListControl** - Module list with action buttons (Run, Unload, Reload, Refresh)
- **ModuleDetailsControl** - Displays module metadata (file, type, dependencies, config)
- **LogOutputControl** - Log display with level filter and search (v0.8.2 enhanced)
- **TestAreaControl** - Placeholder for module UI hosting

**Design Pattern:**
- Controls expose **methods** for parent to update them
- Controls raise **events** for parent to handle
- No business logic in controls (only presentation)
- Deterministic layout via Dock and TableLayoutPanel

**Example:**
```vb
' In DashboardMainForm
Private WithEvents moduleListControl As UI.Controls.ModuleListControl

Private Sub HandleModuleSelected(sender As Object, e As ModuleSelectedEventArgs) _
    Handles moduleListControl.ModuleSelected
    ' React to user selection
End Sub

' Update control
moduleListControl.LoadModules(discoveredModules)
```

---

### Services Layer (`/Source/Services`)

**ModuleLoaderService** (v0.8.0 enhanced)
- **Discovery:** Scans `Modules/` directory for DLL files
- **Loading:** Uses reflection to load assemblies and instantiate IModule types
- **Dependency Resolution:** Validates dependencies and detects circular references
- **Hot-Reload:** Unload and reload modules without restarting Dashboard
- **Configuration:** Loads .config files and applies to modules
- **Caching:** Maintains module metadata and instances (v0.9.1 optimized)

**LoggingService**
- **Centralized Logging:** Single point for all log messages
- **Log Levels:** Info, Warning, Error
- **Event-Driven:** Raises LogMessageReceived for real-time display
- **Filtering:** Supports level filtering and text search (v0.8.2)
- **Performance:** StringBuilder optimization for large logs (v0.9.1)

---

### Core Models (`/Source/Core`) - v0.9.7

**ModuleMetadata.vb**
- Represents metadata for a discovered module
- Properties: FileName, DisplayName, TypeName, IsLoaded, LastLoadedTime, Dependencies, CachedInstance
- Used throughout services and UI

**ModuleConfiguration.vb**
- Represents key-value configuration loaded from .config files
- Methods: SetValue, GetValue, GetValueOrDefault
- Passed to modules via IModule.LoadConfiguration()

---

### Module Interface (`/Source/Modules/Interfaces`)

**IModule** (Core Interface)
```vb
Public Interface IModule
    Inherits IDisposable
    
    Sub Initialize(loggingService As ILoggingService)
    Sub LoadConfiguration(config As IModuleConfiguration)
    Sub Execute()
    Sub OnUnload()
End Interface
```

**IModuleConfiguration**
- Configuration contract for modules
- GetValue/GetValueOrDefault methods

**ModuleDependencyAttribute** (v0.8.0)
- Apply to module class to declare dependencies
- Example: `<ModuleDependency("OtherModule.OtherClass")>`

---

## Key Features by Version

### v0.8.0 - Advanced Features
- Module configuration support (.config files)
- Dependency resolution
- Circular dependency detection
- Hot-reload capability

### v0.8.1 - UI Modularization
- Refactored UI into 4 UserControls
- Event-driven communication
- Deterministic programmatic layout
- Enhanced module details (dependencies, config path)

### v0.8.2 - Enhanced UX
- Auto-filtering on dropdown selection
- "All" level option for complete log view
- Enter key support for search
- Centralized filter logic

### v0.9.1 - Performance Improvements
- Directory listing cache (2-second TTL)
- StringBuilder optimization for log rendering
- Reload timing diagnostics

### v0.9.2-v0.9.6 - Phase 1 Complete (91% Compliance)
- Documentation consolidation
- Temporary file cleanup
- README coverage (100%)
- VersionHistory deprecated
- Formal validation

### v0.9.7 - Core Folder Structure
- Created `/Source/Core` for foundational models
- Aligned with file2.md specifications
- Project structure improved to 85%

---

## Creating Modules

See **[CreatingModules.md](CreatingModules.md)** for complete guide.

**Quick Start:**
1. Create VB.NET Class Library (.NET 8.0)
2. Reference RCH.Forge.Dashboard.dll
3. Implement IModule interface
4. Build and copy DLL to Dashboard's Modules folder
5. Run Dashboard - module appears in list

**Minimal Module:**
```vb
Imports TheForge.Modules.Interfaces
Imports TheForge.Services.Interfaces

Public Class SampleModule
    Implements IModule

    Private _loggingService As ILoggingService

    Public Sub Initialize(loggingService As ILoggingService) _
        Implements IModule.Initialize
        _loggingService = loggingService
        _loggingService.LogInfo("SampleModule initialized")
    End Sub

    Public Sub Execute() Implements IModule.Execute
        _loggingService.LogInfo("Hello from the Forge!")
    End Sub

    ' ... (Dispose, LoadConfiguration, OnUnload implementations)
End Class
```

---

## Key Principles

This Dashboard adheres to the **Forge Prime Directives** (File6.md):

### 1. Determinism
All UI behavior is explicit and predictable:
- Programmatic layout with Dock/TableLayoutPanel
- Explicit control ordering
- No designer-generated layout (by design in v0.8.1)

### 2. Modularity
Components are self-contained and reusable:
- UserControls encapsulate UI concerns
- Services handle business logic
- Interfaces enable dependency injection

### 3. Onboarding-First
Documentation reduces cognitive load:
- 100% README coverage (v0.9.4)
- Comprehensive development log (v0.1.0-v0.9.7)
- Clear architectural documentation

### 4. Architecture Discipline
Clear separation of concerns:
- UI layer: presentation only
- Service layer: business logic
- Core: foundational models
- No business logic in UI controls

### 5. Explicit Over Implicit
All types and behaviors are explicit:
- Option Strict On, Option Explicit On, Option Infer Off
- No implicit type conversions
- All dependencies injected

---

## Documentation Navigation

The Dashboard documentation follows the RCH UI Forge Documentation Taxonomy (file4.md):

**Chronicle** (`/Documentation/Chronicle`)
- **DevelopmentLog.index.md** - Master index of all milestones
- **DevelopmentLog/** - Individual milestone entries (v010-v097)
- **Phase1_Completion_Report.md** - Phase 1 summary and metrics
- **Roadmap_v092_to_v100.md** - Complete roadmap to v1.0.0
- **Progress_Checklist.md** - Quick reference progress tracking

**Tomes** (`/Documentation/Tomes`)
- **ForgeTome.md** (this file) - Overview and onboarding
- **CreatingModules.md** - Module development guide

**Source Code READMEs** (100% coverage as of v0.9.4)
- `/Source/Core/README.md` - Core models documentation
- `/Source/Services/Interfaces/README.md` - Service contracts
- `/Source/Services/Implementations/README.md` - Service implementations
- `/Source/Modules/Interfaces/README.md` - Module interface documentation
- `/Source/UI/Controls/README.md` - UserControl documentation

**Compliance & Planning**
- **ForgeSolutionRuleComplianceAudit_Summary.md** - Current compliance status (92%)
- **Roadmap_v092_to_v100.md** - Detailed milestone planning

---

## Development Workflow

### Adding a New Feature
1. Plan milestone in Roadmap
2. Create feature branch (if needed)
3. Implement feature with tests
4. Update relevant README files
5. Create chronicle entry (vXXX.md)
6. Update compliance tracking
7. Commit with structured message

### Module Development Cycle
1. **Develop:** Create module in separate project
2. **Build:** Compile to DLL
3. **Deploy:** Copy to Dashboard's Modules folder
4. **Test:** Run in Dashboard, check logs
5. **Iterate:** Hot-reload for rapid changes (Reload button)

### Hot-Reload Workflow
1. Make changes to module code
2. Build module project
3. In Dashboard: Select module ? Click "Reload"
4. Module unloaded, reloaded, and ready to Execute
5. Check logs for reload timing (v0.9.1 diagnostics)

---

## Current Status & Roadmap

**Current Version:** v0.9.7  
**Current Compliance:** 92%  
**Target (v1.0.0):** 95%+

**Completed:**
- ? Phase 1: Cleanup & Documentation (v0.9.1-v0.9.6) - 91%
- ?? Phase 2: Structure (v0.9.7-v0.9.8) - 92% (in progress)

**Remaining:**
- v0.9.8: Update ForgeTome (this milestone!) ? 93%
- v0.9.9: Document Policies ? 95%
- v1.0.0: Production Release ? 95%+

**See:** Roadmap_v092_to_v100.md for complete milestone details

---

## Troubleshooting

**No modules discovered:**
- Verify `Modules/` folder exists in Dashboard bin directory
- Check log output for discovery messages
- Ensure DLL files are present

**Module fails to load:**
- Verify module implements IModule interface
- Check namespace matches: `TheForge.Modules.Interfaces.IModule`
- Ensure module targets net8.0
- Review error messages in log output

**Build errors after updating:**
- Clean solution (Build ? Clean Solution)
- Rebuild all projects
- Check that all file paths in .vbproj are correct

**Performance issues:**
- Check directory cache messages in logs (v0.9.1)
- Verify no excessive module reloads
- Review log filtering performance

---

## Support & Contribution

**Getting Help:**
- See `/Documentation/Tomes` for detailed guides
- See `/Documentation/Chronicle/DevelopmentLog.index.md` for version history
- Review README files in each source folder

**Contributing:**
- Follow Forge architecture principles (File6.md)
- Maintain Option Strict On, Option Explicit On
- Update README files for new features
- Create chronicle entries for milestones
- Write comprehensive XML documentation

**Code Standards:**
- Explicit typing (no implicit conversions)
- Dependency injection for services
- Event-driven UI communication
- Deterministic layout (Dock/TableLayoutPanel)
- Comprehensive logging

---

## Next Steps

1. **Review Architecture:** Explore the UserControl-based UI (v0.8.1)
2. **Create a Module:** Follow CreatingModules.md guide
3. **Explore Features:** Try hot-reload, configuration files, dependencies
4. **Read Chronicle:** Review DevelopmentLog.index.md for development history
5. **Check Roadmap:** See what's coming in v0.9.8-v1.0.0

---

**Welcome to the Forge. Version v0.9.7 (92% compliance). Phase 2 in progress.**
