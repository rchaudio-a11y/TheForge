# Branch-Architecture  
**Document Type:** Codex  
**Purpose:** Define rules for project structure, naming canon, modularity, dependencies, and architectural discipline  
**Created:** 2026-01-02  
**Last Updated:** 2026-01-02  
**Status:** Final  
**Character Count:** 8828  
**Related:** ForgeCharter.md, Branch-Coding.md, Branch-Documentation.md, ForgeAudit.md

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

# 7. Component Modularity

## 7.1 Reusable Components
Reusable components must:
- Be self-contained  
- Avoid external state  
- Avoid UI assumptions  
- Follow deterministic initialization  

---

## 7.2 Control Libraries
Controls must:
- Live in dedicated folders  
- Follow naming canon  
- Avoid Designer logic in main files  
- Avoid business logic in controls  

---

# 8. Common Mistakes
**Tags:** errors, namespace-issues, project-file, temp-files, troubleshooting

**Note:** See `Documentation/Chronicle/DevelopmentLog/IssueSummary.md` for full patterns and solutions.

## 8.1 Namespace Issues
❌ Using variable names that match keywords (`module`, `interface`)  
❌ Namespace doesn't match folder structure  
❌ Ambiguous type references without qualification  
✅ Fully qualify ambiguous names  
✅ Keep namespace aligned with folder path  
✅ Use descriptive suffixes for disambiguation  

## 8.2 Project File Sync
❌ Moving files without updating project file  
❌ Duplicate compile entries after refactoring  
❌ References to deleted temporary files  
✅ Verify project file before AND after file operations  
✅ Build immediately after project changes  
✅ Remove old references before adding new ones  

## 8.3 Temporary File Management
❌ Creating `*_old.vb`, `*_backup.vb`, `*_v091.vb` files  
❌ Leaving temporary files in repository  
✅ Use version control for backups, not suffixes  
✅ Delete temp files immediately after refactoring  
✅ Search with wildcards before declaring cleanup complete  

## 8.4 Dependency Management
❌ Circular dependencies between layers  
❌ UI depending on concrete service implementations  
❌ Missing validation for external dependencies  
✅ Interface-first architecture  
✅ Validate dependencies early (at load time)  
✅ No upward dependencies in layered architecture  

---

# 9. Structural Drift Prevention
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

# 9. Routing Behavior
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