# Branch-Architecture  
**Document Type:** Codex  
**Purpose:** Define rules for project structure, naming canon, modularity, dependencies, and architectural discipline  
**Created:** 2025-01-02  
**Last Updated:** 2026-01-03  
**Status:** Final  
$113909  
**Related:** ForgeCharter.md, Branch-Coding.md, Branch-Documentation.md, ForgeAudit.md, CONSTITUTION.md

---

# 1. Purpose
The Architecture Branch defines the structural rules that govern how projects, files, modules, namespaces, and components are organized within the RCH UI Forge ecosystem.

It ensures:
- Determinism  
- Modularity  
- Predictability  
- Maintainability  
- Zero drift  

This branch governs **structure**, not code content or documentation.

---

# 2. Scope
The Architecture Branch applies to:

- Project folder structure  
- File placement  
- Naming canon  
- Module boundaries  
- Dependency rules  
- Layering rules  
- Partial class boundaries  
- Designer/main file separation  
- Service placement  
- Reusable component structure  

This branch does **not** apply to:
- Code content (Coding Branch)  
- Documentation content (Documentation Branch)  
- Audit behavior (ForgeAudit)  

---

# 3. Authoritative Sources
The Architecture Branch draws from:

- ForgeCharter (universal governance)  
- RCH UI Forge naming canon  
- Modular architecture principles  
- Deterministic UI design principles  
- Separation-of-responsibility rules  

No other source may override this branch.

---

# 4. Structural Rules
**Tags:** folder-structure, file-placement, modularity, explicit-design

## 4.1 Project Structure Must Be Explicit
The Forge must never assume:
- Folder layouts  
- File locations  
- Naming conventions  
- Layering patterns  

All structure must be:
- Explicit  
- Deterministic  
- User-defined or governed by this branch  

---

## 4.2 No Implicit Folder Creation
The Forge must not:
- Create folders implicitly  
- Move files implicitly  
- Rename folders implicitly  
- Restructure the project implicitly  

All structural changes require explicit user instruction.

---

## 4.3 Partial Class Boundaries
Partial classes must follow strict separation:

- `.Designer.vb` → layout, control declarations, initialization  
- `.vb` → user logic, event handlers, business logic  

The Forge must never:
- Move logic into the Designer  
- Move layout into the main file  
- Merge partial classes  
- Split partial classes without explicit instruction  

---

## 4.4 Namespace Rules
Namespaces must:
- Match folder structure  
- Be explicit and descriptive  
- Avoid vague names (Common, Utils, Helpers)  
- Follow Forge‑themed naming where appropriate  
- Avoid deep nesting unless justified  

**Examples:**

```vb
' ✅ CORRECT: Namespace matches folder path
' File: Source/Services/Implementations/ModuleLoaderService.vb
Namespace RCH.Forge.Services.Implementations
    Public Class ModuleLoaderService

' File: Source/Models/ModuleMetadata.vb
Namespace RCH.Forge.Models
    Public Class ModuleMetadata

' ❌ WRONG: Namespace doesn't match folder
' File: Source/Models/ModuleMetadata.vb
Namespace RCH.Forge.Core  ' Should be .Models
    Public Class ModuleMetadata

' ❌ WRONG: File in wrong folder for namespace
' File: Source/Core/ModuleMetadata.vb  ' Wrong folder
Namespace RCH.Forge.Models  ' Correct namespace, wrong location
    Public Class ModuleMetadata
```  

---

## 4.5 Module & Service Placement
Modules and services must:
- Live in dedicated folders  
- Follow explicit naming canon  
- Avoid UI dependencies  
- Avoid circular dependencies  
- Be reusable across the project  

Services must be:
- Stateless when possible  
- Interface-driven  
- Dependency-injected  

---

# 5. Naming Canon
**Tags:** naming-conventions, pascalcase, descriptive-names, anti-patterns

## 5.1 General Naming Rules
All names must be:
- Explicit  
- Descriptive  
- Deterministic  
- Purpose-driven  

Avoid:
- Helper  
- Manager  
- Utils  
- Misc  
- Generic prefixes or suffixes  

**Examples:**

```vb
' ✅ CORRECT: Explicit, descriptive names
Namespace RCH.Forge.Services.Module
    Public Class ModuleLoaderService
    Public Class ModuleMetadataReader
    Public Interface IModuleLifecycleManager

' ✅ CORRECT: Disambiguation with suffixes
Dim moduleInterface As IModule  ' Not "module" (keyword conflict)
Dim loggingService As ILoggingService  ' Clear purpose

' ❌ WRONG: Vague, generic names
Namespace RCH.Forge.Common  ' What's "common"?
    Public Class Helper  ' Helper for what?
    Public Class Utils  ' Which utilities?
    Public Class Manager  ' Manages what?

' ❌ WRONG: Keyword conflicts
Dim module As IModule  ' "module" is VB keyword
Dim interface As Type  ' "interface" is keyword
```  

---

## 5.2 File Naming Rules
File names must:
- Match the primary class or module  
- Use PascalCase  
- Avoid abbreviations  
- Avoid ambiguous names  
- Reflect purpose, not implementation  

Examples:
- `AudioService.vb`  
- `WorkoutTimer.Designer.vb`  
- `WorkoutTimer.vb`  
- `DatabaseConnector.vb`  

---

## 5.3 Folder Naming Rules
Folders must:
- Reflect architectural purpose  
- Use PascalCase  
- Avoid vague names  
- Avoid nested ambiguity  

Examples:
- `Services`  
- `Modules`  
- `UI`  
- `Controls`  
- `Data`  
- `Infrastructure`  

---

# 6. Dependency Rules
**Tags:** dependencies, layering, interfaces, circular-dependencies, architecture

## 6.1 No Circular Dependencies
The Forge must detect and prevent:
- Circular references  
- Cyclic module relationships  
- UI → Service → UI loops  

---

## 6.2 Layering Rules
UI may depend on:
- Services  
- Modules  

Services may depend on:
- Modules  
- Infrastructure  

Modules may depend on:
- Nothing above them  

Infrastructure may depend on:
- External libraries  
- System APIs  

No layer may depend on a higher layer.

---

## 6.3 Interface-First Architecture
All services must:
- Expose explicit interfaces  
- Avoid implicit contracts  
- Avoid leaking internal state  

Consumers must depend on:
- Interfaces  
Not:
- Concrete implementations  

---

# 7. Data Persistence Architecture
**Tags:** database, access, configuration, data-store

## 7.1 Database Technology
The Forge uses Microsoft Access for optional data persistence:
- **Format:** `.accdb` (Access 2007+ format)
- **Location:** `/Resources/Data/` directory
- **Purpose:** Configuration, logging, module tracking, historical data

## 7.2 Database Design Principles
- **Optional:** Not required for core functionality
- **Portable:** Single-file database (xcopy deployment)
- **Graceful Degradation:** Falls back to file-based if Access unavailable
- **Windows-Integrated Security:** No separate credentials
- **No External Server:** No database server dependencies

## 7.3 Database Naming
- Use descriptive names: `ForgeData.accdb`, `ForgeLog.accdb`
- Follow Forge naming canon (no abbreviations)
- Location: Always in `/Resources/Data/`

## 7.4 File-Based Fallback
Current implementation uses file-based configuration:
- `.config` files for module configuration
- Text-based logging via `ILoggingService`
- Must remain functional if Access not available

---

# 8. Component Modularity

## 8.1 Reusable Components
Reusable components must:
- Be self-contained  
- Avoid external state  
- Avoid UI assumptions  
- Follow deterministic initialization  

---

## 8.2 Control Libraries
Controls must:
- Live in dedicated folders  
- Follow naming canon  
- Avoid Designer logic in main files  
- Avoid business logic in controls  

---

# 9. Cross-Platform IDE Compatibility
**Tags:** visual-studio, vscode, github, codespaces, project-files

## 9.1 Objective
Maintain compatibility across:
- Visual Studio (Windows)
- Visual Studio Code
- GitHub Codespaces
- Other .NET-compatible editors

## 9.2 File Synchronization Requirements

### When Adding/Removing Files
AI must update:
1. **`.vbproj`** - Add/remove `<Compile>`, `<EmbeddedResource>`, or `<None>` entries
2. **`.sln`** - Update project references if project added/removed
3. **`.slnx`** - Keep synchronized with `.sln` for cross-platform support

### When Modifying Designer-Based UI Components
AI must update:
1. **`.Designer.vb`** - Control declarations, initialization, layout
2. **`.resx`** - Embedded resources, strings, images
3. **`.vbproj`** - Ensure proper `<DependentUpon>` relationships

### When Adding/Removing Project References
AI must update:
1. **`.vbproj`** - Add/remove `<ProjectReference>` or `<Reference>` entries
2. **`.sln`** - Update project dependency hierarchy if needed

## 9.3 File Relationship Patterns (Must Maintain)

```xml
<!-- Code files -->
<Compile Include="Source\UI\DashboardMainForm.vb">
  <SubType>Form</SubType>
</Compile>
<Compile Include="Source\UI\DashboardMainForm.Designer.vb">
  <DependentUpon>DashboardMainForm.vb</DependentUpon>
</Compile>

<!-- Resource files -->
<EmbeddedResource Include="Source\UI\DashboardMainForm.resx">
  <DependentUpon>DashboardMainForm.vb</DependentUpon>
</EmbeddedResource>
```

## 9.4 Common Compatibility Issues to Avoid
- ❌ Adding files to filesystem without updating `.vbproj`
- ❌ Modifying `.sln` without updating `.slnx` (or vice versa)
- ❌ Breaking `<DependentUpon>` relationships (orphaned Designer/resource files)
- ❌ Adding project references without updating solution dependencies
- ❌ Changing file paths without updating all referencing files

## 9.5 Validation Requirements
After any structural change:
- ✅ Verify project builds in multiple editors
- ✅ Ensure `.sln` and `.slnx` remain synchronized
- ✅ Validate `<DependentUpon>` relationships correct
- ✅ Confirm resource files properly embedded

---

# 10. Module Lifecycle Architecture
**Tags:** modules, imodule, lifecycle, dependency-injection

## 10.1 Module Lifecycle Phases
All modules must follow this exact lifecycle:

1. **Discovery** - Filesystem scan for module assemblies
2. **Load** - Assembly load, type discovery
3. **Initialize** - Service injection via `Initialize()` method
4. **Configure** - Optional configuration load via `LoadConfiguration()`
5. **Execute** - Primary functionality via `Execute()` method
6. **OnUnload** - Cleanup via `OnUnload()` method
7. **Dispose** - Resource release via `Dispose()` method

## 10.2 Module Requirements
All modules must:
- Implement `IModule` interface
- Implement `IDisposable` interface
- Accept service dependencies via `Initialize(loggingService)`
- Support optional configuration via `LoadConfiguration(config)`
- Be self-contained (no shared state between modules)
- Be stateless where possible
- Not depend on execution order of other modules

## 10.3 Module Configuration
Modules may optionally:
- Have `.config` file (key=value format)
- Have database-backed configuration (future)
- Request configuration via `LoadConfiguration()`
- Validate configuration before `Execute()`

## 10.4 Module Disposal
Modules must:
- Implement proper `Dispose()` pattern
- Release all resources in `Dispose()`
- Set `_disposed` flag to prevent double-disposal
- Call `GC.SuppressFinalize(Me)` after disposal
- Log disposal via `ILoggingService`

---

# 11. Common Mistakes
**Tags:** errors, namespace-issues, project-file, temp-files, troubleshooting

**Note:** See `Documentation/Chronicle/DevelopmentLog/IssueSummary.md` for full patterns and solutions.

## 11.1 Namespace Issues
❌ Using variable names that match keywords (`module`, `interface`)  
❌ Namespace doesn't match folder structure  
❌ Ambiguous type references without qualification  
✅ Fully qualify ambiguous names  
✅ Keep namespace aligned with folder path  
✅ Use descriptive suffixes for disambiguation  

## 11.2 Project File Sync
❌ Moving files without updating project file  
❌ Duplicate compile entries after refactoring  
❌ References to deleted temporary files  
✅ Verify project file before AND after file operations  
✅ Build immediately after project changes  
✅ Remove old references before adding new ones  

## 11.3 Temporary File Management
❌ Creating `*_old.vb`, `*_backup.vb`, `*_v091.vb` files  
❌ Leaving temporary files in repository  
✅ Use version control for backups, not suffixes  
✅ Delete temp files immediately after refactoring  
✅ Search with wildcards before declaring cleanup complete  

## 11.4 Dependency Management
❌ Circular dependencies between layers  
❌ UI depending on concrete service implementations  
❌ Missing validation for external dependencies  
✅ Interface-first architecture  
✅ Validate dependencies early (at load time)  
✅ No upward dependencies in layered architecture  

---

# 12. Structural Drift Prevention
Before executing any structural task, the Forge must:

- Detect folder drift  
- Detect naming drift  
- Detect file placement drift  
- Detect partial class drift  
- Detect namespace drift  
- Detect dependency drift  

If drift is detected:
- Summarize the issue  
- Request confirmation  
- Never auto-correct silently  

---

# 13. Routing Behavior
The Architecture Branch is invoked when:

- The user requests structural changes  
- The user requests file placement rules  
- The user requests naming canon decisions  
- The user requests module or service organization  
- The user requests dependency or layering decisions  
- The user requests project structure changes  

Routing:
If task involves structure or design → Use Branch-Architecture
Otherwise → Delegate to appropriate branch
ForgeCharter always governs the process

---

# End of Branch-Architecture.md