# Code Rules Summary - The Forge

**Document Type:** Technical Reference  
**Created:** 2025-01-02  
**Character Count:** 7,845  
**Status:** Final  
**Source:** file8.md (Prompts/file8.md)  
**Related:** ArchitectureRules-Summary.md, DocumentationRules-Summary.md

---

## Overview

This document summarizes code implementation rules from file8.md. These rules define **how** Copilot should write code in The Forge project.

**Rule Precedence:** file8.md operates at lowest precedence. If conflicts arise, higher-level rules win (Master.md ? File6.md ? file1-5 ? file7.md ? file8.md).

---

## Core Principles

### Always Do ?
- Route business logic to services, not UI
- Keep event handlers as orchestration only
- Use Anchor for UserControl layouts
- Declare controls ONLY in Designer.vb
- Let errors surface explicitly
- Mirror existing naming patterns

### Never Do ?
- Put business logic in event handlers
- Declare controls in main .vb file
- Use `-ErrorAction SilentlyContinue`
- Modify Designer files with shell commands
- Suppress Designer errors
- Bypass Visual Studio reload mechanisms

---

## Section 1: Rule Ambiguity

**When rules conflict:**
- Prefer most specific rule (narrowest scope)
- Defer to Master.md and ForgeOrchestrator.md
- Ask user for clarification (don't guess)

---

## Section 2: UI & Services Architecture

### UI Layer Rules
- **Event handlers = orchestration only**
- Gather parameters ? Call services ? Update UI
- NO validation, file I/O, or calculations in UI

**Good Example:**
```vb
Private Sub btnSave_Click(...) Handles btnSave.Click
    Dim data = txtInput.Text
    _myService.SaveData(data)  ' Service does work
    UpdateStatus("Saved")       ' UI updates display
End Sub
```

**Bad Example:**
```vb
Private Sub btnSave_Click(...) Handles btnSave.Click
    ' Validation here... ?
    ' File I/O here... ?
    ' Calculations here... ?
End Sub
```

### Service Layer Rules
- Prefer stateless services
- Make state explicit when needed (caches, collections)
- Follow existing service patterns

---

## Section 3: Plugin & Reflection

- Respect established plugin patterns
- Use `Activator.CreateInstance` only where architecture already does
- Inject services via initialization methods (not hidden globals)

---

## Section 4: Performance & Logging

- Follow existing caching patterns
- Add logging only where conventions exist
- Avoid premature optimization

---

## Section 5: Naming & Structure

**Types, Methods, Members:**
- Mirror existing naming patterns
- Use existing suffixes/prefixes
- Don't introduce new schemes without justification

**File Placement:**
- Match existing folder structure (UI, Services, Models)
- Ask before creating new top-level folders

---

## Section 6: Terminal Commands & Errors

### NEVER Use ?
- `-ErrorAction SilentlyContinue`
- Silent error suppression

### ALWAYS Do ?
- Let errors surface explicitly
- Use `-ErrorAction Stop` for critical operations
- Validate paths before destructive operations
- Use `edit_file`, `create_file`, `remove_file` tools (not shell commands)

**Rationale:** Explicit over implicit. Errors must be visible, not hidden.

---

## Section 7: Designer File Handling ? **CRITICAL**

### 7.1-7.3: File Locks & Modification

**Key Facts:**
- Designer files MAY be locked when Designer view is open
- `.Designer.vb` CAN be modified when Designer is closed
- If `edit_file` fails: instruct user to close Designer
- Never use shell commands on Designer files

**Workflow:**
1. User closes Designer view
2. Copilot uses `edit_file` on `.Designer.vb`
3. Copilot uses `edit_file` on `.vb` (logic only)
4. User saves ? Can now open Designer

---

### 7.4: Control Declaration Rule ?? **GOLDEN RULE**

**Partial classes merge ALL declarations from both files.**  
**Declare each element in EXACTLY ONE file.**

**VB.NET Errors if Declared in Both:**
- `BC30260` "already declared"
- `BC30269` "has multiple definitions"
- `BC30663` "attribute cannot be applied multiple times"

**Declaration Matrix:**

| Element | Main .vb | Designer .vb |
|---------|----------|--------------|
| `<DesignerGenerated()>` | ? NO | ? YES |
| `Partial Class` | ? YES | ? YES |
| `Public Sub New()` | ? YES | ? NO |
| `InitializeComponent()` | ? NO | ? YES |
| `Dispose()` | ? NO | ? YES |
| `Private components` | ? NO | ? YES |
| Control declarations | ? NO | ? YES |
| Event handlers | ? YES | ? NO |
| Public/Private methods | ? YES | ? NO |

**Main .vb - Business Logic Only:**
```vb
Partial Public Class MyControl
    Inherits UserControl
    
    ' NO control declarations!
    
    Public Sub New()
        InitializeComponent()
    End Sub
    
    Private Sub btnSave_Click(...) Handles btnSave.Click
        ' Event handlers here
    End Sub
End Class
```

**Designer .vb - UI Only:**
```vb
<DesignerGenerated()>
Partial Class MyControl
    Inherits UserControl
    
    Private components As IContainer
    
    Private Sub InitializeComponent()
        Me.btnSave = New Button()
        ' UI setup here
    End Sub
    
    Friend WithEvents btnSave As Button
End Class
```

---

### 7.5: Layout Strategy ??

**Priority Order:**
1. **Anchor** (PRIMARY) - UserControls, responsive forms
2. **Dock** - Main forms only (structural layout)
3. **Absolute** - Fixed dialogs only

#### Anchor (Recommended)
- Set `Location` and `Size` for initial position
- Set `Anchor` for resize behavior
- Provides Designer editability + responsive behavior

**Common Patterns:**
- `Top + Left` (default): Fixed position
- `Top + Left + Right`: Stretches horizontally
- `Top + Bottom + Left + Right`: Fills space
- `Bottom + Right`: Stays in corner

**Example:**
```vb
Me.txtSearch.Location = New Point(10, 10)
Me.txtSearch.Size = New Size(280, 27)
Me.txtSearch.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
```

#### Dock (Main Forms Only)
- Use for structural regions (left panel, center, right panel)
- Good for splitters, status bars, toolbars
- NOT for UserControl internals

**Example:**
```vb
Me.moduleListControl.Dock = DockStyle.Left
Me.moduleListControl.Width = 250
```

#### Absolute (Fixed Dialogs)
- No Anchor, no Dock
- Fixed position and size
- For dialogs that don't resize

---

### 7.6: Partial Class Consistency
- Both files must declare `Partial Class` with identical names
- `DesignerGenerated` appears ONLY in `.Designer.vb`

---

### 7.7: Project File Integrity
- `.Designer.vb` marked with `<DependentUpon>`
- Main `.vb` includes `<SubType>UserControl</SubType>`
- Check for duplicate project entries

**Correct:**
```xml
<Compile Include="Source\UI\Controls\MyControl.vb">
  <SubType>UserControl</SubType>
</Compile>
<Compile Include="Source\UI\Controls\MyControl.Designer.vb">
  <DependentUpon>MyControl.vb</DependentUpon>
</Compile>
```

---

### 7.8: External Changes
- If editing externally: close Visual Studio first
- After external edits: let VS reload and resynchronize
- Never bypass VS reload mechanism

---

### 7.9: Designer Workflow

**Preferred:**
1. User closes Designer view
2. Copilot uses `edit_file` on `.Designer.vb`
3. Copilot creates text file with complete `.vb` file content (logic only, no control declarations)
4. User pastes content from text file into `.vb` file in Visual Studio
5. User saves files

**Fallback (if edit_file fails on Designer.vb):**
1. Generate complete `.Designer.vb` file content
2. Generate complete `.vb` file content (logic only, no control declarations)
3. User closes Designer view
4. User pastes content into both files in Visual Studio
5. User saves files

---

### 7.10: Error Handling
- Never suppress Designer errors
- Surface locking/synchronization errors explicitly

**Common Errors:**
- `BC30260`: Control declared in both files ? Remove from main
- `BC30506`: Missing WithEvents in Designer
- `BC30269`: Duplicate project entry

---

### 7.11: Automation Limitations
- Don't automate Designer open/close
- Don't modify Designer structures beyond what VS produces
- Provide complete file content, not edit instructions

---

### 7.12: WithEvents Usage

**With event handlers:**
```vb
' Designer.vb
Friend WithEvents btnSave As Button

' Main.vb
Private Sub btnSave_Click(...) Handles btnSave.Click
End Sub
```

**Without event handlers:**
```vb
' Designer.vb
Friend lblStatus As Label  ' No WithEvents

' Main.vb
Public Sub UpdateStatus(message As String)
    lblStatus.Text = message
End Sub
```

---

## Quick Reference Checklists

### ? Code Generation Checklist
- [ ] Business logic in services (not UI)
- [ ] Event handlers = orchestration only
- [ ] Controls declared only in Designer.vb
- [ ] Anchor used for UserControl layouts
- [ ] Proper `Location`, `Size`, `Name`, `TabIndex`
- [ ] Errors surface explicitly (no SilentlyContinue)

### ? Designer File Checklist
- [ ] `Partial Class` in both files
- [ ] `<DesignerGenerated()>` only in Designer.vb
- [ ] `Public Sub New()` only in main .vb
- [ ] `InitializeComponent()` only in Designer.vb
- [ ] All controls declared in Designer.vb
- [ ] Event handlers only in main .vb
- [ ] Project file has proper `<DependentUpon>`

### ? Never Do
- [ ] Business logic in event handlers
- [ ] Controls in main .vb file
- [ ] `-ErrorAction SilentlyContinue`
- [ ] Shell commands on Designer files
- [ ] Suppress Designer errors
- [ ] Bypass VS reload

---

## Layout Decision Tree

```
Generate InitializeComponent?
?
?? Main Form? ? Use Dock
?   (DashboardMainForm structural layout)
?
?? UserControl? ? Use Anchor (PRIMARY)
?   (Responsive + Designer-editable)
?
?? Fixed dialog? ? Use Absolute
?   (No Anchor/Dock)
?
?? Default ? Use Anchor
```

---

## Summary

**file8.md ensures:**
1. Clean architecture (business logic in services)
2. Designer editability (controls in Designer.vb only)
3. Responsive layouts (Anchor for UserControls)
4. No duplicates (one declaration per element)
5. Explicit errors (never silent)

**Priority: Anchor > Dock > Absolute**

**Result:** Fully Designer-editable, responsive controls with clean architecture! ??

---

**Related Documentation:**
- Prompts/file8.md (source)
- ArchitectureRules-Summary.md (File6.md summary)
- DocumentationRules-Summary.md (file4.md + file7.md summary)
