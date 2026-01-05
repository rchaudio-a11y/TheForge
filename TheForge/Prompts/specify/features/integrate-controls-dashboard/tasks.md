# Tasks: Integrate Controls for Dashboard Testing

**Document Type:** Task Breakdown  
**Purpose:** Actionable task list for RCHAutomation.Controls Forge compliance and TemplateBuilderControl extraction  
**Created:** 2026-01-04  
**Last Updated:** 2026-01-04  
**Status:** Active  
**Character Count:** 8082  
**Related:** spec.md, plan.md, constitution.md

---

## Phase 1: Setup (Version Logging System) - v0.1.0

- [ ] T001 [US1] Create `NewDatabaseGenerator/RCHAutomation.Controls/Documentation/` folder
- [ ] T002 [US1] Create `Documentation/Chronicle/` subfolder
- [ ] T003 [US1] Create `Documentation/Chronicle/DevelopmentLog/` subfolder
- [ ] T004 [US1] Copy `TheForge/Documentation/Chronicle/DevelopmentLog/v010.md` as template
- [ ] T005 [US1] Customize v010.md for RCHAutomation.Controls (Description, What It Does, etc.)
- [ ] T006 [US1] Add Forge metadata headers to v010.md
- [ ] T007 [US1] Compute character count for v010.md and update header
- [ ] T008 [US1] Document Phase 1 completion in v010.md

---

## Phase 2: Code Audit - AccessSqlGeneratorControl - v0.2.0

- [ ] T009 [US2] Read `AccessSqlGeneratorControl.vb` to understand structure
- [ ] T010 [US2] Add Forge metadata header to top of AccessSqlGeneratorControl.vb (before Imports)
- [ ] T011 [US2] Verify class naming: AccessSqlGeneratorControl (check for forbidden terms)
- [ ] T012 [US2] Compute character count for AccessSqlGeneratorControl.vb via PowerShell
- [ ] T013 [US2] Update Character Count field in metadata header
- [ ] T014 [US2] Build RCHAutomation.Controls to verify no breaks
- [ ] T015 [US2] Create v020.md documenting Phase 2 work
- [ ] T016 [US2] Add Forge metadata headers to v020.md
- [ ] T017 [US2] Compute character count for v020.md and update

---

## Phase 3: Code Audit - Supporting Files - v0.3.0

- [ ] T018 [US2] Enumerate all .vb files in RCHAutomation.Controls (excluding Designer files)
- [ ] T019 [US2] [P] Add Forge metadata header to File 1 (identify during enumeration)
- [ ] T020 [US2] [P] Add Forge metadata header to File 2
- [ ] T021 [US2] [P] Add Forge metadata header to File 3
- [ ] T022 [US2] [P] Add Forge metadata header to File 4
- [ ] T023 [US2] [P] Add Forge metadata header to File 5
- [ ] T024 [US2] [P] Add Forge metadata header to remaining .vb files (batch if many)
- [ ] T025 [US2] Compute character counts for all modified .vb files
- [ ] T026 [US2] Update Character Count fields in all .vb files
- [ ] T027 [US2] Enumerate all .Designer.vb files in RCHAutomation.Controls
- [ ] T028 [US2] [P] Add Forge metadata header to Designer File 1 (follow ForgeCharter Section 4)
- [ ] T029 [US2] [P] Add Forge metadata header to Designer File 2
- [ ] T030 [US2] [P] Add Forge metadata header to remaining Designer files
- [ ] T031 [US2] Compute character counts for all Designer files
- [ ] T032 [US2] Update Character Count fields in Designer files
- [ ] T033 [US2] Verify naming compliance across all files (check for Helper, Manager, Utility, etc.)
- [ ] T034 [US2] Document any naming violations found in notes
- [ ] T035 [US2] Verify namespace consistency (simple RootNamespace pattern)
- [ ] T036 [US2] Build RCHAutomation.Controls to verify no breaks
- [ ] T037 [US2] Create v030.md documenting Phase 3 work, patterns, violations
- [ ] T038 [US2] Add Forge metadata headers to v030.md
- [ ] T039 [US2] Compute character count for v030.md and update

---

## Phase 4: Documentation Audit - v0.4.0

- [ ] T040 [US3] Review README.md for Forge compliance (already done, verify only)
- [ ] T041 [US3] Check if additional documentation needed (API docs, usage guides)
- [ ] T042 [US3] Create additional docs if needed in correct taxonomy folders (Codex, Tomes, etc.)
- [ ] T043 [US3] Verify all cross-references in README.md are valid
- [ ] T044 [US3] Verify all character counts accurate in all documentation
- [ ] T045 [US3] Check Markdown formatting compliance (Branch-Documentation Section 6)
- [ ] T046 [US3] Run manual ForgeAudit checklist on all documentation
- [ ] T047 [US3] Create v040.md documenting Phase 4 audit results
- [ ] T048 [US3] Add Forge metadata headers to v040.md
- [ ] T049 [US3] Compute character count for v040.md and update

---

## Phase 5: Create TemplateBuilderControl Project - v0.5.0

- [ ] T050 [US4] Create `NewDatabaseGenerator/TemplateBuilder/` folder
- [ ] T051 [US4] Create `TemplateBuilder.vbproj` based on RCHAutomation.Controls.vbproj template
- [ ] T052 [US4] Configure .vbproj for .NET 8.0 (RootNamespace, AssemblyName, etc.)
- [ ] T053 [US4] Move `NewDatabaseGenerator/NewDatabaseGenerator/TemplateBuilderControl.vb` to `TemplateBuilder/`
- [ ] T054 [US4] Move `NewDatabaseGenerator/NewDatabaseGenerator/TemplateBuilderControl.resx` to `TemplateBuilder/`
- [ ] T055 [US4] Update file references in TemplateBuilder.vbproj
- [ ] T056 [US4] Create `TemplateBuilder/INSTRUCTIONS.txt` with manual .sln configuration steps
- [ ] T057 [US4] Update `NewDatabaseGenerator.sln` to include TemplateBuilder project (manual via INSTRUCTIONS.txt)
- [ ] T058 [US4] Build TemplateBuilder project independently to verify
- [ ] T059 [US4] Verify no build errors in TemplateBuilder
- [ ] T060 [US4] Create v050.md documenting Phase 5 extraction
- [ ] T061 [US4] Add Forge metadata headers to v050.md
- [ ] T062 [US4] Compute character count for v050.md and update

---

## Phase 6: NewDatabaseGenerator Integration - v0.6.0

- [ ] T063 [US4] Open `MainTabbedForm.vb` in NewDatabaseGenerator
- [ ] T064 [US4] Remove `directoryTemplate` field declaration from MainTabbedForm.vb
- [ ] T065 [US4] Remove TemplateBuilderControl instantiation from InitializeComponent()
- [ ] T066 [US4] Remove control from `directoryTemplateTab.Controls` collection
- [ ] T067 [US4] Verify `directoryTemplateTab` (Tab 2) remains intact but empty
- [ ] T068 [US4] Check `MainTabbedForm.Designer.vb` for TemplateBuilderControl references (remove if found)
- [ ] T069 [US4] Build NewDatabaseGenerator to verify no breaks
- [ ] T070 [US4] Build entire solution to verify all projects compile
- [ ] T071 [US4] Create folder structure for NewDatabaseGenerator v010.md (if doesn't exist)
- [ ] T072 [US4] Create `NewDatabaseGenerator/NewDatabaseGenerator/Documentation/Chronicle/DevelopmentLog/v010.md`
- [ ] T073 [US4] Document TemplateBuilderControl extraction in NewDatabaseGenerator v010.md
- [ ] T074 [US4] Add Forge metadata headers to NewDatabaseGenerator v010.md
- [ ] T075 [US4] Compute character count for NewDatabaseGenerator v010.md and update
- [ ] T076 [US4] Create `RCHAutomation.Controls/Documentation/Chronicle/DevelopmentLog/v060.md`
- [ ] T077 [US4] Document final integration and feature completion in v060.md
- [ ] T078 [US4] Add Forge metadata headers to v060.md
- [ ] T079 [US4] Compute character count for v060.md and update
- [ ] T080 [US4] Run manual ForgeAudit checklist on RCHAutomation.Controls
- [ ] T081 [US4] Verify ForgeAudit passes 100%
- [ ] T082 [US4] Mark feature complete (v0.6.0)

---

## Summary

**Total Tasks:** 82  
**Parallelizable Tasks:** 11 (marked with [P])  
**Phases:** 6  
**Version Progression:** v0.1.0 ? v0.2.0 ? v0.3.0 ? v0.4.0 ? v0.5.0 ? v0.6.0

**Estimated Time:** ~7 hours (9 hours with buffer)

---

## Task Execution Notes

### Parallelization Guide
Tasks marked [P] can be executed in parallel if desired, but sequential execution is safer and recommended for critical compliance work.

### Version Log Updates
After completing each phase, ensure version log (vXXX.md) is created and documents:
- Description of phase work
- Issues encountered
- Patterns learned
- Build status

### ForgeAudit Checkpoints
Run manual ForgeAudit checklist after:
- Phase 3 (T036) - Code audit complete
- Phase 4 (T046) - Documentation audit complete  
- Phase 6 (T080) - Final integration complete

### Build Verification Points
Build and verify after:
- T014 (Phase 2)
- T036 (Phase 3)
- T059 (Phase 5)
- T069, T070 (Phase 6)

---

**End of Tasks**
