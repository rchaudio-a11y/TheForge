# ForgeCharter Amendment Proposal: Reusable Control Versioning

**Document Type:** Amendment Proposal  
**Created:** 2025-01-02  
**Character Count:** TBD  
**Status:** Draft - Awaiting Approval  
**Proposes Amendment To:** ForgeCharter.md Section 12 (NEW)  
**Related:** Branch-Architecture.md, Branch-Coding.md

---

## Executive Summary

**Proposal:** Add Section 12 to ForgeCharter to establish governance for versioning reusable UI controls.

**Problem:** Current ForgeCharter lacks explicit rules for:
- Versioning reusable UserControls
- Managing breaking changes in controls
- Deprecating old control versions
- Maintaining backward compatibility
- Tracking control evolution

**Impact:** Without versioning governance, reusable controls may evolve inconsistently, break dependent projects, or accumulate technical debt.

---

## Proposed Amendment Text

### Add to ForgeCharter.md as Section 12:

```markdown
# 12. Reusable Control Versioning Governance

The Forge must enforce version management for all reusable UI controls to ensure stability, traceability, and backward compatibility.

## 12.1 Scope
This section applies to:
- UserControls intended for reuse across projects
- Custom controls with external dependencies
- Control libraries or packages
- Shared UI components

This section does NOT apply to:
- Project-specific controls (not reusable)
- Internal helper controls (not exposed)
- One-time-use controls

---

## 12.2 Version Identification

### 12.2.1 Semantic Versioning Required
All reusable controls MUST follow semantic versioning (SemVer):

**Format:** MAJOR.MINOR.PATCH

- **MAJOR:** Breaking changes (incompatible API changes)
- **MINOR:** New features (backward-compatible)
- **PATCH:** Bug fixes (backward-compatible)

**Examples:**
- `ModuleListControl v1.0.0` (initial release)
- `ModuleListControl v1.1.0` (added search feature)
- `ModuleListControl v1.1.1` (fixed layout bug)
- `ModuleListControl v2.0.0` (renamed properties - breaking)

---

### 12.2.2 Version Metadata Required
Every reusable control MUST include version metadata:

**In code (main .vb file):**
```vb
''' <summary>
''' ModuleListControl - Displays a searchable list of modules
''' </summary>
''' <version>1.2.0</version>
''' <changelog>
''' 1.2.0 - Added filter by status (2025-01-02)
''' 1.1.0 - Added search functionality (2024-12-15)
''' 1.0.0 - Initial release (2024-11-01)
''' </changelog>
Public Class ModuleListControl
    Inherits UserControl
    
    Public Const Version As String = "1.2.0"
```

**In documentation (Codex):**
```markdown
# ModuleListControl

**Version:** 1.2.0
**Status:** Stable
**Created:** 2024-11-01
**Last Updated:** 2025-01-02

## Version History
- 1.2.0 (2025-01-02) - Added filter by status
- 1.1.0 (2024-12-15) - Added search functionality
- 1.0.0 (2024-11-01) - Initial release
```

---

## 12.3 Breaking Change Management

### 12.3.1 Breaking Changes Require MAJOR Version Bump
Changes that break existing usage MUST increment MAJOR version:

**Breaking changes include:**
- Renaming public properties/methods/events
- Changing property types
- Removing public members
- Changing event signatures
- Changing constructor parameters
- Changing required dependencies

**Example:**
```vb
' v1.x.x - OLD (breaking change ahead)
Public Property SelectedItem As String

' v2.0.0 - NEW (renamed property)
Public Property SelectedModule As String
```

---

### 12.3.2 Deprecation Policy
Before removing functionality:

1. **Mark as Obsolete** (at least one MINOR version)
2. **Document alternative** (in ObsoleteAttribute message)
3. **Update changelog** (explain migration path)
4. **Remove in next MAJOR** (after deprecation period)

**Example:**
```vb
' v1.5.0 - Deprecate old property
<Obsolete("Use SelectedModule instead. Will be removed in v2.0.0")>
Public Property SelectedItem As String
    Get
        Return SelectedModule
    End Get
    Set(value As String)
        SelectedModule = value
    End Set
End Property

' v2.0.0 - Remove deprecated property
' (Property removed entirely)
```

---

### 12.3.3 Breaking Change Documentation
All breaking changes MUST be documented:

**In Chronicle:**
```markdown
# Breaking Changes - ModuleListControl v2.0.0

**Release Date:** 2025-03-01

## Removed
- `SelectedItem` property (use `SelectedModule` instead)

## Changed
- `LoadModules()` now requires `IModuleFilter` parameter

## Migration Guide
Update code as follows:

```vb
' OLD (v1.x)
control.SelectedItem = "Module1"
control.LoadModules()

' NEW (v2.0)
control.SelectedModule = "Module1"
control.LoadModules(New DefaultModuleFilter())
```
```

---

## 12.4 Backward Compatibility

### 12.4.1 Preserve Compatibility in MINOR/PATCH
MINOR and PATCH releases MUST maintain backward compatibility:

**Allowed in MINOR:**
- New optional properties (with defaults)
- New methods (not required)
- New events
- Performance improvements
- Internal refactoring
- Bug fixes

**Allowed in PATCH:**
- Bug fixes only
- No API changes
- No new features
- Internal fixes

**Example (v1.1.0 - backward compatible):**
```vb
' v1.0.0 - Original
Public Class ModuleListControl
    Public Property Items As List(Of String)
End Class

' v1.1.0 - Added search (backward compatible)
Public Class ModuleListControl
    Public Property Items As List(Of String)
    
    ' New optional property (has default)
    Public Property SearchEnabled As Boolean = True
    
    ' New optional method
    Public Sub Search(query As String)
        ' Implementation
    End Sub
End Class

' OLD code still works (no changes needed)
Dim control As New ModuleListControl()
control.Items = myList ' Still works!
```

---

### 12.4.2 Default Values Required
New optional properties MUST have sensible defaults:

**Good:**
```vb
Public Property AutoRefresh As Boolean = True  ' Clear default
Public Property MaxItems As Integer = 100       ' Safe default
```

**Bad:**
```vb
Public Property AutoRefresh As Boolean  ' No default (breaks compat)
Public Property MaxItems As Integer     ' Defaults to 0 (unexpected)
```

---

## 12.5 Version Documentation

### 12.5.1 Changelog Required
Every control MUST maintain a changelog:

**Location:** `Documentation/Codex/Controls/`

**Format:**
```markdown
# ModuleListControl Changelog

## [1.2.0] - 2025-01-02
### Added
- Filter by status feature
- Status icons in list items

### Changed
- Improved search performance (50% faster)

### Fixed
- Layout bug when resizing window

## [1.1.0] - 2024-12-15
### Added
- Search functionality
- Search box with live filtering

## [1.0.0] - 2024-11-01
### Added
- Initial release
- Basic list display
- Item selection
```

---

### 12.5.2 API Documentation Required
Every control MUST document its public API:

**Include:**
- All public properties (with types, defaults, purpose)
- All public methods (with parameters, return types, behavior)
- All public events (with event args)
- Usage examples
- Version introduced

**Example:**
```markdown
## Public API

### Properties

**Items** `List(Of String)` (since v1.0.0)
- Gets or sets the list of items to display
- Default: Empty list

**SearchEnabled** `Boolean` (since v1.1.0)
- Enables or disables search functionality
- Default: True

### Methods

**LoadModules(filter As IModuleFilter)** (since v2.0.0)
- Loads modules using the specified filter
- Parameters:
  - filter: Filter to apply when loading
- Returns: void

### Events

**ItemSelected** `EventHandler(Of ItemSelectedEventArgs)` (since v1.0.0)
- Raised when user selects an item
- Event args: ItemSelectedEventArgs (contains SelectedItem)
```

---

## 12.6 Version Compatibility Matrix

### 12.6.1 Maintain Compatibility Matrix
For controls with dependencies, maintain a matrix:

**Example:**
```markdown
# ModuleListControl Compatibility

| Control Version | .NET Version | TheForge Version | Breaking Changes |
|-----------------|--------------|------------------|------------------|
| 2.0.0          | .NET 4.8+    | v0.9.0+          | Yes (see migration) |
| 1.2.0          | .NET 4.7.2+  | v0.8.0+          | No |
| 1.1.0          | .NET 4.7.2+  | v0.8.0+          | No |
| 1.0.0          | .NET 4.7.2+  | v0.7.0+          | No |
```

---

### 12.6.2 Dependency Version Pinning
Controls MUST specify minimum versions of dependencies:

**In code:**
```vb
''' <requires>
''' TheForge.Core >= 0.9.0
''' .NET Framework >= 4.8
''' </requires>
```

---

## 12.7 Forge Enforcement Rules

### 12.7.1 Version Check on Control Creation
When creating a reusable control, Forge MUST:
- Prompt for initial version (default: 1.0.0)
- Create version metadata in code
- Create changelog file
- Create API documentation file

### 12.7.2 Version Check on Control Modification
When modifying a reusable control, Forge MUST:
- Detect if change is breaking (require MAJOR bump)
- Detect if change adds features (suggest MINOR bump)
- Detect if change is bug fix only (suggest PATCH bump)
- Update version constant in code
- Update changelog
- Update API documentation if public API changed

### 12.7.3 Deprecation Enforcement
Forge MUST:
- Enforce ObsoleteAttribute on deprecated members
- Require deprecation notice (at least one MINOR version)
- Prevent removal without prior deprecation (unless MAJOR bump)
- Document migration path in ObsoleteAttribute message

### 12.7.4 Audit Compliance
ForgeAudit MUST verify:
- Version metadata present in code
- Version format follows SemVer
- Changelog exists and is current
- API documentation exists
- Breaking changes properly versioned (MAJOR bump)
- Deprecated members have ObsoleteAttribute
- Compatibility matrix current

---

## 12.8 Branch Responsibilities

### Coding Branch
- Enforces version metadata in code
- Enforces ObsoleteAttribute usage
- Generates version constants
- Detects breaking changes

### Architecture Branch
- Enforces folder structure for control documentation
- Enforces naming canon for control versions
- Manages compatibility matrix

### Documentation Branch
- Enforces changelog format
- Enforces API documentation format
- Manages version history documentation

### Audit Branch
- Validates version compliance
- Detects missing version metadata
- Detects inconsistent versioning
- Reports deprecated members without ObsoleteAttribute

---

## 12.9 Examples

### Example 1: Adding Optional Feature (MINOR bump)
```vb
' v1.0.0 - Original
Public Class SearchControl
    Public Property Placeholder As String = "Search..."
End Class

' v1.1.0 - Added case sensitivity (backward compatible)
Public Class SearchControl
    Public Property Placeholder As String = "Search..."
    Public Property CaseSensitive As Boolean = False  ' New, has default
End Class

' OLD code still works
Dim control As New SearchControl()
control.Placeholder = "Find modules"  ' Works in both versions
```

---

### Example 2: Bug Fix (PATCH bump)
```vb
' v1.1.0 - Bug present
Public Sub Search(query As String)
    ' BUG: Doesn't handle null query
    Results = Items.Where(Function(i) i.Contains(query)).ToList()
End Sub

' v1.1.1 - Bug fixed (backward compatible)
Public Sub Search(query As String)
    ' FIX: Handle null query
    If String.IsNullOrEmpty(query) Then
        Results = Items
        Return
    End If
    Results = Items.Where(Function(i) i.Contains(query)).ToList()
End Sub

' Same API, just more robust
```

---

### Example 3: Breaking Change (MAJOR bump)
```vb
' v1.x.x - OLD API
Public Class DataGrid
    Public Property Columns As List(Of String)  ' Simple strings
End Class

' v2.0.0 - NEW API (breaking)
Public Class DataGrid
    Public Property Columns As List(Of ColumnDefinition)  ' Rich objects
    
    ' Changed type = breaking change = MAJOR bump required
End Class

' Deprecation path (v1.9.0):
<Obsolete("Use Columns property with ColumnDefinition objects. Will be removed in v2.0.0")>
Public Property ColumnNames As List(Of String)
    Get
        Return Columns.Select(Function(c) c.Name).ToList()
    End Get
End Property
```

---

## 12.10 Non-Requirements

### This section does NOT require:
- Versioning for project-specific controls (non-reusable)
- Versioning for internal helper controls
- Changelog for private implementation details
- API documentation for private/internal members
- Compatibility matrix for single-project controls

### The Forge must NOT:
- Auto-increment versions without user confirmation
- Modify version numbers implicitly
- Remove version metadata
- Skip changelog updates
- Override user version decisions

---

# End of Proposed Amendment
```

---

## Rationale

### Why This Amendment is Needed

**Problem 1: Untracked Evolution**
- Controls evolve without version tracking
- Breaking changes introduced without notice
- Dependent projects break unexpectedly

**Problem 2: Inconsistent Practices**
- No standard for when to bump versions
- No standard for deprecation
- No standard for compatibility

**Problem 3: Poor Maintainability**
- Hard to know what changed between versions
- Hard to debug version-specific issues
- Hard to maintain backward compatibility

**Problem 4: Forge Enforcement Gap**
- No automated checks for version compliance
- No guidance on versioning decisions
- No audit capability for version drift

---

## Benefits of This Amendment

### For Users
? **Predictable updates** - Know when breaking changes occur  
? **Clear migration paths** - Documented upgrade procedures  
? **Backward compatibility** - Old code keeps working  
? **Better debugging** - Version-specific issue tracking  

### For Maintainers
? **Clear versioning rules** - No guesswork  
? **Enforced deprecation** - Gradual evolution  
? **Automated checks** - Forge/audit enforcement  
? **Better documentation** - Changelog and API docs required  

### For The Forge System
? **Consistent governance** - Extends existing rules  
? **Audit capability** - ForgeAudit can check compliance  
? **Branch alignment** - Clear responsibilities  
? **Future-proof** - Handles control evolution  

---

## Implementation Impact

### Changes Required

**ForgeCharter.md:**
- Add Section 12 (this proposal)

**Branch-Coding.md:**
- Add version metadata generation rules
- Add ObsoleteAttribute enforcement rules
- Add breaking change detection rules

**Branch-Architecture.md:**
- Add control documentation folder rules
- Add compatibility matrix rules

**Branch-Documentation.md:**
- Add changelog format rules
- Add API documentation format rules

**ForgeAudit.md:**
- Add version compliance checks
- Add deprecation checks
- Add breaking change detection

### Migration Plan

**Phase 1:** Add Section 12 to ForgeCharter (this proposal)  
**Phase 2:** Update branches with specific rules  
**Phase 3:** Update ForgeAudit with version checks  
**Phase 4:** Document existing controls with versions  
**Phase 5:** Validate all controls comply  

**Estimated time:** 2-3 hours

---

## Questions for Review

1. ? Approve adding Section 12 to ForgeCharter?
2. ? Approve semantic versioning requirement?
3. ? Approve deprecation policy (one MINOR version notice)?
4. ? Approve changelog/API documentation requirements?
5. ? Approve Forge enforcement rules?

---

**Approval Status:** ? Awaiting Decision

**Next Steps:**
1. Review this proposal
2. Approve or request changes
3. Update ForgeCharter.md with Section 12
4. Update affected branches
5. Update ForgeAudit.md

**Related Documentation:**
- ForgeCharter.md (will be amended)
- Branch-Coding.md (will need updates)
- Branch-Architecture.md (will need updates)
- Branch-Documentation.md (will need updates)
- ForgeAudit.md (will need updates)
