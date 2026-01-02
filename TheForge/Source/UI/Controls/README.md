# RCH.Forge.Dashboard — User Controls

**Folder Type:** Source/UI/Controls  
**Purpose:** Modular UserControl components for Dashboard UI  
**Last Updated:** 2025-01-02  
**Architecture Milestone:** v0.8.1 (UI Modularization)

---

## Overview

This folder contains all custom UserControl components that comprise the RCH.Forge.Dashboard user interface. Each UserControl encapsulates a specific UI concern with deterministic layout, event-driven communication, and clean separation from business logic.

**Architecture Philosophy:**
- Each UserControl has single responsibility
- Controls expose methods for updates, events for actions
- No business logic in controls (routing only)
- Deterministic layout (Dock, TableLayoutPanel)
- Self-contained and independently testable

---

## Files in This Folder

### ModuleListControl.vb
**Purpose:** Module list display with action buttons  

**Visual Components:**
- `lstModules` - ListBox for module names
- `btnRunModule` - Execute selected module
- `btnUnloadModule` - Unload selected module
- `btnReloadModule` - Hot-reload selected module
- `btnRefreshModules` - Refresh module list

**Layout:**
- lstModules: Dock.Fill
- Buttons: Dock.Bottom (stacked)

**Exposed Methods:**
- `LoadModules(modules As List(Of ModuleMetadata))` - Populate list
- `UpdateButtonStates(canRun, canUnload, canReload)` - Set button enabled states
- `SetSelectedIndex(index As Integer)` - Set selection programmatically

**Exposed Properties:**
- `SelectedIndex As Integer` - Get current selection

**Exposed Events:**
- `ModuleSelected As EventHandler(Of ModuleSelectedEventArgs)` - User selected module
- `RunRequested As EventHandler` - User clicked Run
- `UnloadRequested As EventHandler` - User clicked Unload
- `ReloadRequested As EventHandler` - User clicked Reload
- `RefreshRequested As EventHandler` - User clicked Refresh

**Usage Pattern:**
```vb
' In parent form
Private WithEvents moduleListControl As UI.Controls.ModuleListControl

Private Sub HandleModuleSelected(sender As Object, e As ModuleSelectedEventArgs) _
    Handles moduleListControl.ModuleSelected
    ' React to selection
End Sub

Private Sub HandleRunRequested(sender As Object, e As EventArgs) _
    Handles moduleListControl.RunRequested
    ' Execute module via service
End Sub
```

---

### ModuleDetailsControl.vb
**Purpose:** Display module metadata in structured format  

**Visual Components:**
- TableLayoutPanel with labels and read-only TextBoxes
- Fields: FileName, DisplayName, TypeName, LastLoadedTime, IsLoaded, Dependencies, ConfigPath

**Layout:**
- TableLayoutPanel: Dock.Fill
- 1 column, 15 rows (label + textbox pairs)

**Exposed Methods:**
- `UpdateDetails(metadata As ModuleMetadata)` - Display module info
- `ClearDetails()` - Clear all fields

**No Events:** Read-only display control

---

### LogOutputControl.vb
**Purpose:** Log display with filtering and search  

**Visual Components:**
- `txtLogOutput` - Multiline read-only TextBox
- `pnlLogFilter` - Filter panel (Dock.Bottom)
  - `cmbLogLevel` - Level dropdown (All, Info, Warning, Error)
  - `txtLogSearch` - Search textbox
  - `btnApplyFilter` - Apply search button
  - `btnClearLog` - Clear log button

**Layout:**
- txtLogOutput: Dock.Fill
- pnlLogFilter: Dock.Bottom, Height=70
- Filter controls in TableLayoutPanel (2 cols × 3 rows)

**Exposed Methods:**
- `AppendLog(message As String)` - Add log entry (thread-safe)
- `RebuildLog(entries, filterLevel, searchText)` - Rebuild with filters
- `ClearLog()` - Clear all output

**Exposed Events:**
- `ClearLogRequested As EventHandler` - User clicked Clear
- `FilterApplied As EventHandler(Of FilterAppliedEventArgs)` - User changed filters

**Features:**
- Thread-safe AppendLog via InvokeRequired
- Auto-filtering on dropdown change
- Enter key applies search filter
- StringBuilder optimization for large logs (v0.9.1)

---

### TestAreaControl.vb
**Purpose:** Placeholder for module UI testing area  

**Visual Components:**
- Empty panel with white background

**Layout:**
- Dock.Fill (fills center area)

**Future Use:**
- Module UI display area
- Testing/experimentation space
- Dynamic control hosting

---

## UserControl Development Guidelines

### Creating a New UserControl

**Step 1: Create Class File**
```vb
Imports System.Windows.Forms

Namespace UI.Controls
    Public Class NewControl
        Inherits UserControl
        
        Public Sub New()
            InitializeComponent()
        End Sub
        
        Private Sub InitializeComponent()
            Me.SuspendLayout()
            
            ' Create and configure controls
            
            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
```

**Step 2: Add to Project File**
```xml
<Compile Include="Source\UI\Controls\NewControl.vb">
  <SubType>UserControl</SubType>
</Compile>
```

**Step 3: Define Public API**
- Expose methods for updates
- Expose events for user actions
- Expose read-only properties for state

**Step 4: Use in Parent Form**
```vb
Private WithEvents newControl As UI.Controls.NewControl

Private Sub InitializeLayout()
    newControl = New UI.Controls.NewControl()
    newControl.Dock = DockStyle.Fill
    Me.Controls.Add(newControl)
End Sub
```

---

## Design Patterns

### Pattern 1: Event-Driven Communication
Controls raise events instead of calling parent methods directly:

```vb
' In UserControl
Public Event ActionRequested As EventHandler

Private Sub btnAction_Click(sender As Object, e As EventArgs)
    RaiseEvent ActionRequested(Me, EventArgs.Empty)
End Sub

' In Parent Form
Private Sub Control_ActionRequested(sender As Object, e As EventArgs) _
    Handles myControl.ActionRequested
    ' Handle action
End Sub
```

### Pattern 2: Method-Based Updates
Parent updates control via public methods:

```vb
' In UserControl
Public Sub UpdateData(data As List(Of Item))
    ' Update UI
End Sub

' In Parent Form
myControl.UpdateData(serviceData)
```

### Pattern 3: Property-Based State
Controls expose read-only state via properties:

```vb
' In UserControl
Public ReadOnly Property SelectedItem As Item
    Get
        Return _selectedItem
    End Get
End Property

' In Parent Form
Dim item As Item = myControl.SelectedItem
```

### Pattern 4: Thread-Safe Updates
Controls check InvokeRequired for cross-thread calls:

```vb
Public Sub UpdateUI(data As String)
    If Me.InvokeRequired Then
        Me.Invoke(New Action(Of String)(AddressOf UpdateUI), data)
        Return
    End If
    
    txtDisplay.Text = data
End Sub
```

---

## Layout Standards

### Rule 1: Use Deterministic Layout
- TableLayoutPanel for complex layouts
- Dock for simple stacking
- NO anchoring

### Rule 2: Explicit Control Ordering
Add controls in correct z-order for Dock behavior:
```vb
' Fill control added first
Me.Controls.Add(contentPanel)      ' Dock.Fill
Me.Controls.Add(bottomPanel)       ' Dock.Bottom
Me.Controls.Add(topPanel)          ' Dock.Top
```

### Rule 3: Explicit Sizing
Set explicit Height/Width for controls within TableLayoutPanel:
```vb
lblField.Height = 20
txtField.Height = 23
```

---

## Related Documentation

- **UI Modularization Milestone:** `/Documentation/Chronicle/DevelopmentLog/v081.md`
- **Parent Form:** `/Source/UI/DashboardMainForm.vb`
- **Service Layer:** `/Source/Services/README.md`
- **Development Patterns:** `/Documentation/Chronicle/DevelopmentLog/IssueSummary.md`

---

**All UserControls follow the modular UI architecture established in v0.8.1.**
