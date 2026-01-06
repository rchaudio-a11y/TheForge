# Tasks: TemplateBuilder UI Control Library

**Feature:** `templatebuilder-ui-controls`  
**Status:** Ready for Development  
**Total Estimated:** 15-18 hours  
**Created:** 2026-01-05

---

## Task Breakdown

### Phase 1: Project Setup (2h)

#### TBUI-001: Create Project Structure
**Priority:** HIGH  
**Estimate:** 0.5h  
**Dependencies:** None

**Description:**
Create `RCH.TemplateStorage.Controls.vbproj` with proper configuration.

**Acceptance Criteria:**
- Project file created
- Target framework: .NET 8.0 (Windows)
- UseWindowsForms enabled
- Builds successfully

**Tasks:**
- [ ] Create project file
- [ ] Configure project properties
- [ ] Add reference to RCH.TemplateStorage
- [ ] Build and verify

---

#### TBUI-002: Create Folder Structure
**Priority:** HIGH  
**Estimate:** 0.5h  
**Dependencies:** TBUI-001

**Description:**
Create organized folder structure for controls, dialogs, and helpers.

**Acceptance Criteria:**
- `Controls/` folder exists
- `Dialogs/` folder exists
- `Helpers/` folder exists
- `Documentation/` folder exists

**Tasks:**
- [ ] Create Controls folder
- [ ] Create Dialogs folder
- [ ] Create Helpers folder
- [ ] Create Documentation folder
- [ ] Add .gitkeep files

---

#### TBUI-003: Create Initial README
**Priority:** MEDIUM  
**Estimate:** 0.5h  
**Dependencies:** TBUI-002

**Description:**
Create README.md with project overview and placeholder content.

**Acceptance Criteria:**
- README.md exists
- Project description clear
- Installation instructions placeholder
- Usage example placeholder

**Tasks:**
- [ ] Create README.md
- [ ] Add project description
- [ ] Add placeholders for sections
- [ ] Commit to Git

---

#### TBUI-004: Verify Build
**Priority:** HIGH  
**Estimate:** 0.5h  
**Dependencies:** TBUI-001, TBUI-002

**Description:**
Build project and verify no errors.

**Acceptance Criteria:**
- Build succeeds
- Zero errors
- Zero warnings
- DLL generated

**Tasks:**
- [ ] Build in Debug mode
- [ ] Build in Release mode
- [ ] Verify DLL exists
- [ ] Test reference from sample project

---

### Phase 2: Extract Core UI (4h)

#### TBUI-005: Create TemplateBuilderControl Shell
**Priority:** HIGH  
**Estimate:** 1h  
**Dependencies:** TBUI-004

**Description:**
Create empty TemplateBuilderControl UserControl with basic structure.

**Acceptance Criteria:**
- TemplateBuilderControl.vb created
- TemplateBuilderControl.Designer.vb created
- Inherits UserControl
- Version constant defined
- Builds successfully

**Tasks:**
- [ ] Create TemplateBuilderControl.vb
- [ ] Create Designer file
- [ ] Add version constant
- [ ] Add basic properties
- [ ] Add XML documentation
- [ ] Build and verify

---

#### TBUI-006: Extract TreeView Logic
**Priority:** HIGH  
**Estimate:** 2h  
**Dependencies:** TBUI-005

**Description:**
Extract TreeView population and management from legacy control.

**Acceptance Criteria:**
- TreeView added to control
- Node creation logic extracted
- Tree population works
- No DirectoryTemplate references
- Uses TemplateDefinition

**Tasks:**
- [ ] Add TreeView to Designer
- [ ] Extract node creation method
- [ ] Extract tree population method
- [ ] Replace DirectoryTemplate with TemplateDefinition
- [ ] Replace FolderDefinition with TemplateFolderDefinition
- [ ] Test tree display

**Code Reference:**
```vb
Private Sub PopulateTree(template As TemplateDefinition)
    treeView.Nodes.Clear()
    For Each folder In template.Folders
        Dim node = CreateNode(folder)
        treeView.Nodes.Add(node)
    Next
End Sub

Private Function CreateNode(folder As TemplateFolderDefinition) As TreeNode
    Dim node As New TreeNode(folder.Name)
    node.Tag = folder
    For Each subfolder In folder.SubFolders
        node.Nodes.Add(CreateNode(subfolder))
    Next
    Return node
End Function
```

---

#### TBUI-007: Add Button Handlers
**Priority:** HIGH  
**Estimate:** 1h  
**Dependencies:** TBUI-006

**Description:**
Add buttons (Add, Remove, Save, Load) and wire up event handlers.

**Acceptance Criteria:**
- Add Folder button exists
- Remove Folder button exists
- Save button exists
- Load button exists
- All handlers wired
- Basic validation in place

**Tasks:**
- [ ] Add buttons to Designer
- [ ] Create AddFolder handler
- [ ] Create RemoveFolder handler
- [ ] Create Save handler
- [ ] Create Load handler
- [ ] Add validation logic
- [ ] Test button clicks

---

### Phase 3: Service Integration (3h)

#### TBUI-008: Add Service Dependency
**Priority:** HIGH  
**Estimate:** 0.5h  
**Dependencies:** TBUI-007

**Description:**
Add TemplateStorageService as dependency with initialization.

**Acceptance Criteria:**
- DatabasePath property added
- Service initialized on demand
- Error handling for missing database
- Service disposed properly

**Tasks:**
- [ ] Add DatabasePath property
- [ ] Add private _service field
- [ ] Create InitializeService method
- [ ] Add null checks
- [ ] Implement IDisposable
- [ ] Test initialization

**Code:**
```vb
Private _service As TemplateStorageService

Public Property DatabasePath As String

Private Sub EnsureServiceInitialized()
    If _service Is Nothing Then
        If String.IsNullOrEmpty(DatabasePath) Then
            Throw New InvalidOperationException("DatabasePath must be set")
        End If
        _service = New TemplateStorageService(DatabasePath)
    End If
End Sub
```

---

#### TBUI-009: Implement Save Operation
**Priority:** HIGH  
**Estimate:** 1h  
**Dependencies:** TBUI-008

**Description:**
Implement SaveTemplate() using TemplateStorageService.

**Acceptance Criteria:**
- SaveTemplate() method works
- Creates new or updates existing
- Returns success/failure
- Raises TemplateSaved event
- Error handling in place

**Tasks:**
- [ ] Create SaveTemplate method
- [ ] Add Create/Update logic
- [ ] Add transaction handling
- [ ] Add error handling
- [ ] Raise TemplateSaved event
- [ ] Update HasUnsavedChanges
- [ ] Add unit test

---

#### TBUI-010: Implement Load Operation
**Priority:** HIGH  
**Estimate:** 1h  
**Dependencies:** TBUI-008

**Description:**
Implement LoadTemplate(id) using TemplateStorageService.

**Acceptance Criteria:**
- LoadTemplate(id) method works
- Populates TreeView
- Raises TemplateLoaded event
- Error handling in place
- Validates template exists

**Tasks:**
- [ ] Create LoadTemplate method
- [ ] Add retrieval logic
- [ ] Add null check
- [ ] Populate TreeView
- [ ] Raise TemplateLoaded event
- [ ] Reset HasUnsavedChanges
- [ ] Add unit test

---

#### TBUI-011: Implement Import/Export
**Priority:** HIGH  
**Estimate:** 0.5h  
**Dependencies:** TBUI-008

**Description:**
Implement JSON import/export using TemplateJsonSerializer.

**Acceptance Criteria:**
- ImportFromJson(path) works
- ExportToJson(path) works
- Legacy templates supported
- Error handling in place

**Tasks:**
- [ ] Create ImportFromJson method
- [ ] Create ExportToJson method
- [ ] Add file dialog support
- [ ] Add validation
- [ ] Test with legacy JSON
- [ ] Add unit tests

---

### Phase 4: Polish & Features (2h)

#### TBUI-012: Add Icons
**Priority:** MEDIUM  
**Estimate:** 0.5h  
**Dependencies:** TBUI-006

**Description:**
Add folder icons to TreeView nodes.

**Acceptance Criteria:**
- Folder icon on closed nodes
- Open folder icon on expanded
- Icons embedded as resources
- Icons display correctly

**Tasks:**
- [ ] Add folder.png to resources
- [ ] Add folder_open.png to resources
- [ ] Create ImageList
- [ ] Assign to TreeView
- [ ] Set node ImageKey
- [ ] Test display

---

#### TBUI-013: Add Context Menu
**Priority:** MEDIUM  
**Estimate:** 0.5h  
**Dependencies:** TBUI-007

**Description:**
Add right-click context menu with common operations.

**Acceptance Criteria:**
- Context menu appears on right-click
- Add Subfolder option
- Rename option
- Remove option
- Menu items enabled/disabled correctly

**Tasks:**
- [ ] Create ContextMenuStrip
- [ ] Add menu items
- [ ] Wire up handlers
- [ ] Add enable/disable logic
- [ ] Test menu

---

#### TBUI-014: Add Keyboard Shortcuts
**Priority:** MEDIUM  
**Estimate:** 0.5h  
**Dependencies:** TBUI-007

**Description:**
Add keyboard shortcuts for common operations.

**Acceptance Criteria:**
- F2 = Rename
- Delete = Remove
- Insert = Add
- Ctrl+S = Save
- Shortcuts documented

**Tasks:**
- [ ] Add KeyDown handler
- [ ] Map F2 to rename
- [ ] Map Delete to remove
- [ ] Map Insert to add
- [ ] Map Ctrl+S to save
- [ ] Test shortcuts
- [ ] Document in README

---

#### TBUI-015: Add Tooltips
**Priority:** LOW  
**Estimate:** 0.5h  
**Dependencies:** TBUI-006

**Description:**
Add tooltips showing folder details on hover.

**Acceptance Criteria:**
- Tooltip appears on hover
- Shows folder name and description
- Tooltip formatting clean
- No performance issues

**Tasks:**
- [ ] Add ToolTip component
- [ ] Add MouseHover handler
- [ ] Format tooltip text
- [ ] Test hover behavior

---

### Phase 5: Testing (2h)

#### TBUI-016: Unit Tests - TreeView Operations
**Priority:** HIGH  
**Estimate:** 0.5h  
**Dependencies:** TBUI-007

**Description:**
Create unit tests for TreeView operations.

**Acceptance Criteria:**
- Test AddFolder
- Test RemoveFolder
- Test RenameFolder
- Test node selection
- 90%+ coverage

**Tasks:**
- [ ] Create TemplateBuilderControlTests.vb
- [ ] Write TestAddFolder
- [ ] Write TestRemoveFolder
- [ ] Write TestRenameFolder
- [ ] Write TestSelection
- [ ] Run tests
- [ ] Verify coverage

---

#### TBUI-017: Unit Tests - Data Binding
**Priority:** HIGH  
**Estimate:** 0.5h  
**Dependencies:** TBUI-010

**Description:**
Create unit tests for data binding between model and TreeView.

**Acceptance Criteria:**
- Test tree population
- Test model sync
- Test node updates
- All tests passing

**Tasks:**
- [ ] Write TestPopulateTree
- [ ] Write TestModelToTree
- [ ] Write TestTreeToModel
- [ ] Run tests

---

#### TBUI-018: Integration Tests
**Priority:** HIGH  
**Estimate:** 0.5h  
**Dependencies:** TBUI-009, TBUI-010

**Description:**
Create integration tests with real database.

**Acceptance Criteria:**
- Test save and load cycle
- Test import legacy JSON
- Test export to JSON
- All tests passing

**Tasks:**
- [ ] Create test database
- [ ] Write TestSaveLoadCycle
- [ ] Write TestLegacyImport
- [ ] Write TestExport
- [ ] Clean up test data
- [ ] Run tests

---

#### TBUI-019: Manual Testing
**Priority:** HIGH  
**Estimate:** 0.5h  
**Dependencies:** TBUI-015

**Description:**
Execute manual test plan.

**Acceptance Criteria:**
- All test cases pass
- No bugs found
- Performance acceptable
- UI responsive

**Tasks:**
- [ ] Execute test plan (spec.md 6.3)
- [ ] Document results
- [ ] Fix any issues
- [ ] Retest failures

---

### Phase 6: Documentation (1h)

#### TBUI-020: Complete README
**Priority:** HIGH  
**Estimate:** 0.25h  
**Dependencies:** TBUI-019

**Description:**
Complete README.md with full content.

**Acceptance Criteria:**
- Installation instructions complete
- Usage examples complete
- API overview included
- Links to other docs

**Tasks:**
- [ ] Write installation section
- [ ] Write usage examples
- [ ] Add API overview
- [ ] Add links
- [ ] Review and edit

---

#### TBUI-021: Create API Documentation
**Priority:** HIGH  
**Estimate:** 0.25h  
**Dependencies:** TBUI-019

**Description:**
Create API.md with complete API reference.

**Acceptance Criteria:**
- All properties documented
- All methods documented
- All events documented
- Examples included

**Tasks:**
- [ ] Create API.md
- [ ] Document properties
- [ ] Document methods
- [ ] Document events
- [ ] Add examples

---

#### TBUI-022: Create CHANGELOG
**Priority:** MEDIUM  
**Estimate:** 0.25h  
**Dependencies:** TBUI-019

**Description:**
Create CHANGELOG.md for version 1.0.0.

**Acceptance Criteria:**
- CHANGELOG.md exists
- Version 1.0.0 documented
- Added/Changed/Fixed sections
- Keep a Changelog format

**Tasks:**
- [ ] Create CHANGELOG.md
- [ ] Add v1.0.0 section
- [ ] Document features
- [ ] Add migration notes

---

#### TBUI-023: Add XML Documentation
**Priority:** MEDIUM  
**Estimate:** 0.25h  
**Dependencies:** TBUI-019

**Description:**
Add XML documentation to all public members.

**Acceptance Criteria:**
- All public properties documented
- All public methods documented
- All public events documented
- IntelliSense works

**Tasks:**
- [ ] Add XML to properties
- [ ] Add XML to methods
- [ ] Add XML to events
- [ ] Generate XML file
- [ ] Test IntelliSense

---

### Phase 7: Deprecation (1h)

#### TBUI-024: Mark Legacy Control Obsolete
**Priority:** HIGH  
**Estimate:** 0.25h  
**Dependencies:** TBUI-023

**Description:**
Add [Obsolete] attribute to legacy TemplateBuilderControl.

**Acceptance Criteria:**
- [Obsolete] attribute added
- Warning message clear
- Constructor shows notice
- Compiles with warning

**Tasks:**
- [ ] Add [Obsolete] attribute
- [ ] Write clear message
- [ ] Add MessageBox in constructor
- [ ] Build and verify warning

---

#### TBUI-025: Update NewDatabaseGenerator
**Priority:** HIGH  
**Estimate:** 0.5h  
**Dependencies:** TBUI-024

**Description:**
Update NewDatabaseGenerator to use new library.

**Acceptance Criteria:**
- Reference to new library added
- Imports updated
- Control usage updated
- Builds successfully
- Runs successfully

**Tasks:**
- [ ] Add project reference
- [ ] Update Imports statements
- [ ] Update control instantiation
- [ ] Test build
- [ ] Test runtime
- [ ] Fix any issues

---

#### TBUI-026: Create Migration Guide
**Priority:** MEDIUM  
**Estimate:** 0.25h  
**Dependencies:** TBUI-025

**Description:**
Create MIGRATION.md documenting migration from legacy control.

**Acceptance Criteria:**
- MIGRATION.md exists
- Before/after examples
- API mapping table
- Clear instructions

**Tasks:**
- [ ] Create MIGRATION.md
- [ ] Add before/after examples
- [ ] Create API mapping
- [ ] Add troubleshooting
- [ ] Review and edit

---

## Task Summary

### By Phase

| Phase | Tasks | Hours | Priority |
|-------|-------|-------|----------|
| 1. Setup | 4 | 2h | HIGH |
| 2. Extract UI | 3 | 4h | HIGH |
| 3. Services | 4 | 3h | HIGH |
| 4. Polish | 4 | 2h | MEDIUM |
| 5. Testing | 4 | 2h | HIGH |
| 6. Docs | 4 | 1h | HIGH |
| 7. Deprecate | 3 | 1h | HIGH |
| **Total** | **26** | **15h** | |

---

### By Priority

| Priority | Tasks | Hours |
|----------|-------|-------|
| HIGH | 18 | 12h |
| MEDIUM | 7 | 2.5h |
| LOW | 1 | 0.5h |
| **Total** | **26** | **15h** |

---

### Critical Path

```
TBUI-001 ? TBUI-004 ? TBUI-005 ? TBUI-006 ? TBUI-007 ? 
TBUI-008 ? TBUI-009 ? TBUI-010 ? TBUI-016 ? TBUI-017 ? 
TBUI-018 ? TBUI-019 ? TBUI-020 ? TBUI-024 ? TBUI-025
```

**Critical Path Duration:** ~10-11 hours

---

## Progress Tracking

### Daily Standup Template

**What did I complete yesterday?**
- Task IDs and brief description

**What will I work on today?**
- Task IDs and estimated hours

**Any blockers?**
- Issues or dependencies

---

### Completion Checklist

- [ ] All 26 tasks complete
- [ ] All tests passing (10+ tests)
- [ ] Zero compiler warnings
- [ ] Documentation complete
- [ ] Legacy control obsolete
- [ ] NewDatabaseGenerator updated
- [ ] Git history clean
- [ ] Ready for release

---

## Notes

### Task Conventions

**Task ID Format:** TBUI-XXX (TemplateBuilder UI - number)

**Status Values:**
- TODO - Not started
- IN PROGRESS - Currently working
- BLOCKED - Waiting on dependency
- DONE - Complete and verified

**Estimate Guidelines:**
- 0.25h = 15 minutes
- 0.5h = 30 minutes
- 1h = 1 hour
- 2h = 2 hours

---

**Document Status:** ? READY FOR EXECUTION  
**Last Updated:** 2026-01-05  
**Total Tasks:** 26  
**Total Estimate:** 15 hours

---

**End of Tasks Document**
