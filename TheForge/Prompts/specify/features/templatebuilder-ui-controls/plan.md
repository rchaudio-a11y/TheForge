# Implementation Plan: TemplateBuilder UI Control Library

**Feature:** `templatebuilder-ui-controls`  
**Status:** Draft - Awaiting Approval  
**Created:** 2026-01-05  
**Estimated Effort:** 15-18 hours  
**Related:** spec.md, tasks.md

---

## Overview

Extract TemplateBuilderControl UI (1,607 lines) into reusable control library that consumes RCH.TemplateStorage v1.0 service layer.

---

## Prerequisites

### Must Be Complete
- ? RCH.TemplateStorage v1.0 (Production Ready - 2026-01-05)
- ? All 32 tests passing
- ? Documentation complete

### Required Tools
- Visual Studio 2022
- .NET 8.0 SDK
- MS Access Database Engine
- Git

---

## Implementation Phases

### Phase 1: Project Setup (2h)

**Goal:** Create empty project structure

**Steps:**
1. Create `RCH.TemplateStorage.Controls.vbproj`
   ```xml
   <Project Sdk="Microsoft.NET.Sdk">
     <PropertyGroup>
       <TargetFramework>net8.0-windows</TargetFramework>
       <UseWindowsForms>true</UseWindowsForms>
       <RootNamespace>RCH.TemplateStorage.Controls</RootNamespace>
     </PropertyGroup>
     <ItemGroup>
       <ProjectReference Include="..\RCH.TemplateStorage\RCH.TemplateStorage.vbproj" />
     </ItemGroup>
   </Project>
   ```

2. Create folder structure:
   - `Controls/`
   - `Dialogs/`
   - `Helpers/`
   - `Documentation/`

3. Create initial README.md

4. Build and verify

**Deliverables:**
- ? Project builds successfully
- ? Structure in place
- ? README exists

---

### Phase 2: Extract Core UI (4h)

**Goal:** Extract TreeView and basic UI from legacy control

**Steps:**

#### 2.1 Create TemplateBuilderControl Shell
```vb
''' <summary>
''' UserControl for building directory structure templates
''' </summary>
Public Class TemplateBuilderControl
    Inherits UserControl
    
    Public Const Version As String = "1.0.0"
    
    Private _currentTemplate As TemplateDefinition
    Private _service As TemplateStorageService
    Private _hasUnsavedChanges As Boolean
    
    Public Sub New()
        InitializeComponent()
    End Sub
End Class
```

#### 2.2 Extract TreeView Logic
From legacy control, extract:
- TreeView creation and initialization
- Node population logic
- Node selection handling
- Visual styling

**Remove:**
- All DirectoryTemplate references
- All FolderDefinition references
- JSON serialization code

**Replace With:**
- TemplateDefinition
- TemplateFolderDefinition
- TemplateJsonSerializer (via service)

#### 2.3 Create Designer File
- Add TreeView control
- Add buttons (Add, Remove, Save, Load)
- Add properties panel
- Set anchoring for resize

**Deliverables:**
- ? TemplateBuilderControl with TreeView
- ? Basic folder display working
- ? Zero duplicate models

---

### Phase 3: Service Integration (3h)

**Goal:** Replace all data operations with service calls

**Steps:**

#### 3.1 Add Service Dependency
```vb
Public Property DatabasePath As String

Private _service As TemplateStorageService

Private Sub InitializeService()
    If String.IsNullOrEmpty(DatabasePath) Then
        Throw New InvalidOperationException("DatabasePath must be set")
    End If
    
    _service = New TemplateStorageService(DatabasePath)
End Sub
```

#### 3.2 Implement Save
```vb
Public Function SaveTemplate() As Boolean
    Try
        If _currentTemplate.TemplateID = 0 Then
            _currentTemplate = _service.CreateTemplate(_currentTemplate)
        Else
            _service.UpdateTemplate(_currentTemplate)
        End If
        
        _hasUnsavedChanges = False
        RaiseEvent TemplateSaved(Me, New TemplateSavedEventArgs(_currentTemplate))
        Return True
        
    Catch ex As Exception
        RaiseEvent ValidationError(Me, New ValidationErrorEventArgs(ex.Message))
        Return False
    End Try
End Function
```

#### 3.3 Implement Load
```vb
Public Sub LoadTemplate(templateId As Integer)
    Try
        _currentTemplate = _service.GetTemplate(templateId)
        
        If _currentTemplate Is Nothing Then
            Throw New InvalidOperationException($"Template {templateId} not found")
        End If
        
        RefreshTreeView()
        _hasUnsavedChanges = False
        RaiseEvent TemplateLoaded(Me, New TemplateLoadedEventArgs(_currentTemplate))
        
    Catch ex As Exception
        RaiseEvent ValidationError(Me, New ValidationErrorEventArgs(ex.Message))
    End Try
End Sub
```

#### 3.4 Implement Import/Export
```vb
Public Function ImportFromJson(filePath As String) As Boolean
    Try
        Dim serializer As New TemplateJsonSerializer()
        _currentTemplate = serializer.DeserializeFromFile(filePath)
        RefreshTreeView()
        _hasUnsavedChanges = True
        Return True
    Catch ex As Exception
        RaiseEvent ValidationError(Me, New ValidationErrorEventArgs(ex.Message))
        Return False
    End Try
End Function

Public Function ExportToJson(filePath As String) As Boolean
    Try
        Dim serializer As New TemplateJsonSerializer()
        serializer.SerializeToFile(_currentTemplate, filePath)
        Return True
    Catch ex As Exception
        RaiseEvent ValidationError(Me, New ValidationErrorEventArgs(ex.Message))
        Return False
    End Try
End Function
```

**Deliverables:**
- ? All operations via service
- ? No direct database access
- ? Error handling in place

---

### Phase 4: Polish & Features (2h)

**Goal:** Add professional UI elements

**Steps:**

#### 4.1 Add Icons
```vb
' Add to TreeView nodes
Private Function CreateFolderNode(folder As TemplateFolderDefinition) As TreeNode
    Dim node As New TreeNode(folder.Name)
    node.ImageKey = "folder"
    node.SelectedImageKey = "folder_open"
    node.Tag = folder
    Return node
End Function
```

#### 4.2 Context Menu
```vb
Private Sub CreateContextMenu()
    Dim menu As New ContextMenuStrip()
    menu.Items.Add("Add Subfolder", Nothing, AddressOf OnAddSubfolder)
    menu.Items.Add("Rename", Nothing, AddressOf OnRename)
    menu.Items.Add("Remove", Nothing, AddressOf OnRemove)
    treeView.ContextMenuStrip = menu
End Sub
```

#### 4.3 Keyboard Shortcuts
```vb
Private Sub treeView_KeyDown(sender As Object, e As KeyEventArgs)
    Select Case e.KeyCode
        Case Keys.F2
            RenameSelectedFolder()
        Case Keys.Delete
            RemoveSelectedFolder()
        Case Keys.Insert
            AddFolder()
    End Select
End Sub
```

#### 4.4 Tooltips
```vb
Private Sub treeView_MouseHover(sender As Object, e As EventArgs)
    Dim node = treeView.GetNodeAt(treeView.PointToClient(MousePosition))
    If node IsNot Nothing Then
        Dim folder = CType(node.Tag, TemplateFolderDefinition)
        toolTip.SetToolTip(treeView, $"{folder.Name}{vbCrLf}{folder.Description}")
    End If
End Sub
```

**Deliverables:**
- ? Icons on nodes
- ? Context menu working
- ? Keyboard shortcuts
- ? Tooltips

---

### Phase 5: Testing (2h)

**Goal:** Comprehensive test coverage

**Steps:**

#### 5.1 Unit Tests
Create `TemplateBuilderControlTests.vb`:

```vb
<TestClass>
Public Class TemplateBuilderControlTests
    
    <TestMethod>
    Public Sub TestAddFolder()
        ' Arrange
        Dim control As New TemplateBuilderControl()
        control.DatabasePath = testDbPath
        control.NewTemplate()
        
        ' Act
        control.AddFolder("TestFolder")
        
        ' Assert
        Assert.AreEqual(1, control.CurrentTemplate.Folders.Count)
        Assert.AreEqual("TestFolder", control.CurrentTemplate.Folders(0).Name)
    End Sub
    
    ' ... more tests
End Class
```

#### 5.2 Integration Tests
Test with real database:
```vb
<TestMethod>
Public Sub TestSaveAndLoad()
    ' Create template
    Dim control1 As New TemplateBuilderControl()
    control1.DatabasePath = testDbPath
    control1.NewTemplate()
    control1.AddFolder("Folder1")
    Dim saved = control1.SaveTemplate()
    
    ' Load in new control
    Dim control2 As New TemplateBuilderControl()
    control2.DatabasePath = testDbPath
    control2.LoadTemplate(control1.CurrentTemplate.TemplateID)
    
    ' Verify
    Assert.AreEqual(1, control2.CurrentTemplate.Folders.Count)
    Assert.AreEqual("Folder1", control2.CurrentTemplate.Folders(0).Name)
End Sub
```

#### 5.3 Manual Testing
Execute test plan (see spec.md section 6.3)

**Deliverables:**
- ? 10+ unit tests (90%+ coverage)
- ? Integration tests passing
- ? Manual test plan complete

---

### Phase 6: Documentation (1h)

**Goal:** Complete professional documentation

**Steps:**

#### 6.1 README.md
```markdown
# RCH.TemplateStorage.Controls

Reusable UI controls for template building and management.

## Installation

```xml
<ProjectReference Include="..\RCH.TemplateStorage.Controls\RCH.TemplateStorage.Controls.vbproj" />
```

## Quick Start

```vb
Dim control As New TemplateBuilderControl()
control.DatabasePath = "C:\Templates\Templates.accdb"
control.NewTemplate()
```

## Features
- Visual folder hierarchy editor
- Database-backed persistence
- JSON import/export
- Legacy template support
```

#### 6.2 API.md
Full API reference with all properties, methods, events

#### 6.3 CHANGELOG.md
```markdown
# Changelog

## [1.0.0] - 2026-01-05

### Added
- Initial release
- TemplateBuilderControl with full UI
- Service layer integration
- Legacy template import
- 10+ unit tests
```

#### 6.4 XML Documentation
Add to all public members

**Deliverables:**
- ? README complete
- ? API reference complete
- ? CHANGELOG created
- ? XML docs added

---

### Phase 7: Deprecation (1h)

**Goal:** Mark legacy control obsolete

**Steps:**

#### 7.1 Add Obsolete Attribute
In legacy `TemplateBuilderControl.vb`:
```vb
<Obsolete("This control has been moved to RCH.TemplateStorage.Controls library. " & _
          "Update your references. This version will be removed in next major release.", 
          False)>
Public Class TemplateBuilderControl
    Inherits UserControl
```

#### 7.2 Update NewDatabaseGenerator
1. Add reference to new library
2. Update Imports statements
3. Test build and run
4. Verify functionality

#### 7.3 Create Migration Guide
Document the migration process

**Deliverables:**
- ? Legacy control marked obsolete
- ? NewDatabaseGenerator updated
- ? Migration guide created

---

## Risk Management

### High-Priority Risks

**Risk 1: TreeView Binding Issues**
- **Mitigation:** Start simple, test incrementally
- **Contingency:** Use manual refresh pattern

**Risk 2: Service Layer Compatibility**
- **Mitigation:** RCH.TemplateStorage frozen at v1.0
- **Contingency:** Wrapper layer if needed

**Risk 3: Timeline Overrun**
- **Mitigation:** Track daily, defer optional features
- **Contingency:** Release without drag-and-drop

---

## Quality Gates

### Phase Completion Criteria

Each phase must meet:
- ? All code compiles
- ? All tests passing
- ? No warnings
- ? Documentation updated
- ? Git commit with clear message

---

## Dependencies

**Critical Path:**
```
Phase 1 ? Phase 2 ? Phase 3 ? Phase 5 ? Phase 7
```

**Parallel Work:**
- Phase 4 (Polish) can start during Phase 3
- Phase 6 (Docs) can start during Phase 4

---

## Timeline Summary

| Phase | Hours | Days (4h/day) | Start | End |
|-------|-------|---------------|-------|-----|
| 1. Setup | 2h | 0.5 | Day 1 AM | Day 1 PM |
| 2. Extract UI | 4h | 1.0 | Day 1 PM | Day 2 PM |
| 3. Services | 3h | 0.75 | Day 2 PM | Day 3 AM |
| 4. Polish | 2h | 0.5 | Day 3 AM | Day 3 PM |
| 5. Testing | 2h | 0.5 | Day 3 PM | Day 4 AM |
| 6. Docs | 1h | 0.25 | Day 4 AM | Day 4 AM |
| 7. Deprecate | 1h | 0.25 | Day 4 PM | Day 4 PM |
| **Total** | **15h** | **3.75 days** | | |

**With Buffer (+3h):** 18h = 4.5 days

---

## Success Metrics

### Completion Checklist

- [ ] All phases complete
- [ ] 10+ tests passing (90%+ coverage)
- [ ] Zero compiler warnings
- [ ] Documentation complete
- [ ] Legacy control obsolete
- [ ] NewDatabaseGenerator updated
- [ ] Manual testing passed
- [ ] Git commits clean

---

## Next Steps

1. **Review this plan** - Approve or adjust
2. **Create tasks.md** - Break down into tickets
3. **Set up project** - Phase 1
4. **Start extraction** - Phase 2
5. **Daily standups** - Track progress

---

**Document Status:** ? READY FOR EXECUTION  
**Last Updated:** 2026-01-05  
**Related:** spec.md, tasks.md, NEXT_GOALS.md

---

**End of Implementation Plan**
