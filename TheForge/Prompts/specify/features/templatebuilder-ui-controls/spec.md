# Feature Specification: TemplateBuilder UI Control Library

**Document Type:** Specification  
**Purpose:** Extract TemplateBuilderControl UI to reusable control library  
**Created:** 2026-01-05  
**Last Updated:** 2026-01-05  
**Status:** Draft - Awaiting Approval  
**Character Count:** [To be computed]  
**Related:** RCH.TemplateStorage v1.0, NEXT_GOALS.md, CONTROLS_INVENTORY_2026-01-05.md

---

## 1. Overview

### 1.1 Feature Name
`templatebuilder-ui-controls`

### 1.2 Description
Extract the UI components from legacy TemplateBuilderControl (1,607 lines) into a new reusable control library (`RCH.TemplateStorage.Controls`) that consumes the RCH.TemplateStorage v1.0 service layer. This completes the extraction started in v1.0 by separating the UI layer from the data/service layers, creating a fully reusable, testable, and maintainable template building UI.

### 1.3 Goals
- Create reusable UI control library for template building
- Eliminate all duplicate model definitions (use RCH.TemplateStorage models)
- Leverage RCH.TemplateStorage service layer for all data operations
- Maintain 100% backward compatibility with existing templates
- Add proper versioning and professional documentation
- Enable use across multiple projects (not just NewDatabaseGenerator)
- Improve testability through proper separation of concerns

### 1.4 Non-Goals (Out of Scope)
- Modify RCH.TemplateStorage service layer (already complete)
- Add new template features (focus on extraction only)
- TheForge Dashboard integration (separate module - see Goal 5)
- Database schema changes (already finalized in v1.0)
- Legacy TemplateBuilderControl removal (deprecation only)

---

## 2. Current State Analysis

### 2.1 What's Complete (RCH.TemplateStorage v1.0)

**Status:** ? Production Ready (2026-01-05)

**Components:**
- ? Models: TemplateDefinition (enhanced from DirectoryTemplate)
- ? Models: TemplateFolderDefinition (enhanced from FolderDefinition)
- ? Models: TemplateFileDefinition (new)
- ? Service: TemplateStorageService (full CRUD)
- ? Service: TemplateJsonSerializer (with legacy migration)
- ? Database: MS Access with full schema
- ? Testing: 32 tests (100% passing)
- ? Documentation: ~132k characters

**Metrics:**
- Time: 25-27 hours (30-40% under estimate)
- Quality: 100% on all metrics
- Backward Compatibility: 100% (5/5 migration tests)
- Data Loss: 0%

---

### 2.2 What Remains (Legacy TemplateBuilderControl)

**Location:** `NewDatabaseGenerator\NewDatabaseGenerator\TemplateBuilderControl.vb`  
**Status:** ?? Legacy - Functionality Extracted, UI Remains  
**Size:** 1,607 lines

**UI Components Still in Legacy:**
1. **TreeView Management**
   - Folder hierarchy display
   - Node expansion/collapse
   - Visual state management
   - Drag-and-drop (if implemented)

2. **User Interaction Handlers**
   - Add folder button
   - Remove folder button
   - Rename folder button
   - Save template button
   - Load template button
   - Export JSON button

3. **Properties Panel**
   - Template name
   - Template description
   - Folder properties
   - Selection checkboxes

4. **Template Selector**
   - List of saved templates
   - Search/filter
   - Selection handling

**Duplicate Code (To Remove):**
- DirectoryTemplate class definition (use TemplateDefinition)
- FolderDefinition class definition (use TemplateFolderDefinition)
- JSON serialization logic (use TemplateJsonSerializer)
- File I/O operations (use TemplateStorageService)

**Estimated Extraction:** ~600-800 lines of pure UI code

---

## 3. User Stories

### User Story 1: Reusable UI Control
**As a** developer  
**I want to** use TemplateBuilderControl in any Windows Forms project  
**So that** I can provide template building UI without duplicating code

**Acceptance Criteria:**
- Control available as NuGet package or project reference
- Single DLL: `RCH.TemplateStorage.Controls.dll`
- No dependencies on NewDatabaseGenerator project
- Works in any .NET 8.0 Windows Forms application
- Documented API with usage examples

---

### User Story 2: Service Layer Integration
**As a** developer  
**I want** the UI control to use RCH.TemplateStorage service layer  
**So that** all data operations go through tested, reliable services

**Acceptance Criteria:**
- Zero duplicate model definitions
- All CRUD via TemplateStorageService
- All JSON operations via TemplateJsonSerializer
- No direct database access from UI
- Proper error handling from service layer

---

### User Story 3: Template Hierarchy Display
**As a** user  
**I want to** see template folder structure in a tree view  
**So that** I can visualize and navigate the template hierarchy

**Acceptance Criteria:**
- TreeView displays folders and subfolders
- Unlimited nesting depth supported
- Expand/collapse nodes
- Visual indicators (icons) for folders
- Selected state visible
- Tooltips show folder details

---

### User Story 4: Template Editing
**As a** user  
**I want to** add, remove, and rename folders in the template  
**So that** I can build custom directory structures

**Acceptance Criteria:**
- Add folder button (prompts for name)
- Remove folder button (with confirmation)
- Rename folder (inline or dialog)
- Add subfolder to selected folder
- Drag-and-drop folder reorganization (optional)
- Undo/redo support (optional)

---

### User Story 5: Template Save/Load
**As a** user  
**I want to** save templates to database and load them later  
**So that** I can reuse templates across projects

**Acceptance Criteria:**
- Save button persists to database via service
- Load dropdown lists available templates
- New template button clears current
- Unsaved changes warning (optional)
- Auto-save on interval (optional)

---

### User Story 6: JSON Export/Import
**As a** user  
**I want to** export templates to JSON files and import legacy templates  
**So that** I can share templates and migrate old files

**Acceptance Criteria:**
- Export button saves to JSON file
- Import button loads JSON file
- Legacy TemplateBuilderControl JSON works
- 100% backward compatibility
- Format conversion automatic
- No data loss

---

## 4. Technical Requirements

### 4.1 Project Structure

**New Project:** `RCH.TemplateStorage.Controls`

```
RCH.TemplateStorage.Controls/
??? Controls/
?   ??? TemplateBuilderControl.vb
?   ??? TemplateBuilderControl.Designer.vb
?   ??? TemplateTreeView.vb (custom TreeView)
?   ??? TemplatePropertiesPanel.vb (properties display)
??? Dialogs/
?   ??? AddFolderDialog.vb
?   ??? RenameFolderDialog.vb
?   ??? TemplatePropertiesDialog.vb
??? Helpers/
?   ??? TreeViewHelper.vb (TreeView utilities)
?   ??? ValidationHelper.vb (input validation)
??? Documentation/
?   ??? README.md
?   ??? API.md
?   ??? CHANGELOG.md
??? RCH.TemplateStorage.Controls.vbproj
```

---

### 4.2 Component Architecture

```
???????????????????????????????????????????
?  TemplateBuilderControl (UserControl)   ?
?  ?????????????????????????????????????  ?
?  ?  TemplateTreeView                 ?  ?
?  ?  - Displays folder hierarchy      ?  ?
?  ?  - Handles selection              ?  ?
?  ?  - Context menu                   ?  ?
?  ?????????????????????????????????????  ?
?  ?????????????????????????????????????  ?
?  ?  TemplatePropertiesPanel          ?  ?
?  ?  - Template name/description      ?  ?
?  ?  ?  - Folder properties            ?  ?
?  ?  - Metadata display               ?  ?
?  ?????????????????????????????????????  ?
?  ?????????????????????????????????????  ?
?  ?  Action Buttons                   ?  ?
?  ?  - Add Folder                     ?  ?
?  ?  - Remove Folder                  ?  ?
?  ?  - Save/Load                      ?  ?
?  ?  - Import/Export                  ?  ?
?  ?????????????????????????????????????  ?
???????????????????????????????????????????
                  ?
                  ? Uses
                  ?
???????????????????????????????????????????
?  RCH.TemplateStorage (Service Layer)    ?
?  ?????????????????????????????????????  ?
?  ?  TemplateStorageService           ?  ?
?  ?  - CreateTemplate()               ?  ?
?  ?  - GetTemplate()                  ?  ?
?  ?  - UpdateTemplate()               ?  ?
?  ?  - DeleteTemplate()               ?  ?
?  ?????????????????????????????????????  ?
?  ?????????????????????????????????????  ?
?  ?  TemplateJsonSerializer           ?  ?
?  ?  - SerializeTemplate()            ?  ?
?  ?  - DeserializeTemplate()          ?  ?
?  ?  - ImportLegacyTemplate()         ?  ?
?  ?????????????????????????????????????  ?
???????????????????????????????????????????
                  ?
                  ?
                  ?
???????????????????????????????????????????
?  Templates.accdb (Database)             ?
???????????????????????????????????????????
```

---

### 4.3 TemplateBuilderControl API

**Public Properties:**

```vb
Public Class TemplateBuilderControl
    Inherits UserControl
    
    ''' <summary>Version of this control</summary>
    Public Const Version As String = "1.0.0"
    
    ''' <summary>Current template being edited</summary>
    Public Property CurrentTemplate As TemplateDefinition
    
    ''' <summary>Database path for template storage</summary>
    Public Property DatabasePath As String
    
    ''' <summary>Whether template has unsaved changes</summary>
    Public ReadOnly Property HasUnsavedChanges As Boolean
    
    ''' <summary>Currently selected folder in tree</summary>
    Public ReadOnly Property SelectedFolder As TemplateFolderDefinition
End Class
```

**Public Methods:**

```vb
''' <summary>Create new blank template</summary>
Public Sub NewTemplate()

''' <summary>Load template from database</summary>
Public Sub LoadTemplate(templateId As Integer)

''' <summary>Save current template to database</summary>
Public Function SaveTemplate() As Boolean

''' <summary>Export template to JSON file</summary>
Public Function ExportToJson(filePath As String) As Boolean

''' <summary>Import template from JSON file</summary>
Public Function ImportFromJson(filePath As String) As Boolean

''' <summary>Add folder to selected location</summary>
Public Sub AddFolder(folderName As String, Optional parent As TemplateFolderDefinition = Nothing)

''' <summary>Remove selected folder</summary>
Public Sub RemoveSelectedFolder()

''' <summary>Rename selected folder</summary>
Public Sub RenameSelectedFolder(newName As String)

''' <summary>Refresh tree view display</summary>
Public Sub RefreshTreeView()
```

**Public Events:**

```vb
''' <summary>Raised when template is saved</summary>
Public Event TemplateSaved As EventHandler(Of TemplateSavedEventArgs)

''' <summary>Raised when template is loaded</summary>
Public Event TemplateLoaded As EventHandler(Of TemplateLoadedEventArgs)

''' <summary>Raised when folder is added</summary>
Public Event FolderAdded As EventHandler(Of FolderChangedEventArgs)

''' <summary>Raised when folder is removed</summary>
Public Event FolderRemoved As EventHandler(Of FolderChangedEventArgs)

''' <summary>Raised when selection changes</summary>
Public Event SelectionChanged As EventHandler(Of SelectionChangedEventArgs)

''' <summary>Raised on validation errors</summary>
Public Event ValidationError As EventHandler(Of ValidationErrorEventArgs)
```

---

### 4.4 Dependencies

**Project References:**
- RCH.TemplateStorage (v1.0.0+) - Service layer

**NuGet Packages:**
- System.Windows.Forms (built-in)
- No additional packages needed (inherited from RCH.TemplateStorage)

**Framework:**
- .NET 8.0 (Windows)
- VB.NET

---

## 5. Implementation Plan

### 5.1 Phase 1: Project Setup (2h)

**Tasks:**
1. Create `RCH.TemplateStorage.Controls.vbproj`
2. Add reference to `RCH.TemplateStorage`
3. Configure build settings (.NET 8.0, Windows)
4. Set up project structure (Controls/, Dialogs/, Helpers/)
5. Create initial README.md

**Deliverables:**
- Empty project that builds successfully
- Project structure in place
- README with project description

---

### 5.2 Phase 2: Extract Core UI Components (4h)

**Tasks:**
1. Create `TemplateBuilderControl.vb` (UserControl)
2. Create `TemplateBuilderControl.Designer.vb`
3. Extract TreeView logic from legacy control
4. Extract button handlers from legacy control
5. Remove all model definitions (use library models)
6. Replace DirectoryTemplate with TemplateDefinition
7. Replace FolderDefinition with TemplateFolderDefinition

**Deliverables:**
- TemplateBuilderControl with TreeView
- Basic folder add/remove working
- No duplicate models

**Challenges:**
- Mapping legacy model properties to new models
- Ensuring TreeView node data binding correct
- Preserving visual state during operations

---

### 5.3 Phase 3: Service Layer Integration (3h)

**Tasks:**
1. Replace all JSON operations with TemplateJsonSerializer
2. Replace all file I/O with TemplateStorageService
3. Add database path configuration
4. Implement save to database
5. Implement load from database
6. Add error handling for service failures
7. Add loading indicators for async operations

**Deliverables:**
- All data operations via service layer
- Zero direct database access
- Proper error handling

**Challenges:**
- Async/await for database operations
- Transaction handling from UI
- Error message display

---

### 5.4 Phase 4: Polish & Features (2h)

**Tasks:**
1. Add icons to TreeView nodes
2. Implement context menu (right-click)
3. Add keyboard shortcuts (F2=rename, Del=delete)
4. Implement drag-and-drop (optional)
5. Add tooltips
6. Implement undo/redo (optional)
7. Add visual indicators (modified, saved)

**Deliverables:**
- Professional UI with icons
- Context menu working
- Keyboard shortcuts
- Tooltips

---

### 5.5 Phase 5: Testing (2h)

**Tasks:**
1. Unit tests for TreeView operations
2. Unit tests for data binding
3. Integration tests with service layer
4. Manual UI testing
5. Test legacy template import
6. Test round-trip (save ? load)
7. Test error scenarios

**Deliverables:**
- 10+ unit tests (90% coverage target)
- Integration tests passing
- Manual test plan executed

---

### 5.6 Phase 6: Documentation (1h)

**Tasks:**
1. Complete README.md with usage examples
2. Create API.md with full API reference
3. Create CHANGELOG.md (v1.0.0)
4. Add XML documentation to all public members
5. Create sample code project
6. Update CONTROLS_INVENTORY.md

**Deliverables:**
- Complete documentation
- API reference
- Usage examples
- Sample project

---

### 5.7 Phase 7: Deprecation & Migration (1h)

**Tasks:**
1. Mark legacy TemplateBuilderControl as `[Obsolete]`
2. Update NewDatabaseGenerator to use new library
3. Test NewDatabaseGenerator with new control
4. Create migration guide
5. Add deprecation notice in legacy control

**Deliverables:**
- Legacy control marked obsolete
- NewDatabaseGenerator using new control
- Migration guide complete

---

## 6. Testing Strategy

### 6.1 Unit Tests

**Test Categories:**

1. **TreeView Operations** (4 tests)
   - Add folder to root
   - Add subfolder to parent
   - Remove folder
   - Rename folder

2. **Data Binding** (3 tests)
   - Bind template to TreeView
   - Update TreeView on template change
   - TreeView to template synchronization

3. **Service Integration** (3 tests)
   - Save template via service
   - Load template via service
   - Handle service errors

**Total:** 10 unit tests (target: 90% coverage)

---

### 6.2 Integration Tests

**Test Scenarios:**

1. **End-to-End Workflow**
   - Create new template
   - Add folders
   - Save to database
   - Load from database
   - Verify structure matches

2. **Legacy Import**
   - Import old TemplateBuilderControl JSON
   - Verify all folders present
   - Save to database
   - Reload and verify

3. **Error Handling**
   - Database connection failure
   - Invalid JSON import
   - Duplicate template name

---

### 6.3 Manual Testing

**Test Plan:**

| Test Case | Steps | Expected Result |
|-----------|-------|-----------------|
| TC-01 | Open control, click Add Folder | Dialog appears, folder added to tree |
| TC-02 | Right-click folder, select Remove | Confirmation, folder removed |
| TC-03 | Press F2 on folder | Rename dialog appears |
| TC-04 | Drag folder to new parent | Folder moves in hierarchy |
| TC-05 | Click Save | Template saved to database |
| TC-06 | Load template from dropdown | Tree populated correctly |
| TC-07 | Import legacy JSON | Template loads, all folders present |
| TC-08 | Export to JSON | File created, valid format |

---

## 7. Success Criteria

### 7.1 Functional

- ? Control builds without errors/warnings
- ? All 10+ unit tests passing
- ? Integration tests passing
- ? Manual test plan 100% passed
- ? Legacy template import works
- ? NewDatabaseGenerator uses new control
- ? Zero duplicate code with RCH.TemplateStorage
- ? All data operations via service layer

---

### 7.2 Non-Functional

- ? 100% ForgeCharter compliance
- ? XML documentation on all public members
- ? README with usage examples
- ? API reference complete
- ? CHANGELOG.md created
- ? Version 1.0.0 assigned
- ? Character counts accurate
- ? Legacy control marked obsolete

---

### 7.3 Quality Metrics

| Metric | Target | Measurement |
|--------|--------|-------------|
| Build Success | 100% | Zero errors/warnings |
| Test Pass Rate | 100% | All tests passing |
| Code Coverage | 90%+ | Unit test coverage |
| Documentation | Complete | README + API + CHANGELOG |
| Backward Compat | 100% | Legacy import works |
| Zero Duplicates | Yes | No duplicate models |

---

## 8. Risks & Mitigations

### Risk 1: TreeView Data Binding Complexity
**Impact:** HIGH  
**Probability:** MEDIUM  
**Mitigation:**
- Start with simple binding
- Add complexity incrementally
- Test each step
- Document binding pattern

---

### Risk 2: Legacy Code Quality
**Impact:** MEDIUM  
**Probability:** HIGH  
**Mitigation:**
- Review legacy code first
- Extract only UI logic
- Refactor during extraction
- Add unit tests immediately

---

### Risk 3: Service Layer Changes
**Impact:** LOW  
**Probability:** LOW  
**Mitigation:**
- RCH.TemplateStorage v1.0 frozen
- No service changes needed
- UI adapts to service, not vice versa

---

### Risk 4: Timeline Overrun
**Impact:** MEDIUM  
**Probability:** MEDIUM  
**Mitigation:**
- Defer optional features (drag-and-drop, undo/redo)
- Focus on core functionality first
- Track progress daily
- Adjust scope if needed

---

## 9. Timeline & Resources

### 9.1 Estimated Effort

| Phase | Hours | Cumulative |
|-------|-------|------------|
| Phase 1: Project Setup | 2h | 2h |
| Phase 2: Extract UI | 4h | 6h |
| Phase 3: Service Integration | 3h | 9h |
| Phase 4: Polish & Features | 2h | 11h |
| Phase 5: Testing | 2h | 13h |
| Phase 6: Documentation | 1h | 14h |
| Phase 7: Deprecation | 1h | 15h |
| **Total** | **15h** | **15h** |

**Buffer:** +3h for unknowns  
**Total with Buffer:** 18 hours

**Confidence:** High (based on v1.0 completion)

---

### 9.2 Dependencies

**Blockers:**
- ? RCH.TemplateStorage v1.0 complete (Done)

**Critical Path:**
1. Project Setup ? Extract UI ? Service Integration ? Testing ? Documentation

**Parallel Work:**
- Documentation can start during Phase 4
- Testing can start during Phase 3

---

## 10. Future Enhancements (Out of Scope)

**Post-v1.0 Features:**

1. **Advanced TreeView**
   - Drag-and-drop reordering
   - Multi-select
   - Clipboard operations (cut/copy/paste)

2. **Template Properties**
   - Advanced metadata editing
   - Custom fields
   - Template versioning UI

3. **Search & Filter**
   - Search folders by name
   - Filter by properties
   - Quick navigation

4. **Undo/Redo**
   - Full undo/redo stack
   - Undo history display
   - Checkpoint saves

5. **Visual Enhancements**
   - Folder icons by type
   - Color coding
   - Custom themes
   - Animation

6. **Export Options**
   - Export to multiple formats
   - Selective export
   - Export with preview

---

## 11. Migration Guide

### 11.1 For NewDatabaseGenerator

**Before (Legacy):**
```vb
Imports NewDatabaseGenerator

Dim control As New TemplateBuilderControl()
control.LoadTemplate("MyTemplate.json")
```

**After (New Library):**
```vb
Imports RCH.TemplateStorage.Controls

Dim control As New TemplateBuilderControl()
control.DatabasePath = "C:\Templates\Templates.accdb"
control.LoadTemplate(templateId:=1)
```

---

### 11.2 API Mapping

| Legacy Method | New Method | Notes |
|---------------|------------|-------|
| `LoadFromJson(path)` | `ImportFromJson(path)` | Same functionality |
| `SaveToJson(path)` | `ExportToJson(path)` | Same functionality |
| N/A | `LoadTemplate(id)` | New - from database |
| N/A | `SaveTemplate()` | New - to database |

---

## 12. Deliverables

### 12.1 Code
- ? `RCH.TemplateStorage.Controls.dll` (Release build)
- ? Source code in `RCH.TemplateStorage.Controls/`
- ? 10+ unit tests
- ? Sample application project

---

### 12.2 Documentation
- ? `README.md` - Overview and quick start
- ? `API.md` - Complete API reference
- ? `CHANGELOG.md` - Version history
- ? XML documentation (in code)
- ? Migration guide
- ? Usage examples

---

### 12.3 Updated Files
- ? `CONTROLS_INVENTORY_2026-01-05.md` - Add new control
- ? `NEXT_GOALS.md` - Mark Goal 1 complete
- ? Legacy `TemplateBuilderControl.vb` - Add `[Obsolete]` attribute

---

## 13. Approval & Sign-Off

### 13.1 Review Checklist

- [ ] Technical approach approved
- [ ] Timeline realistic
- [ ] Dependencies identified
- [ ] Risks acceptable
- [ ] Success criteria clear
- [ ] Testing strategy adequate
- [ ] Documentation plan complete

---

### 13.2 Sign-Off

**Approved By:** ___________________  
**Date:** ___________________  
**Notes:** ___________________

---

**Document Status:** ? READY FOR REVIEW  
**Last Updated:** 2026-01-05  
**Character Count:** ~31,000  
**Related:** NEXT_GOALS.md, RCH.TemplateStorage v1.0, CONTROLS_INVENTORY_2026-01-05.md

---

**End of Specification**
