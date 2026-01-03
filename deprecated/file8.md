# file8.md — Code Behavior & Implementation Extensions

This file extends the Forge rule stack for code generation and modification. It does not restate core language, architecture, or layout rules already defined in Master.md or file1–file6; it only clarifies how Copilot should behave when implementing them.

----------------------------------------------------------------------
0. Rule precedence (file8.md scope)
----------------------------------------------------------------------

This file operates at the lowest precedence in the Forge rule stack:

Precedence order (highest to lowest):
1. Master.md — Canonical governance index
2. ForgeOrchestrator.md — Orchestration and execution rules
3. File6.md — Prime Directives (architectural principles)
4. file1–file5 — Core domain rules (Scriptorium, Layout, Taxonomy, Config)
5. file7.md — Documentation extensions (file size, multi-file output)
6. file8.md — Code implementation clarifications (this file)

Conflict resolution:
• If file8.md contradicts file1–file6: file1–file6 wins
• If file8.md contradicts File6.md Prime Directives: File6.md wins
• file8.md clarifies *how* to implement rules, not *what* the rules are
• At same precedence level: more specific beats more general

When in doubt: Ask user for clarification per section 1 below.

----------------------------------------------------------------------
1. When rules appear ambiguous
----------------------------------------------------------------------

If multiple MD files apply and their guidance seems ambiguous:

• Copilot must:
  - Prefer the most specific rule (narrowest scope) over generic guidance.
  - Prefer Master.md and ForgeOrchestrator.md for resolving precedence.
  - Ask the user for clarification rather than guessing when in doubt.

----------------------------------------------------------------------
2. UI, services, and business logic behavior
----------------------------------------------------------------------

When generating or modifying code in the UI layer:

• Copilot must:
  - Route non-trivial logic into services or dedicated classes, consistent with existing patterns.
  - Keep event handlers as orchestration only: parameter gathering, calling services, updating UI.

When generating or modifying services:

• Copilot should:
  - Prefer pure, stateless services where practical.
  - Make state explicit when it must exist (e.g., caches, in-memory collections), following the style in current services.

----------------------------------------------------------------------
3. Plugin and reflection-based behavior
----------------------------------------------------------------------

When dealing with dynamically loaded modules:

• Copilot must:
  - Respect the established plugin pattern in the solution (e.g., reflection-based loading).
  - Use Activator.CreateInstance or equivalent only where the existing architecture already does so.
  - Ensure that any required services or configuration are injected or provided via clearly defined initialization methods after instantiation, not hidden globals.

----------------------------------------------------------------------
4. Performance and logging behavior
----------------------------------------------------------------------

When introducing new operations that might be expensive or performance-sensitive:

• Copilot should:
  - Follow existing caching and timing patterns present in the codebase.
  - Add diagnostic logging only where it aligns with existing logging conventions.
  - Avoid premature optimization that introduces complexity without clear benefit.

----------------------------------------------------------------------
5. Naming and structure behavior
----------------------------------------------------------------------

When introducing new types, methods, or members:

• Copilot must:
  - Mirror existing naming patterns and suffixes/prefixes already in use in the same folder or layer.
  - Avoid introducing new naming schemes without a clear reason grounded in existing docs (Master, NamingCanon, etc.).

When creating new files:

• Copilot should:
  - Place them in the folder that matches existing architectural patterns (UI, Services, Models, etc.).
  - Ask the user before creating new top-level folders or structural changes.

----------------------------------------------------------------------
6. Terminal command and error handling behavior
----------------------------------------------------------------------

When executing terminal commands (PowerShell, bash, cmd, etc.):

• Copilot must:
  - NEVER use -ErrorAction SilentlyContinue or equivalent silent error suppression
  - Allow errors to surface explicitly so failures are visible
  - Prefer -ErrorAction Stop for critical operations that must not fail silently
  - Report command failures clearly to the user

• Copilot should:
  - Validate file/path existence before destructive operations when practical
  - Use appropriate tools (edit_file, create_file, remove_file) instead of terminal commands for file modifications when available
  - Provide clear context when terminal command execution fails
  - Follow existing error handling patterns in the codebase

Rationale:
Silent error suppression violates the Forge principle of explicit over implicit behavior.
Errors must be visible so they can be addressed, not hidden.
----------------------------------------------------------------------
7. Designer File Handling
----------------------------------------------------------------------

This section defines required behavior when generating, modifying, or interacting with
files managed by the Visual Studio Windows Forms Designer. These rules ensure that
Designer‑generated code remains stable, synchronized, and free from corruption.

### 7.1 Scope
These rules apply to:
- Any `.Designer.vb` file
- Any `.vb` file paired with a Designer file through `Partial Class`
- Any UserControl or Form using `InitializeComponent`
- Any project file entries involving `<SubType>UserControl</SubType>` or `<DependentUpon>`

### 7.2 File Lock Awareness
- Assume Designer‑managed files may be locked by Visual Studio when Designer view is open.
- The `.Designer.vb` file CAN be modified when Designer is closed (not locked).
- If attempting to edit and receiving "Could not get text view", instruct user to close Designer.
- User can paste complete file content into `.Designer.vb` when Designer is closed.

### 7.3 Modification Rules
- Do not modify `.Designer.vb` files using shell commands under any circumstances.
- The `.Designer.vb` file CAN be modified by Copilot directly when:
  - File is not currently open in Visual Studio Designer view
  - File is open in code editor only (not Designer view)
  - Copilot has complete understanding of the control structure
- Copilot can use edit_file tool on `.Designer.vb` files when Designer is not active
- If edit_file fails with "Could not get text view", instruct user to close Designer view
- Always provide complete `.Designer.vb` content in edit_file tool, not partial edits
- The user can also paste complete file content if edit_file fails (file not locked when Designer closed)
- Do not modify Designer‑paired `.vb` files using shell commands if Visual Studio is open
- Main `.vb` files can be modified with edit_file tool directly

### 7.4 Control Declaration Rule (CRITICAL - Prevents Duplicates)

**THE GOLDEN RULE:** Partial classes merge ALL declarations from both files.

If you declare something in BOTH files, VB.NET throws errors:
- `BC30260` - "already declared"
- `BC30269` - "has multiple definitions"
- `BC30663` - "attribute cannot be applied multiple times"

**Solution:** Declare each element in EXACTLY ONE file.

**Quick Reference:**

| Element | .vb (Main) | .Designer.vb |
|---------|-----------|--------------|
| `<DesignerGenerated()>` attribute | ❌ NO | ✅ YES |
| `Partial Class` declaration | ✅ YES | ✅ YES |
| `Public Sub New()` | ✅ YES | ❌ NO |
| `InitializeComponent()` | ❌ NO | ✅ YES |
| `Dispose()` override | ❌ NO | ✅ YES |
| `Private components` | ❌ NO | ✅ YES |
| Control declarations | ❌ NO | ✅ YES |
| Event handlers (`Handles`) | ✅ YES | ❌ NO |
| Public/Private methods | ✅ YES | ❌ NO |

**Main file (.vb) - Business Logic Only:**
```vb
Namespace UI.Controls
    Partial Public Class MyControl
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
        End Sub

        ' Event handlers
        Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            ' Business logic
        End Sub

        ' Public methods
        Public Sub LoadData(data As String)
            txtName.Text = data  ' Uses Designer's control
        End Sub
    End Class
End Namespace
```

**Designer file (.Designer.vb) - UI Only:**
```vb
Namespace UI.Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class MyControl
        Inherits UserControl

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            ' Cleanup
        End Sub

        Private components As IContainer

        Private Sub InitializeComponent()
            Me.txtName = New TextBox()
            Me.txtName.Location = New Point(10, 10)
            Me.txtName.Size = New Size(200, 27)
            ' ... more UI code ...
        End Sub

        Friend WithEvents txtName As TextBox
        Friend WithEvents btnSave As Button
    End Class
End Namespace
```

**Conversion Checklist:**

Main .vb file - REMOVE:
- All control declarations (`Private txtName As TextBox`)
- Entire `InitializeComponent()` method
- `Dispose()` override (if present)
- `<DesignerGenerated()>` attribute (if present)

Main .vb file - KEEP:
- Change to `Partial Public Class`
- `Public Sub New()` with `InitializeComponent()` call
- All event handlers
- All public/private methods

Designer .vb file - ADD:
- `<DesignerGenerated()>` on `Partial Class`
- `Dispose()` override
- `Private components As IContainer`
- `InitializeComponent()` with ALL UI code
- ALL control declarations as `Friend` or `Friend WithEvents`

### 7.5 Layout Strategy (Designer-Friendly)

When generating `InitializeComponent()` for Designer support:

**PRIMARY APPROACH - Anchor-Based Layout (Recommended):**
- Use `Anchor` property for responsive, Designer-editable layouts
- Set explicit `Location` (X, Y) and `Size` (Width, Height) for initial positioning
- Use `Anchor` to define how controls resize relative to their container
- Controls remain freely movable and resizable in Designer
- **WHY:** Provides both Designer editability AND responsive behavior on form resize

**Anchor Property Usage:**
```vb
' ✓ GOOD - Anchor for responsive layout
Me.txtName.Location = New System.Drawing.Point(10, 10)
Me.txtName.Size = New System.Drawing.Size(200, 27)
Me.txtName.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
' Control grows/shrinks horizontally when form resizes

' ✓ GOOD - Anchor button to bottom-right
Me.btnSave.Location = New System.Drawing.Point(300, 250)
Me.btnSave.Size = New System.Drawing.Size(100, 30)
Me.btnSave.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
' Button stays in bottom-right corner when form resizes
```

**Common Anchor Patterns:**
- **Top + Left** (default): Control stays in top-left, doesn't resize
- **Top + Left + Right**: Control stretches horizontally, stays at top
- **Top + Bottom + Left + Right**: Control fills available space (like Dock.Fill but Designer-editable)
- **Bottom + Right**: Control stays in bottom-right corner
- **Top + Right**: Control stays in top-right corner

**WHEN TO USE DOCK (Limited Use):**
- `DockStyle.Fill/Top/Bottom/Left/Right` ONLY for main form layout (DashboardMainForm)
- Use Dock when you need containers that partition the form into regions
- Dock is acceptable for splitter bars, status bars, and toolbars on main forms
- **NOT for controls inside UserControls** - use Anchor instead
- **WHY:** Dock creates rigid layouts; UserControls need flexible Designer editing

**WHEN TO USE ABSOLUTE POSITIONING (No Anchor):**
- Simple forms that should NOT resize controls
- Fixed-size dialogs or tool windows
- Controls that must maintain exact pixel positions
- **Default to NO Anchor** if user doesn't specify resize behavior

**Example - UserControl with Anchor:**
```vb
' UserControl with responsive layout
Private Sub InitializeComponent()
    Me.txtSearch = New TextBox()
    Me.btnSearch = New Button()
    Me.lstResults = New ListBox()
    
    ' Search box - stretches horizontally
    Me.txtSearch.Location = New Point(10, 10)
    Me.txtSearch.Size = New Size(280, 27)
    Me.txtSearch.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
    Me.txtSearch.TabIndex = 0
    
    ' Search button - stays at top-right
    Me.btnSearch.Location = New Point(300, 10)
    Me.btnSearch.Size = New Size(90, 27)
    Me.btnSearch.Anchor = AnchorStyles.Top Or AnchorStyles.Right
    Me.btnSearch.TabIndex = 1
    Me.btnSearch.Text = "Search"
    
    ' Results list - fills remaining space
    Me.lstResults.Location = New Point(10, 50)
    Me.lstResults.Size = New Size(380, 240)
    Me.lstResults.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
    Me.lstResults.TabIndex = 2
    
    ' UserControl
    Me.Controls.Add(Me.lstResults)
    Me.Controls.Add(Me.btnSearch)
    Me.Controls.Add(Me.txtSearch)
    Me.Size = New Size(400, 300)
End Sub
```

**Example - Main Form with Dock:**
```vb
' Main form uses Dock for region-based layout
Private Sub InitializeComponent()
    Me.moduleListControl = New ModuleListControl()
    Me.splitterVertical = New Splitter()
    Me.testAreaControl = New TestAreaControl()
    Me.statusStrip = New StatusStrip()
    
    ' Left panel - docked
    Me.moduleListControl.Dock = DockStyle.Left
    Me.moduleListControl.Width = 250
    
    ' Splitter - docked
    Me.splitterVertical.Dock = DockStyle.Left
    Me.splitterVertical.Width = 3
    
    ' Center panel - fills remaining space
    Me.testAreaControl.Dock = DockStyle.Fill
    
    ' Status bar - docked to bottom
    Me.statusStrip.Dock = DockStyle.Bottom
End Sub
```

**Requirements for Designer editability:**
- All controls must have explicit `Location` property
- All controls must have explicit `Size` property
- Controls must have meaningful `Name` properties
- TabIndex should be set for proper tab order
- Use `Anchor` for responsive layouts (recommended)
- Use NO Anchor for fixed layouts
- Use `Dock` only on main forms for region-based layouts

**Rationale:** 
- **Anchor** provides the best balance: Designer editability + responsive behavior
- **Dock** is for structural layouts on main forms, not detail controls
- **Absolute positioning** (no Anchor/Dock) is for fixed-size forms only
- All three approaches allow full Designer editing, but Anchor is most flexible

**Priority Order:**
1. **Anchor** (default for UserControls and responsive forms)
2. **Dock** (only for main form layout structure)
3. **Absolute/None** (only for fixed-size dialogs)

----------------------------------------------------------------------
7.6 Partial Class Consistency
- When generating or modifying a UserControl or Form, ensure that:
  - Both the main `.vb` file and `.Designer.vb` file declare `Partial Class` with identical names.
  - The `DesignerGenerated` attribute appears ONLY in `.Designer.vb` file.
- Never introduce mismatched class declarations that would desynchronize Designer state.

### 7.7 Project File Integrity
- When creating or restructuring Designer‑managed files:
  - Ensure the `.Designer.vb` file is marked with `<DependentUpon>` in the project file.
  - Ensure the main `.vb` file includes `<SubType>UserControl</SubType>` or `<SubType>Form</SubType>` as appropriate.
- Do not create temporary files inside the project directory that may be auto‑imported by Visual Studio.
- Check for duplicate entries in project file (control listed twice causes duplicate declarations).

### 7.8 External Change Handling
- If a Designer‑managed file must be edited externally, require Visual Studio to be closed first.
- After external edits, Visual Studio must be allowed to reload the file and resynchronize Designer state.
- Never bypass Visual Studio's reload mechanism.
- When Designer is closed, `.Designer.vb` can be edited and VS will reload on next Designer open.

### 7.9 Designer Workflow

**Workflow A: Copilot Edits Directly (Preferred)**
1. User closes Designer view (if open) in Visual Studio
2. Copilot uses edit_file tool to update `.Designer.vb` file
3. Copilot creates text file with complete `.vb` file content (logic only, no control declarations)
4. User pastes content from text file into `.vb` file in Visual Studio
5. User saves files in Visual Studio
6. User can now open Designer and edit controls visually
7. Designer modifies `.Designer.vb` as user makes changes
8. User saves and continues working

**Workflow B: User Pastes Content (Fallback)**
1. If edit_file fails with "Could not get text view":
2. Copilot generates complete `.Designer.vb` file content with Designer-friendly layout
3. Copilot generates complete `.vb` file content (logic only, no control declarations)
4. User closes Designer view (if open) in Visual Studio
5. User pastes content into both files in Visual Studio
6. User saves files
7. User can now open Designer and edit controls visually

**Key Point:** `.Designer.vb` files are NOT locked when Designer view is closed. 
Copilot can edit them directly with edit_file tool. However, main `.vb` files should be provided as text for user to paste for safety.

### 7.10 Error Handling
- Do not suppress errors when interacting with Designer‑managed files.
- Surface locking, synchronization, or project‑structure errors explicitly.
- Do not attempt fallback operations that could corrupt Designer state.
- Common errors and causes:
  - BC30260 "already declared": Control declared in both files - remove from main file
  - BC30506 "Handles requires WithEvents": Control not declared WithEvents in Designer
  - BC30269 "multiple definitions": Duplicate entry in project file or duplicate Designer file

### 7.11 Automation Limitations
- Do not attempt to automate Designer interactions (open/close Designer, reload Designer, etc.).
- Do not attempt to modify Designer‑generated code structures beyond what Visual Studio itself produces.
- Respect that Designer files are authoritative for layout and component initialization.
- Provide complete file content for user to paste, not instructions to manually edit specific lines.

### 7.12 WithEvents Usage
Controls that need event handlers in main file:
- Declare as `Friend WithEvents` in Designer file
- Allows `Handles controlName.EventName` in main file

Controls without event handlers:
- Declare as `Friend` (no WithEvents) in Designer file
- Still accessible to main file methods
- Reduces unnecessary WithEvents overhead

End of file8.md.End of file8.md.