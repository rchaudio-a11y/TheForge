# Branch-Coding  
**Document Type:** Codex  
**Purpose:** Define rules for code generation, modification, UI behavior, and implementation discipline  
**Created:** 2025-01-02  
**Last Updated:** 2026-01-03  
**Status:** Final  
**Character Count:** 10876  
**Related:** ForgeCharter.md, Branch-Architecture.md, Branch-Documentation.md, ForgeAudit.md, CONSTITUTION.md

---

# 1. Purpose
The Coding Branch defines all rules governing code generation and modification within the RCH UI Forge ecosystem.  
It ensures determinism, clarity, modularity, and consistency across all implementation tasks.

This branch governs **code only**.  
It does not define documentation rules, architectural rules, or audit rules.

---

# 2. Scope
The Coding Branch applies to:

- VB.NET code generation  
- UI control creation  
- Designer file behavior  
- Services and modules  
- Event handlers  
- Naming in code  
- Error handling  
- Layout logic  
- Refactoring  
- Code cleanup  
- Implementation discipline  

This branch does **not** apply to:

- Documentation (Documentation Branch)  
- Project structure (Architecture Branch)  
- Auditing (ForgeAudit)  

---

# 3. Authoritative Sources
The Coding Branch draws from:

- ForgeCharter (universal governance)  
- Implementation discipline defined by the Forge ethos  
- Coding standards established across the Forge ecosystem  
- UI determinism principles  
- Modular service design principles  

No other source may override this branch.

---

# 4. Coding Rules
**Tags:** vb.net, compiler-settings, option-strict, code-quality

## 4.1 Language & Compiler Rules
All generated or modified code must:

- Use **VB.NET**  
- Use the following compiler settings:  
  - `Option Strict On`  
  - `Option Explicit On`  
  - `Option Infer Off`  
- Avoid implicit conversions  
- Avoid hidden state  
- Avoid magic strings and magic numbers  
- Avoid commented‑out code  
- Avoid unused code  
- Avoid TODOs without owners  

---

## 4.2 Class & Module Rules
All classes must:

- Have explicit, descriptive names  
- Include a summary of purpose  
- Be deterministic and predictable  
- Avoid vague names (Helper, Manager, Utils, etc.)  
- Follow Forge‑themed naming where appropriate  
- Expose explicit, documented interfaces  
- Avoid leaking internal state  

Modules must:

- Have a single, clear purpose  
- Be reusable across projects  
- Avoid UI dependencies  
- Avoid circular dependencies  

---

## 4.3 Service Rules (Enhanced)
Services must:

- Be stateless when possible  
- Be testable  
- Avoid side effects  
- Avoid ad‑hoc instantiation  
- Use dependency injection  
- Use explicit interfaces  
- **Be thread-safe by default**
- **Be injected into modules and UI components**

### Service Characteristics (Constitutional Principles)
- **Interface-first design** - Always define interface before implementation
- **Stateless where possible** - Minimize mutable state to improve testability
- **Thread-safe by default** - Assume multi-threaded usage in all service implementations
- **Explicit dependencies** - All dependencies via constructor or Initialize() method
- **Injected everywhere** - Services injected into modules via Initialize(), into UI via constructor/properties

**Examples:**

```vb
' ✅ CORRECT: Service with interface-first design
Public Interface ILoggingService
    Sub LogInfo(message As String)
    Sub LogError(ex As Exception)
End Interface

Public Class LoggingService
    Implements ILoggingService
    
    ' Thread-safe, stateless implementation
    Public Sub LogInfo(message As String) Implements ILoggingService.LogInfo
        ' Thread-safe logging logic
    End Sub
End Class

' ✅ CORRECT: Service injection into module
Public Class AudioModule
    Implements IModule
    
    Private _loggingService As ILoggingService
    
    Public Sub Initialize(loggingService As ILoggingService) Implements IModule.Initialize
        _loggingService = loggingService
    End Sub
End Class

' ❌ WRONG: Direct instantiation (no injection)
Public Class AudioModule
    Private _loggingService As New LoggingService()  ' Should be injected!
End Class

' ❌ WRONG: Stateful service (not thread-safe)
Public Class CounterService
    Private _count As Integer = 0  ' Mutable state = thread issues
    
    Public Function Increment() As Integer
        _count += 1  ' Not thread-safe!
        Return _count
    End Function
End Class
```

---

# 5. UI & Designer Rules
**Tags:** designer, layout, controls, winforms, anchor-dock, deterministic-layout

## 5.1 Designer File Boundaries
Designer files must:

- Contain **only** control declarations  
- Contain **only** layout initialization  
- Contain **InitializeComponent()**  
- Never contain business logic  
- Never contain event handlers  
- Never contain custom methods  

Main `.vb` files must:

- Never declare controls  
- Never duplicate Designer declarations  
- Never modify Designer‑generated code  

---

## 5.2 Control Naming Rules
All controls must:

- Have explicit, descriptive names  
- Avoid default names (Button1, TextBox1, etc.)  
- Follow deterministic naming patterns  
- Reflect purpose, not appearance  

---

## 5.3 Layout Rules
All UI layout must:

- Use deterministic layout behavior  
- Avoid flicker, resizing jumps, or implicit behavior  
- Use explicit Anchor or Dock rules  
- Avoid overlapping controls  
- Avoid hidden or implicit behaviors  

---

## 5.4 Event Handler Rules
Event handlers must:

- Contain no business logic  
- Delegate to services or modules  
- Avoid side effects  
- Avoid inline logic blocks  
- Avoid long methods  
- Avoid UI‑driven state management  
---
## 5.5 Default Layout Policy
**Tags:** anchor, dock, layout-mode, deterministic, control-positioning

The Forge uses a deterministic layout model. Unless the user explicitly requests otherwise, the following rules apply:

1. **Anchor is the default layout mode for all controls.**  
   - The AI must always assign explicit Anchor values.  
   - Controls must never rely on Designer defaults or implicit behavior.

2. **Dock is only used when explicitly requested by the user.**  
   - Phrases such as “dock”, “fill”, “full height”, “full width”, or “left panel that fills” qualify as explicit requests.  
   - The AI must not infer Dock behavior from context alone.

3. **Anchor and Dock must never be mixed on the same control.**  
   - If Dock is used, Anchor must be omitted.  
   - If Anchor is used, Dock must be omitted.

4. **Implicit layout behavior is forbidden.**  
   - No AutoSize‑driven layout unless explicitly requested.  
   - No reliance on default margins, padding, or control placement.  
   - All layout behavior must be explicit and deterministic.

5. **Resizing must remain stable and predictable.**  
   - Controls must not drift, overlap, or resize unpredictably.  
   - Minimum form size must be set when necessary to preserve layout integrity.

**Examples:**

```vb
' ✅ CORRECT: Explicit Anchor (default approach)
Me.btnSubmit.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
Me.btnSubmit.Location = New Point(500, 400)
Me.btnSubmit.Size = New Size(100, 30)

' ✅ CORRECT: Explicit Dock (user requested "fill the left side")
Me.panelLeft.Dock = DockStyle.Left
Me.panelLeft.Width = 200

' ❌ WRONG: No Anchor specified (relies on defaults)
Me.btnSubmit.Location = New Point(500, 400)
Me.btnSubmit.Size = New Size(100, 30)
' Missing: Anchor assignment

' ❌ WRONG: Mixing Anchor and Dock
Me.panelLeft.Dock = DockStyle.Left
Me.panelLeft.Anchor = AnchorStyles.Top Or AnchorStyles.Left
' Cannot use both - conflicts
```

This policy ensures that all UI generated by the Forge behaves consistently across machines, resolutions, and DPI settings.
# 6. Error Handling Rules
Error handling must:

- Be explicit  
- Avoid silent suppression  
- Avoid swallowing exceptions  
- Avoid empty `Catch` blocks  
- Use structured exception handling  
- Avoid global exception traps unless justified  

---

# 7. Common Mistakes
**Tags:** errors, patterns, troubleshooting, best-practices, anti-patterns

**Note:** See `Documentation/Chronicle/DevelopmentLog/IssueSummary.md` for full patterns and solutions.

## 7.1 Layout Issues
❌ Adding controls without explicit Anchor/Dock  
❌ Not testing layout after each control addition  
❌ Adding controls in visual order (creates wrong z-order)  
✅ Add controls bottom-to-top, right-to-left  
✅ Set explicit Anchor before adding next control  

## 7.2 State Management
❌ Multiple sources of truth for same state  
❌ Updating UI without updating underlying state  
❌ No validation before state changes  
✅ Single source of truth pattern  
✅ Update all dependents when state changes  

## 7.3 Event Timing
❌ Using services in constructor before initialization  
❌ No null checks in event handlers  
❌ Events firing before subscribers ready  
✅ Initialize dependencies before dependents  
✅ Defensive null checks everywhere  

## 7.4 Interface Implementation
❌ Changing interface without updating all implementers  
❌ Missing explicit implementation clauses  
✅ Build immediately after interface changes  
✅ Use compiler to find all affected implementations  

## 7.5 Thread Safety
❌ Updating UI controls from background threads  
❌ No thread context checking  
✅ Marshal to UI thread before control updates  
✅ Thread-safe patterns for shared state  

## 7.6 Designer File Constraints
❌ Attempting automated edits of open Designer files  
❌ Putting business logic in Designer files  
✅ Close Visual Studio before automated edits  
✅ Keep Designer files to layout/declarations only  

---

# 8. Non‑Interference Boundaries
The Coding Branch:

- Does **not** define documentation rules  
- Does **not** define project structure  
- Does **not** define taxonomy  
- Does **not** define audit behavior  
- Does **not** override ForgeCharter  

It governs **code only**.

---

# 9. Build Verification Rules
**Tags:** build, compilation, efficiency, documentation, workflow

## 9.1 When to Build
Build verification is **required** after:
- Any code modification (`.vb`, `.Designer.vb`, `.resx`)
- Project file changes (`.vbproj`, `.sln`, `.slnx`)
- Adding/removing files
- Reference changes
- Mixed code + documentation changes

## 9.2 When to Skip Build
Build verification is **not required** after:
- Documentation-only changes (`.md`, `.txt` files)
- Changes to `/Documentation/` folder only
- Constitution or governance file updates
- README updates
- Version history updates (if no code changed)

## 9.3 Rationale
- Documentation files are not compiled
- Building wastes time for docs-only changes
- Allows rapid documentation iteration
- Focus build resources on actual code changes
- Separation of concerns: documentation quality ≠ compilation success

## 9.4 Build Before Deployment
Always build before:
- Committing code changes
- Creating releases
- Deploying to production
- Merging branches (if code changed)

**Examples:**
```
✅ Build Required:
- Modified LoggingService.vb
- Added new control to DashboardMainForm.Designer.vb
- Updated TheForge.vbproj with new file reference
- Changed IModule interface signature

❌ Build NOT Required:
- Updated README.md
- Modified ForgeTome.md in /Documentation/Tomes/
- Added CONSTITUTION.md to .github/
- Updated VersionHistory.chronicle.md (docs only)

✅ Build Required (Mixed):
- Modified ModuleLoaderService.vb AND updated API documentation
- Added new UI control AND updated user guide
```

---

# 10. Routing Behavior
The Coding Branch is invoked when:

- The user requests code  
- The user requests UI controls  
- The user requests Designer behavior  
- The user requests refactoring  
- The user requests implementation details  
- The user requests layout logic  
- The user requests error handling  

Routing:
If task involves code → Use Branch-Coding
Otherwise → Delegate to appropriate branch
ForgeCharter always governs the process

---

# End of Branch-Coding.md
