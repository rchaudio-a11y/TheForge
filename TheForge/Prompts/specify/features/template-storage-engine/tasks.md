# Task Breakdown: Template Storage Engine

**Document Type:** Task List  
**Purpose:** Granular task breakdown for template storage implementation  
**Created:** 2026-01-05  
**Last Updated:** 2026-01-05  
**Status:** Ready for Execution  
**Character Count:** 23297  
**Related:** spec.md, plan.md, INTEGRATION_ANALYSIS.md, META-REFACTORING-REFLECTION.md

---

## Task Summary

**Total Tasks:** 70 (revised from 65)  
**Estimated Total Effort:** 30-40 hours (reduced from 40-60 hours)  
**Phases:** 6  
**Target Completion:** 1.5-2 weeks  
**Time Savings:** 11-16 hours (30%) due to component extraction

---

## Phase 1: Project Setup & Component Extraction (Tasks 1-15)
**Duration:** 3-4 hours (reduced from 2-3 hours due to extraction work)  
**Goal:** Create project structure and extract components from TemplateBuilderControl

### T001: Create RCH.TemplateStorage Project
- Create new Class Library project
- Target: .NET 8.0 (Windows)
- RootNamespace: RCH.TemplateStorage
- AssemblyName: RCH.TemplateStorage
- **Verify:** Project file created, solution compiles

### T002: Add NuGet Dependencies
- Add System.Data.OleDb (10.0.1)
- Add Newtonsoft.Json (13.0.3)
- **Verify:** Packages restore successfully

### T003: Add Project Reference to RCHAutomation.Controls
- Add ProjectReference to ..\NewDatabaseGenerator\RCHAutomation.Controls\RCHAutomation.Controls.vbproj
- **Verify:** Reference resolves, no build errors

### T004: Create Folder Structure
- Create Models/ folder
- Create Services/Interfaces/ folder
- Create Services/Implementations/ folder
- Create DatabaseSchema/ folder
- Create Documentation/ folder
- Create Documentation/Chronicle/ folder
- Create Documentation/Chronicle/DevelopmentLog/ folder
- **Verify:** All folders exist

### T005: **Extract DirectoryTemplate ? TemplateDefinition**
- Copy DirectoryTemplate class from TemplateBuilderControl.vb
- Rename to TemplateDefinition
- Move to Models/TemplateDefinition.vb
- Add Forge metadata header
- Add database-specific properties (TemplateID, CreatedBy, ModifiedBy, etc.)
- Preserve original properties for backward compatibility
- **Verify:** Class compiles, original structure intact

### T006: **Extract FolderDefinition ? TemplateFolderDefinition**
- Copy FolderDefinition class from TemplateBuilderControl.vb
- Rename to TemplateFolderDefinition
- Move to Models/TemplateFolderDefinition.vb
- Add Forge metadata header
- Add metadata properties (Description, CreatedDate)
- Preserve SubFolders list for unlimited nesting
- **Verify:** Class compiles, hierarchy support intact

### T007: **Create TemplateFileDefinition (New)**
- Create Models/TemplateFileDefinition.vb
- Define properties per spec (FileID, FileName, FileType, ContentTemplate, etc.)
- Add Forge metadata header
- **Verify:** Model compiles

### T008: **Extract JSON Serialization Logic**
- Copy JSON serialization code from TemplateBuilderControl
- Create Services/Implementations/TemplateJsonSerializer.vb
- Add Forge metadata header
- Enhance with schema validation
- Add ImportLegacyTemplate() method for migration
- **Verify:** Serializer compiles

### T009: Create README.md
- Add Forge metadata header
- Add project overview
- Add installation instructions
- Add usage examples (placeholder)
- Document extraction strategy
- Add API reference (placeholder)
- **Verify:** Character count computed and accurate

### T010: Create v010.md
- Add Forge metadata header
- Document Phase 1 setup and extraction
- Document component sources (TemplateBuilderControl)
- Document extraction decisions
- **Verify:** Character count computed and accurate

### T011: **Test Extracted Models**
- Create simple test instantiating TemplateDefinition
- Verify Folders list works
- Verify hierarchical structure (unlimited nesting)
- **Verify:** Models functional

### T012: **Verify Backward Compatibility**
- Load sample TemplateBuilderControl JSON
- Deserialize with new TemplateJsonSerializer
- Verify all fields map correctly
- **Verify:** Legacy JSON imports successfully

### T013: Create .gitignore (if needed)
- Add bin/
- Add obj/
- Add *.user files
- **Verify:** Git ignores build artifacts

### T014: Initial Build Verification
- Build RCH.TemplateStorage project
- Verify no errors
- Verify no warnings
- **Verify:** Build successful

### T015: Phase 1 Documentation Update
- Update v010.md with Phase 1 completion
- Document extraction results
- Update character counts
- **Verify:** Documentation accurate

---

## Phase 2: Database Layer (Tasks 16-25)
**Duration:** 4-5 hours (slightly increased for compatibility testing)  
**Goal:** Design and implement database schema compatible with extracted models

### T016: Design Database Schema
- Create TemplateDatabase.sql file
- Define Template table with comprehensive metadata
- Define TemplateFolder table with self-referencing ParentFolderID
- Define TemplateFile table
- Add indexes
- **Verify schema supports extracted model structure**
- Add Forge metadata header
- **Verify:** SQL syntax valid

### T017: **Validate Schema Against Extracted Models**
- Review Template table fields vs TemplateDefinition properties
- Ensure all extracted properties have corresponding columns
- Verify ManifestTemplate field included
- **Verify:** Schema matches models

### T018: Create Database Initialization Script
- Create DatabaseInitializer.vb
- Implement CreateDatabase() method
- Implement ValidateDatabaseSchema() method
- Add Forge metadata header
- **Verify:** Database creates successfully

### T019: Implement Connection String Builder
- Create ConnectionStringBuilder.vb
- Implement GetConnectionString() method
- Add validation for database path
- Add Forge metadata header
- **Verify:** Connection string valid

### T020: Create Database Connection Wrapper
- Create DatabaseConnection.vb
- Implement IDisposable pattern
- Add connection pooling logic (if applicable)
- Add Forge metadata header
- **Verify:** Connections open/close correctly

### T021: Implement Database Validation
- Create DatabaseValidator.vb
- Implement ValidateSchema() method
- Implement CheckTableExists() methods
- Add Forge metadata header
- **Verify:** Validation logic correct

### T022: Create Test Database
- Generate Templates_Test.accdb
- Execute schema script
- Verify tables created
- Add sample data (3 templates)
- **Verify:** Database structure correct

### T023: **Import Legacy TemplateBuilderControl Template**
- Manually create a test template using TemplateBuilderControl JSON format
- Attempt to deserialize with extracted models
- Verify hierarchical folder structure imports
- **Verify:** Legacy templates compatible

### T024: Add Database Error Handling
- Create TemplateDatabaseException.vb
- Implement exception hierarchy
- Add detailed error messages
- Add Forge metadata header
- **Verify:** Exceptions thrown correctly

### T025: Implement Database Backup/Restore
- Create DatabaseBackup.vb
- Implement BackupDatabase() method
- Implement RestoreDatabase() method
- Add Forge metadata header
- **Verify:** Backup/restore functional

### T026: Phase 2 Build Verification
- Build project
- Verify no errors
- Run database initialization
- **Verify:** Build successful, database creates

### T027: Phase 2 Documentation Update
- Create v020.md
- Document database design decisions
- Document schema rationale
- Update character counts
- **Verify:** Documentation complete

---

## Phase 3: Data Models Enhancement (Tasks 26-35)
**Duration:** 3-4 hours (reduced from 4-5 hours due to extraction)  
**Goal:** Enhance extracted models with database persistence and comprehensive metadata

### T026: **Enhance TemplateDefinition with Database Fields**
- Add TemplateID property (if not present)
- Ensure CreatedBy, ModifiedBy properties exist
- Add Category, Tags properties
- Add Dependencies, Notes properties
- Add UsageCount, LastUsedDate properties
- Preserve original Folders list
- **Verify:** Backward compatible with JSON

### T027: **Enhance TemplateFolderDefinition with Metadata**
- Ensure FolderID property exists
- Add Description property
- Add CreatedDate property
- Verify ParentFolderID supports unlimited nesting
- Preserve Files and Subfolders lists
- **Verify:** Hierarchy functional

### T028: **Implement TemplateFileDefinition Fully**
- Add all properties per spec
- Implement RequiresMetadataHeader flag
- Add ContentTemplate with placeholder support
- Add validation logic
- **Verify:** Model complete

### T029: Add Model Validation Logic
- Create ModelValidator.vb
- Implement ValidateTemplate() method
- Implement ValidateFolder() method
- Implement ValidateFile() method
- **Validate extracted structure integrity**
- Add Forge metadata header
- **Verify:** Validation catches invalid models

### T030: Implement Model Equality Comparers
- Add Equals() overrides to all three models
- Add GetHashCode() overrides
- Implement IEquatable<T>
- **Verify:** Equality logic correct

### T031: Add Model ToString() Overrides
- Override ToString() for debugging in all models
- Format output for readability
- **Verify:** ToString() output useful

### T032: **Create Backward Compatibility Tests**
- Test TemplateDefinition vs DirectoryTemplate
- Verify JSON round-trip (old format ? new format ? old format)
- Test folder hierarchy preservation
- **Verify:** Zero data loss in migration

### T033: Create Model Unit Tests
- Create TemplateDefinitionTests.vb
- Test property setters/getters
- Test validation logic
- Test equality logic
- **Test extracted model behavior**
- **Verify:** All tests pass

### T034: Phase 3 Build Verification
- Build project
- Run model tests
- Verify no errors
- **Verify:** Build successful, tests pass

### T035: Phase 3 Documentation Update
- Create v030.md
- Document model enhancements
- Document validation rules
- **Document extraction enhancements**
- Update character counts
- **Verify:** Documentation complete

---

## Phase 4: Service Layer - CRUD Operations (Tasks 36-50)
**Duration:** 8-10 hours  
**Goal:** Implement all CRUD operations with transaction support

### T038: Create ITemplateStorageService Interface
- Create Services/Interfaces/ITemplateStorageService.vb
- Define all CRUD methods
- Define search methods
- Define JSON methods
- Add Forge metadata header
- **Verify:** Interface compiles

### T039: Create TemplateStorageService Class
- Create Services/Implementations/TemplateStorageService.vb
- Implement ITemplateStorageService
- Add constructor with database path
- Add connection management
- Add Forge metadata header
- **Verify:** Class compiles

### T040: Implement CreateTemplate Method
- Implement CreateTemplate() logic
- Add transaction support
- Add error handling
- Add logging
- **Verify:** Template creation works

### T041: Implement GetTemplate Method
- Implement GetTemplate() logic
- Load template with folders/files
- Handle not found case
- **Verify:** Template retrieval works

### T042: Implement GetAllTemplates Method
- Implement GetAllTemplates() logic
- Add pagination support (optional)
- Optimize query performance
- **Verify:** Bulk retrieval works

### T043: Implement UpdateTemplate Method
- Implement UpdateTemplate() logic
- Add transaction support
- Handle modified date update
- **Verify:** Template updates work

### T044: Implement DeleteTemplate Method
- Implement DeleteTemplate() logic
- Add cascade delete for folders/files
- Add transaction support
- **Verify:** Template deletion works

### T045: Implement GetTemplateByName Method
- Implement GetTemplateByName() logic
- Add case-insensitive search
- **Verify:** Name search works

### T046: Implement SearchTemplates Method
- Implement SearchTemplates() logic
- Search across name/description/tags
- Add wildcard support
- **Verify:** Search works correctly

### T047: Implement GetTemplatesByTag Method
- Implement GetTemplatesByTag() logic
- Handle comma-separated tags
- **Verify:** Tag filtering works

### T048: Implement GetActiveTemplates Method
- Implement GetActiveTemplates() logic
- Filter by IsActive flag
- **Verify:** Active filtering works

### T049: Add Transaction Logging
- Log all write operations
- Log transaction commits/rollbacks
- **Verify:** Logging works

### T050: Implement Retry Logic
- Add retry for transient errors
- Implement exponential backoff
- **Verify:** Retry logic works

### T051: Phase 4 Build Verification
- Build project
- Test all CRUD operations
- Verify transactions work
- **Verify:** Build successful, CRUD functional

### T052: Phase 4 Documentation Update
- Create v040.md
- Document CRUD implementation
- Document transaction strategy
- Update character counts
- **Verify:** Documentation complete

---

## Phase 5: JSON Serialization & Migration (Tasks 51-60)
**Duration:** 5-6 hours (increased from 6-8 hours - extraction helps but migration adds work)  
**Goal:** Implement JSON serialization with backward compatibility and legacy migration

### T051: **Enhance Extracted TemplateJsonSerializer**
- Review extracted serialization code
- Add Newtonsoft.Json configuration
- Configure for backward compatibility
- Add Forge metadata header
- **Verify:** Class compiles

### T052: **Implement SerializeTemplate Method**
- Implement SerializeTemplate() logic
- Configure JSON formatting (match TemplateBuilderControl format)
- Handle null values (backward compatible)
- Preserve folder/file hierarchy structure
- **Verify:** Serialization produces valid JSON

### T053: **Implement DeserializeTemplate Method**
- Implement DeserializeTemplate() logic
- Handle both old (DirectoryTemplate) and new (TemplateDefinition) formats
- Add validation after deserialization
- Handle malformed JSON
- Auto-migrate old format fields
- **Verify:** Deserialization handles both formats

### T054: **Implement ImportLegacyTemplate Method**
- Create ImportLegacyTemplate() specifically for TemplateBuilderControl JSON
- Parse as DirectoryTemplate format
- Convert to TemplateDefinition format
- Preserve all folder/file structure
- Add default values for new fields
- **Verify:** Legacy templates import perfectly

### T055: Implement ExportTemplate Method
- Implement ExportTemplate() logic
- Save JSON to file
- Handle file I/O errors
- **Verify:** Export works

### T056: Implement ImportTemplate Method
- Implement ImportTemplate() logic
- Load JSON from file
- Detect format (legacy vs new)
- Route to appropriate deserializer
- Validate before inserting
- **Verify:** Import works for both formats

### T057: Add JSON Schema Validation
- Create JSON schema file for new format
- Implement schema validation
- Handle schema violations gracefully
- **Verify:** Schema validation works

### T058: **Test Migration with Real TemplateBuilderControl Files**
- Locate existing TemplateBuilderControl .json templates
- Import each one
- Verify folder structure intact
- Verify no data loss
- Export and compare
- **Verify:** 100% successful migration

### T059: Handle Edge Cases
- Test null/empty templates
- Test deeply nested folders (unlimited depth)
- Test very large templates
- Test malformed JSON recovery
- **Verify:** Edge cases handled

### T060: Create JSON Unit Tests
- Test serialization round-trip
- **Test legacy format import**
- **Test format conversion accuracy**
- Test export/import workflow
- Test error handling
- **Verify:** All tests pass

### T061: Phase 5 Build Verification
- Build project
- Run JSON tests
- Verify no data loss
- **Verify:** Build successful, JSON functional

### T062: Phase 5 Documentation Update
- Create v050.md
- Document JSON format
- Document export/import workflow
- Update character counts
- **Verify:** Documentation complete

---

## Phase 6: Testing, Documentation & Integration (Tasks 61-70)
**Duration:** 8-10 hours  
**Goal:** Complete testing, finalize documentation, verify Forge compliance and backward compatibility

### T061: **Create Backward Compatibility Integration Tests**
- Create BackwardCompatibilityTests.vb
- Test full cycle: Legacy JSON ? Database ? New JSON ? Verify
- Test with at least 5 different TemplateBuilderControl templates
- Measure success rate (target: 100%)
- **Verify:** All legacy templates migrate successfully

### T062: Create Integration Tests
- Create IntegrationTests.vb
- Test database round-trip
- Test AccessSqlGeneratorControl connection
- Test concurrent operations
- **Verify:** Integration tests pass

### T063: Create Performance Tests
- Create PerformanceTests.vb
- Test GetTemplate() response time
- Test GetAllTemplates() response time
- Test CreateTemplate() response time
- Test JSON import performance
- Document baseline measurements
- **Verify:** Performance acceptable

### T064: **Document Extraction Strategy**
- Update README.md with extraction details
- Document what was extracted from TemplateBuilderControl
- Document enhancements made
- Add migration guide for users
- Update character counts
- **Verify:** README complete

### T065: **Create Migration Documentation**
- Create MIGRATION.md in Documentation/
- Step-by-step guide for migrating TemplateBuilderControl templates
- Code examples for ImportLegacyTemplate()
- Troubleshooting section
- **Verify:** Migration guide clear

### T066: Complete API Documentation
- Update README.md with full API
- Add code examples for all methods
- Add troubleshooting section
- Document backward compatibility features
- Update character counts
- **Verify:** README complete

### T067: Create Database Schema Documentation
- Document each table
- Document relationships
- Document indexes
- Document how schema supports extracted models
- Add to README or separate doc
- **Verify:** Schema documented

### T068: ForgeCharter Compliance Audit
- Verify all files have metadata headers
- Verify all character counts accurate
- Verify naming conventions followed
- **Verify extraction-related files compliant**
- Run ForgeAudit (if available)
- **Verify:** 100% compliant

### T069: Build Verification (Final)
- Clean solution
- Rebuild all
- Run all tests (unit + integration + backward compatibility)
- Verify no warnings
- Verify no errors
- **Verify:** Build successful, all tests pass

### T070: Create v060.md (Final)
- Document Phase 6 completion
- Document test results
- Document compliance verification
- **Document extraction success metrics (40% reuse, 30% time savings)**
- **Document backward compatibility validation results**
- Update character counts
- **Verify:** Version log complete

---

## Task Dependencies

```
Phase 1 (Setup & Extraction) ? Must complete before Phase 2
  ?? T005-T008 (Extraction) are critical path
  ?? T012 (Backward compatibility check) validates extraction

Phase 2 (Database) ? Must complete before Phase 3
  ?? T017 (Schema validation) depends on T005-T007 (models)
  ?? T023 (Legacy import test) validates extraction

Phase 3 (Models) ? Must complete before Phase 4
  ?? T032 (Backward compatibility tests) critical validation

Phase 4 (CRUD) ? Must complete before Phase 5
Phase 5 (JSON & Migration) ? Can overlap with Phase 4
  ?? T053-T054 (Deserialize/Import) depend on extracted serializer (T008)
  ?? T058 (Real file testing) critical validation milestone

Phase 6 (Testing) ? Requires Phases 4 & 5 complete
  ?? T061 (Backward compatibility integration) final validation
```

---

## Task Tracking

| Phase | Tasks | Status | Completed | Remaining | Notes |
|-------|-------|--------|-----------|-----------|-------|
| 1 | 1-15 | Pending | 0/15 | 15 | Includes 5 extraction tasks |
| 2 | 16-25 | Pending | 0/10 | 10 | Includes compatibility validation |
| 3 | 26-35 | Pending | 0/10 | 10 | Model enhancement phase |
| 4 | 36-50 | Pending | 0/15 | 15 | CRUD operations |
| 5 | 51-60 | Pending | 0/10 | 10 | Migration-heavy phase |
| 6 | 61-70 | Pending | 0/10 | 10 | Testing & documentation |
| **Total** | **1-70** | **Pending** | **0/70** | **70** | **30-40 hours** |

---

## Extraction-Specific Tasks Summary

**Total Extraction Tasks:** 12 tasks across all phases

| Task ID | Description | Phase | Est. Hours |
|---------|-------------|-------|------------|
| T005 | Extract DirectoryTemplate ? TemplateDefinition | 1 | 1.0 |
| T006 | Extract FolderDefinition ? TemplateFolderDefinition | 1 | 0.5 |
| T008 | Extract JSON Serialization Logic | 1 | 1.0 |
| T012 | Verify Backward Compatibility (initial) | 1 | 0.5 |
| T017 | Validate Schema Against Extracted Models | 2 | 0.5 |
| T023 | Import Legacy TemplateBuilderControl Template | 2 | 0.5 |
| T026-T028 | Enhance Extracted Models | 3 | 2.0 |
| T032 | Create Backward Compatibility Tests | 3 | 1.0 |
| T051-T054 | Enhance Extracted Serializer + Migration | 5 | 3.0 |
| T058 | Test Migration with Real Files | 5 | 1.0 |
| T061 | Backward Compatibility Integration Tests | 6 | 2.0 |
| T064-T065 | Document Extraction & Migration | 6 | 2.0 |

**Subtotal Extraction Work:** ~15 hours  
**Extraction Savings:** ~11-16 hours (would have been 26-31 hours to build from scratch)  
**Net Benefit:** ~30% time reduction

---

## Completion Criteria

### Phase-Level
- ? All phase tasks complete
- ? Phase build successful
- ? Phase documentation complete (vXXX.md)
- ? Character counts updated
- ? **Backward compatibility verified (Phases 1, 2, 3, 5, 6)**

### Feature-Level
- ? All 70 tasks complete
- ? All unit tests pass (90%+ coverage)
- ? All integration tests pass
- ? **All backward compatibility tests pass (100% success rate)**
- ? **Legacy TemplateBuilderControl templates migrate successfully**
- ? Performance targets met (documented)
- ? ForgeCharter 100% compliant
- ? README complete
- ? Release notes complete
- ? **Extraction metrics documented (40% reuse, 30% savings)**

---

## Actionable Items Summary

### Critical Path Tasks (Must complete in order)
1. T005-T008: Component extraction (Phase 1)
2. T012: Initial backward compatibility check
3. T017: Schema validation against models
4. T032: Model backward compatibility tests
5. T053-T054: Deserializer with legacy support
6. T058: Real TemplateBuilderControl file testing
7. T061: Final backward compatibility integration tests

### Validation Milestones
- **Milestone 1:** Phase 1 complete, extracted models compile ?
- **Milestone 2:** Phase 2 complete, legacy template imports ?
- **Milestone 3:** Phase 3 complete, backward compatibility tests pass ?
- **Milestone 4:** Phase 5 complete, real file migration succeeds ?
- **Milestone 5:** Phase 6 complete, 100% backward compatibility ?

---

**Task List Status:** Draft - Updated with Extraction Strategy  
**Ready for Execution:** YES  
**Start Date:** TBD  
**Target Completion:** 1.5-2 weeks from start  
**Time Savings:** 11-16 hours (30%) due to component extraction

---

**End of Task Breakdown**
