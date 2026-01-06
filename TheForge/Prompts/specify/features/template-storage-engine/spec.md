# Feature Specification: Template Storage Engine

**Document Type:** Specification  
**Purpose:** Define requirements for database-backed template storage system with integrated database management  
**Created:** 2026-01-05  
**Last Updated:** 2026-01-05  
**Status:** Draft - Ready for Step 3 (plan.md Update)  
**Character Count:** 21265  
**Related:** RCH.TemplateGenerator project, AccessSqlGeneratorControl, RCHAutomation.Controls, Spec 2 (Governance Automation Engine), INTEGRATION_ANALYSIS.md

---

## 1. Overview

### 1.1 Feature Name
`template-storage-engine`

### 1.2 Description
Create a reusable library (`RCH.TemplateStorage`) that provides database-backed storage and retrieval of template definitions using Microsoft Access. The system must integrate AccessSqlGeneratorControl for direct database modification and maintenance, allowing users to create, edit, and manage templates through both programmatic APIs and interactive SQL editing.

### 1.3 Goals
- Provide persistent storage for template definitions
- Enable CRUD operations on templates via clean API
- Support JSON serialization/deserialization of templates
- Integrate AccessSqlGeneratorControl for database management
- **Store Manifest.md template structure for directory documentation**
- **Maintain compatibility with existing TemplateBuilderControl templates**
- **Extract and enhance proven data models from production code**
- **Leverage existing JSON serialization and state management patterns**
- Create reusable foundation for all future TemplateGenerator components (including automation engine)
- Maintain 100% ForgeCharter compliance from inception

### 1.4 Non-Goals (Out of Scope)
- UI for template editing (Spec 4)
- File/folder scaffolding execution (Spec 2)
- **Automated manifest generation/update (Spec 2 - Governance Automation Engine)**
- **Directory scanning and header injection (Spec 2 - Governance Automation Engine)**
- Forge Dashboard integration (Spec 5)

---

## 2. User Stories

### User Story 1: Database Management
**As a** developer  
**I want to** use AccessSqlGeneratorControl in a dedicated tab  
**So that** I can manage the template database directly while working with templates

**Acceptance Criteria:**
- AccessSqlGeneratorControl displayed in dedicated tab/panel
- Can create/modify template tables via SQL
- Can inspect database schema
- Changes persist to .accdb file
- SQL templates available for common operations
- Tab remains accessible while working with templates

---

### User Story 2: Template Definition Storage
**As a** developer  
**I want to** store comprehensive template definitions with Manifest.md structure in Access database  
**So that** templates can generate complete directory structures with governance documentation

**Acceptance Criteria:**
- Template table with most complete metadata header created to date:
  - TemplateID, Name, Description, Version
  - CreatedDate, ModifiedDate, CreatedBy, ModifiedBy
  - IsActive, Tags, Category
  - TemplateJSON (serialized structure)
  - Notes, Dependencies
- TemplateFile table for file definitions with content templates
- TemplateFolder table for unlimited hierarchical folder structures
- **ManifestTemplate field stores initial Manifest.md structure**
- All tables follow Forge naming conventions
- Metadata supports placeholders (e.g., `{{ProjectName}}`, `{{Description}}`)
- Manifest template can be customized per template
- **Maintains compatibility with existing TemplateBuilderControl templates**
- **Existing JSON templates can be imported without modification**
- **Enhanced with database persistence without breaking JSON format**
- **Note:** Manifest population/update handled by Spec 2 (Governance Automation Engine)

---

### User Story 3: Template CRUD Operations
**As a** developer  
**I want to** perform Create, Read, Update, Delete operations on templates  
**So that** I can manage template library programmatically

**Acceptance Criteria:**
- `CreateTemplate(template As TemplateDefinition) As Integer` - Returns new ID
- `GetTemplate(id As Integer) As TemplateDefinition` - Returns template or Nothing
- `GetAllTemplates() As List(Of TemplateDefinition)` - Returns all templates
- `GetTemplateByName(name As String) As TemplateDefinition` - Search by exact name
- `UpdateTemplate(template As TemplateDefinition) As Boolean` - Returns success
- `DeleteTemplate(id As Integer) As Boolean` - Returns success with cascade
- All operations use transactions
- All operations log to ForgeCharter-compliant logging
- Basic search functionality provided (expandable in future)

---

### User Story 4: JSON Template Serialization
**As a** developer  
**I want to** serialize/deserialize templates to/from JSON  
**So that** templates can be exported, imported, and shared with room for format expansion

**Acceptance Criteria:**
- `SerializeTemplate(template As TemplateDefinition) As String` - Returns JSON
- `DeserializeTemplate(json As String) As TemplateDefinition` - Returns template
- JSON structure validates against schema
- Handles unlimited nested folder/file structures
- Preserves all metadata (dates, version, tags, etc.)
- Preserves content templates with placeholders
- Architecture allows future format additions (XML, CSV) without breaking changes

---

## 3. Technical Requirements

### 3.1 Database Schema

#### Table: Template
```sql
CREATE TABLE Template (
    TemplateID AUTOINCREMENT PRIMARY KEY,
    Name TEXT(200) NOT NULL UNIQUE,
    Description MEMO,
    TemplateJSON MEMO NOT NULL,
    
    -- Manifest Template (Initial structure, populated by Spec 2 automation)
    ManifestTemplate MEMO,
    
    -- Version & Status
    Version TEXT(20) DEFAULT "1.0.0",
    IsActive YESNO DEFAULT True,
    
    -- Categorization
    Category TEXT(100),
    Tags TEXT(500),
    
    -- Audit Trail (Most Complete Metadata Header)
    CreatedDate DATETIME DEFAULT Now(),
    CreatedBy TEXT(100),
    ModifiedDate DATETIME DEFAULT Now(),
    ModifiedBy TEXT(100),
    
    -- Additional Metadata
    Dependencies TEXT(500),
    Notes MEMO,
    
    -- Usage Tracking
    LastUsedDate DATETIME,
    UsageCount LONG DEFAULT 0
);
```

**Rationale:** 
- Most comprehensive metadata header created in the Forge to date
- ManifestTemplate stores initial Manifest.md structure
- Actual manifest population/update will be handled by Spec 2 (Governance Automation Engine)
- Template provides foundation for both scaffolding (Spec 2) and automation (Spec 2)

#### Table: TemplateFolder
```sql
CREATE TABLE TemplateFolder (
    FolderID AUTOINCREMENT PRIMARY KEY,
    TemplateID LONG NOT NULL,
    ParentFolderID LONG,  -- NULL for root folders, supports unlimited nesting
    FolderName TEXT(255) NOT NULL,
    RelativePath TEXT(1000),
    OrderIndex INTEGER DEFAULT 0,
    
    -- Metadata
    Description TEXT(500),
    CreatedDate DATETIME DEFAULT Now(),
    
    FOREIGN KEY (TemplateID) REFERENCES Template(TemplateID) ON DELETE CASCADE,
    FOREIGN KEY (ParentFolderID) REFERENCES TemplateFolder(FolderID) ON DELETE CASCADE
);
```

**Rationale:** Self-referencing ParentFolderID enables unlimited folder hierarchy depth.

#### Table: TemplateFile
```sql
CREATE TABLE TemplateFile (
    FileID AUTOINCREMENT PRIMARY KEY,
    FolderID LONG NOT NULL,
    FileName TEXT(255) NOT NULL,
    FileType TEXT(50),
    
    -- Content Template with Placeholder Support
    ContentTemplate MEMO,  -- Supports {{ProjectName}}, {{Description}}, etc.
    
    -- Metadata (No CharacterCountTarget per requirements)
    RequiresMetadataHeader YESNO DEFAULT True,
    OrderIndex INTEGER DEFAULT 0,
    Description TEXT(500),
    CreatedDate DATETIME DEFAULT Now(),
    
    FOREIGN KEY (FolderID) REFERENCES TemplateFolder(FolderID) ON DELETE CASCADE
);
```

**Rationale:** ContentTemplate supports placeholder variables for dynamic file generation in future specs.

### 3.2 Data Models

**Note:** `TemplateDefinition` and `TemplateFolderDefinition` are enhanced versions of existing classes from TemplateBuilderControl (`DirectoryTemplate` and `FolderDefinition`). This maintains backward compatibility with existing templates while adding database persistence and comprehensive metadata.

#### TemplateDefinition Class
```vb
Public Class TemplateDefinition
    ' Core Identity
    Public Property TemplateID As Integer
    Public Property Name As String
    Public Property Description As String
    Public Property TemplateJSON As String
    
    ' Manifest Template (Initial Manifest.md structure)
    Public Property ManifestTemplate As String
    
    ' Version & Status
    Public Property Version As String
    Public Property IsActive As Boolean
    
    ' Categorization
    Public Property Category As String
    Public Property Tags As String
    
    ' Audit Trail (Most Complete Metadata Header)
    Public Property CreatedDate As DateTime
    Public Property CreatedBy As String
    Public Property ModifiedDate As DateTime
    Public Property ModifiedBy As String
    
    ' Additional Metadata
    Public Property Dependencies As String
    Public Property Notes As String
    
    ' Usage Tracking
    Public Property LastUsedDate As DateTime?
    Public Property UsageCount As Integer
    
    ' Hierarchical Structure
    Public Property Folders As List(Of TemplateFolderDefinition)
    
    Public Sub New()
        Folders = New List(Of TemplateFolderDefinition)()
        Version = "1.0.0"
        IsActive = True
        CreatedDate = DateTime.Now
        ModifiedDate = DateTime.Now
        UsageCount = 0
        ManifestTemplate = String.Empty
    End Sub
End Class
```

#### TemplateFolderDefinition Class
```vb
Public Class TemplateFolderDefinition
    Public Property FolderID As Integer
    Public Property FolderName As String
    Public Property RelativePath As String
    Public Property ParentFolderID As Integer?  -- Supports unlimited nesting
    Public Property OrderIndex As Integer
    Public Property Description As String
    Public Property CreatedDate As DateTime
    
    ' Hierarchical Structure (Unlimited Depth)
    Public Property Files As List(Of TemplateFileDefinition)
    Public Property Subfolders As List(Of TemplateFolderDefinition)
    
    Public Sub New()
        Files = New List(Of TemplateFileDefinition)()
        Subfolders = New List(Of TemplateFolderDefinition)()
        CreatedDate = DateTime.Now
    End Sub
End Class
```

#### TemplateFileDefinition Class
```vb
Public Class TemplateFileDefinition
    Public Property FileID As Integer
    Public Property FileName As String
    Public Property FileType As String
    Public Property ContentTemplate As String  -- Supports {{Placeholders}}
    Public Property RequiresMetadataHeader As Boolean
    Public Property OrderIndex As Integer
    Public Property Description As String
    Public Property CreatedDate As DateTime
    
    Public Sub New()
        RequiresMetadataHeader = True
        CreatedDate = DateTime.Now
    End Sub
End Class
```

### 3.3 API Surface

#### TemplateStorageService Interface
```vb
Public Interface ITemplateStorageService
    ' CRUD Operations
    Function CreateTemplate(template As TemplateDefinition) As Integer
    Function GetTemplate(id As Integer) As TemplateDefinition
    Function GetAllTemplates() As List(Of TemplateDefinition)
    Function UpdateTemplate(template As TemplateDefinition) As Boolean
    Function DeleteTemplate(id As Integer) As Boolean
    Function GetTemplateByName(name As String) As TemplateDefinition
    
    ' Search & Filter
    Function SearchTemplates(searchTerm As String) As List(Of TemplateDefinition)
    Function GetTemplatesByTag(tag As String) As List(Of TemplateDefinition)
    Function GetActiveTemplates() As List(Of TemplateDefinition)
    
    ' JSON Operations
    Function SerializeTemplate(template As TemplateDefinition) As String
    Function DeserializeTemplate(json As String) As TemplateDefinition
    Function ExportTemplate(id As Integer, filePath As String) As Boolean
    Function ImportTemplate(filePath As String) As Integer
    
    ' Database Management
    ReadOnly Property DatabasePath As String
    Function OpenDatabase(path As String) As Boolean
    Function CreateDatabase(path As String) As Boolean
    Function ValidateDatabaseSchema() As Boolean
End Interface
```

### 3.4 Dependencies

**Required NuGet Packages:**
- System.Data.OleDb (10.0.1+) - Access database connectivity
- Newtonsoft.Json (13.0.3+) - JSON serialization

**Required Project References:**
- RCHAutomation.Controls - For AccessSqlGeneratorControl

**Extracted from Existing Projects:**
- TemplateBuilderControl data models (DirectoryTemplate, FolderDefinition)
- AppStateManager state persistence pattern
- JSON serialization logic from TemplateBuilderControl

**Framework:**
- .NET 8.0 (Windows)
- VB.NET

---

## 4. Architecture

### 4.1 Project Structure
```
RCH.TemplateStorage/
??? Models/
?   ??? TemplateDefinition.vb (enhanced from TemplateBuilderControl)
?   ??? TemplateFolderDefinition.vb (enhanced from TemplateBuilderControl)
?   ??? TemplateFileDefinition.vb (new)
??? Services/
?   ??? Interfaces/
?   ?   ??? ITemplateStorageService.vb
?   ??? Implementations/
?       ??? TemplateStorageService.vb
?       ??? TemplateJsonSerializer.vb (extracted from TemplateBuilderControl)
??? DatabaseSchema/
?   ??? TemplateDatabase.sql - Schema definition script
??? Documentation/
?   ??? Chronicle/DevelopmentLog/
?   ??? README.md
??? RCH.TemplateStorage.vbproj
```

### 4.1.1 Component Extraction Strategy

**From TemplateBuilderControl (NewDatabaseGenerator):**
- Extract `DirectoryTemplate` class ? `Models/TemplateDefinition.vb` (enhanced with database fields)
- Extract `FolderDefinition` class ? `Models/TemplateFolderDefinition.vb` (add comprehensive metadata)
- Create new `Models/TemplateFileDefinition.vb` (new functionality for individual files)
- Extract JSON serialization logic ? `Services/Implementations/TemplateJsonSerializer.vb`

**From AppStateManager (NewDatabaseGenerator):**
- Reference directly for state management in UI projects (Spec 4)
- Extend `AppState` class with `TemplateStorageStateData` for user preferences

**Integration Benefits:**
- Reuse 40% of existing, production-tested code
- Maintain backward compatibility with existing templates
- Accelerate development timeline by ~30% (11-16 hours saved)
- Proven data structures from real-world usage

**Migration Note:** Existing TemplateBuilderControl JSON files can be imported directly into the database without modification. The enhanced models add database persistence while preserving the original JSON structure.

### 4.2 Component Interaction

```
???????????????????????????????????????????
?  Consumer Application                    ?
?  (RCH.TemplateGenerator.exe / UI)       ?
???????????????????????????????????????????
             ?
             ? Uses API
             ?
???????????????????????????????????????????
?  RCH.TemplateStorage (This Library)     ?
?  ?????????????????????????????????????  ?
?  ? ITemplateStorageService           ?  ?
?  ?  - CreateTemplate()               ?  ?
?  ?  - GetTemplate()                  ?  ?
?  ?  - SerializeTemplate()            ?  ?
?  ?????????????????????????????????????  ?
?             ?                            ?
?             ?                            ?
?  ?????????????????????????????????????  ?
?  ? TemplateStorageService            ?  ?
?  ?  - CRUD Logic                     ?  ?
?  ?  - Transaction Management         ?  ?
?  ?????????????????????????????????????  ?
?             ?                            ?
?             ?                            ?
?  ?????????????????????????????????????  ?
?  ? System.Data.OleDb                 ?  ?
?  ?  - Access Database Connectivity   ?  ?
?  ?????????????????????????????????????  ?
???????????????????????????????????????????
              ?
              ?
     ??????????????????????
     ? Template.accdb     ?
     ?  - Template table  ?
     ?  - Folder table    ?
     ?  - File table      ?
     ??????????????????????

???????????????????????????????????????????
?  Database Management UI                  ?
?  (AccessSqlGeneratorControl)            ?
?   - Direct SQL editing                   ?
?   - Schema inspection                    ?
?   - Query execution                      ?
???????????????????????????????????????????
              ?
              ? (Same database)
     ??????????????????????
     ? Template.accdb     ?
     ??????????????????????
```

---

## 5. Integration Points

### 5.1 AccessSqlGeneratorControl Integration

**Requirement:** Library must expose database path so AccessSqlGeneratorControl can connect to the same database for management.

**Implementation Pattern:**
```vb
' In consumer application (e.g., TemplateManagerForm)
Dim storageService As New TemplateStorageService("C:\Templates\Templates.accdb")
Dim dbPath As String = storageService.DatabasePath

' Pass to AccessSqlGeneratorControl
accessSqlControl.ConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}"
```

**Benefits:**
- User can modify database schema via SQL
- Can query templates directly
- Can bulk-insert test data
- Can inspect/troubleshoot database issues
- No need for separate database management tool

---

## 6. Constraints

### 6.1 ForgeCharter Compliance
- All files must have Forge metadata headers with character counts
- All naming must follow Forge conventions (no Helper, Manager, Utility)
- Documentation must follow Chronicle/Codex/Tomes taxonomy
- Version logs required for all changes

### 6.2 Performance
- Performance targets will be fine-tuned during implementation
- Baseline measurements will be documented in version logs
- Optimization opportunities will be identified through profiling
- Final performance characteristics documented in v060.md release notes
- Target: Responsive UI (<1s for typical operations)

### 6.3 Database Size
- Initial database < 5MB
- Support up to 10,000 templates
- Support up to 100,000 files/folders per template

---

## 7. Success Criteria

### 7.1 Functional
- ? Database created with correct schema
- ? All CRUD operations work correctly
- ? JSON serialization/deserialization accurate
- ? **Existing TemplateBuilderControl templates import successfully**
- ? **JSON format backward-compatible with production templates**
- ? AccessSqlGeneratorControl can connect and manage database
- ? **AppStateManager integration functional**
- ? All unit tests pass (target: 90%+ coverage)

### 7.2 Non-Functional
- ? 100% ForgeCharter compliance
- ? Build succeeds without warnings
- ? All files have accurate character counts
- ? Documentation complete (README + v010.md)
- ? Zero naming violations

---

## 8. Risks & Mitigations

### Risk 1: Access Database Engine Not Installed
**Impact:** HIGH  
**Probability:** MEDIUM  
**Mitigation:** 
- Detect ACE driver availability at startup
- Provide clear error message with download link
- Document installation requirement in README

### Risk 2: Concurrent Access Issues
**Impact:** MEDIUM  
**Probability:** LOW  
**Mitigation:**
- Use file-based locking
- Implement retry logic with exponential backoff
- Document single-user limitation

### Risk 3: JSON Schema Evolution
**Impact:** MEDIUM  
**Probability:** MEDIUM  
**Mitigation:**
- Version JSON schema
- Implement migration logic
- Maintain backward compatibility for v1.0

---

## 9. Future Enhancements (Not in Scope)

**Note:** The following features will be implemented in separate specs:

### Spec 2: Governance Automation Engine (Next Phase)
- **Directory Scanner:** Scan selected folder at any time
- **Manifest Generator:** Create/update Manifest.md in root directory with:
  - Complete file/folder tree
  - Index of all file headers
  - Character counts per file
- **Header Injector:** Add/update Forge metadata headers automatically
- **Smart File Type Filter:** 
  - Master whitelist of file types (.vb, .Designer.vb, .md, .txt, .sql, etc.)
  - User-configurable include/exclude per file type
  - All files documented in manifest, but only relevant data per type
  - Skip dangerous types (.json, .dll, .exe, binary, .resx, IDE files)
- **Character Counter:** Automatic computation, no more manual PowerShell!
- **Self-Maintaining Compliance:** Keep projects ForgeCharter-compliant automatically

### Other Future Enhancements
1. Multi-user support with SQL Server backend
2. Template versioning history (audit trail)
3. Template categories/hierarchies
4. Template sharing/export to cloud
5. Template preview/thumbnail generation

**Architectural Note:** Spec 1 (this spec) provides storage foundation. Spec 2 will consume this library to perform automation tasks.
