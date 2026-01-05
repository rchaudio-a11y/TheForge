# Implementation Plan: Integrate Controls for Dashboard Testing

**Document Type:** Implementation Plan  
**Purpose:** Technical design for RCHAutomation.Controls Forge compliance and TemplateBuilderControl extraction  
**Created:** 2026-01-04  
**Last Updated:** 2026-01-04  
**Status:** Active  
**Character Count:** 12849  
**Related:** spec.md, constitution.md, ForgeCharter.md

---

## Tech Stack

### Core Technologies
- **Language:** VB.NET
- **Framework:** .NET 8.0 (Windows)
- **IDE:** Visual Studio 2022
- **Version Control:** Git
- **Documentation:** Markdown (.md)

### Tools & Utilities
- **PowerShell** - Terminal operations, folder creation
- **edit_file tool** - AI edits .Designer.vb files directly (per ForgeCharter Section 4)
- **Text file instructions** - AI provides instructions for user to manually edit .vb files with Designer associations
- **ForgeAudit** - Compliance verification (manual, no automation)
- **Character Count** - Manual calculation via PowerShell `Get-Content | .Length`

### Governance Tools
- **ForgeCharter.md** - Universal governance rules
- **Branch-Coding.md** - VB.NET coding standards
- **Branch-Documentation.md** - Documentation taxonomy
- **Branch-Architecture.md** - Project structure rules

---

## File Structure

### Phase 1: Setup (v0.1.0)
**Create:**
- `NewDatabaseGenerator/RCHAutomation.Controls/Documentation/`
- `NewDatabaseGenerator/RCHAutomation.Controls/Documentation/Chronicle/`
- `NewDatabaseGenerator/RCHAutomation.Controls/Documentation/Chronicle/DevelopmentLog/`
- `NewDatabaseGenerator/RCHAutomation.Controls/Documentation/Chronicle/DevelopmentLog/v010.md`

**Template Source:** `TheForge/Documentation/Chronicle/DevelopmentLog/v010.md`

**Note:** Template storage location TBD - consider creating `TheForge/Prompts/specify/templates/` for reusable templates

---

### Phase 2: Code Audit - AccessSqlGeneratorControl (v0.2.0)
**Modify:**
- `NewDatabaseGenerator/RCHAutomation.Controls/AccessSqlGeneratorControl.vb` - Add Forge metadata header

**Create:**
- `NewDatabaseGenerator/RCHAutomation.Controls/Documentation/Chronicle/DevelopmentLog/v020.md`

**Header Template:**
```vb
' Document Type: UserControl Implementation
' Purpose: SQL editor and database management control
' Created: YYYY-MM-DD
' Last Updated: YYYY-MM-DD
' Status: Active
' Character Count: TBD
' Related: RCHAutomation.Controls.vbproj
```

---

### Phase 3: Code Audit - Supporting Files (v0.3.0)
**Identify and Modify All .vb Files:**
- Enumerate all `.vb` files in `NewDatabaseGenerator/RCHAutomation.Controls/`
- Add Forge metadata headers to each
- Verify naming (no Helper, Manager, Utility)
- Compute character counts

**Create:**
- `NewDatabaseGenerator/RCHAutomation.Controls/Documentation/Chronicle/DevelopmentLog/v030.md`

**Note:** Designer.vb files need headers following ForgeCharter Section 4 rules (edit Designer, not main .vb)

---

### Phase 4: Documentation Audit (v0.4.0)
**Verify:**
- `NewDatabaseGenerator/RCHAutomation.Controls/README.md` (already Forge-compliant)

**Create (if needed):**
- `NewDatabaseGenerator/RCHAutomation.Controls/Documentation/Codex/` - API documentation
- `NewDatabaseGenerator/RCHAutomation.Controls/Documentation/Tomes/` - User guides

**Create:**
- `NewDatabaseGenerator/RCHAutomation.Controls/Documentation/Chronicle/DevelopmentLog/v040.md`

---

### Phase 5: Create TemplateBuilderControl Project (v0.5.0)
**Create:**
- `NewDatabaseGenerator/TemplateBuilder/` - New project folder (same root as other test modules)
- `NewDatabaseGenerator/TemplateBuilder/TemplateBuilder.vbproj` - New project file
- `NewDatabaseGenerator/TemplateBuilder/INSTRUCTIONS.txt` - Manual .vbproj configuration guide for user

**Move:**
- `NewDatabaseGenerator/NewDatabaseGenerator/TemplateBuilderControl.vb` ? `NewDatabaseGenerator/TemplateBuilder/TemplateBuilderControl.vb`
- `NewDatabaseGenerator/NewDatabaseGenerator/TemplateBuilderControl.resx` ? `NewDatabaseGenerator/TemplateBuilder/TemplateBuilderControl.resx`

**Modify:**
- `NewDatabaseGenerator/NewDatabaseGenerator.sln` - Add new project reference

**Create:**
- `NewDatabaseGenerator/RCHAutomation.Controls/Documentation/Chronicle/DevelopmentLog/v050.md`

---

### Phase 6: NewDatabaseGenerator Integration (v0.6.0)
**Modify:**
- `NewDatabaseGenerator/NewDatabaseGenerator/MainTabbedForm.vb` - Remove TemplateBuilderControl from Tab 2
- `NewDatabaseGenerator/NewDatabaseGenerator/MainTabbedForm.Designer.vb` - Remove control instantiation (if present)

**Create:**
- `NewDatabaseGenerator/NewDatabaseGenerator/Documentation/` (if doesn't exist)
- `NewDatabaseGenerator/NewDatabaseGenerator/Documentation/Chronicle/DevelopmentLog/`
- `NewDatabaseGenerator/NewDatabaseGenerator/Documentation/Chronicle/DevelopmentLog/v010.md`
- `NewDatabaseGenerator/RCHAutomation.Controls/Documentation/Chronicle/DevelopmentLog/v060.md`

**Verify:**
- NewDatabaseGenerator builds successfully
- RCHAutomation.Controls builds successfully
- TemplateBuilder builds independently

---

## Dependencies

### Phase Dependencies
```
Phase 1 (Setup)
    ?
Phase 2 (Main Control) ? Depends on Phase 1 (needs v010.md template)
    ?
Phase 3 (Supporting Files) ? Depends on Phase 2 (sequential audit)
    ?
Phase 4 (Documentation) ? Depends on Phase 3 (needs code complete)
    ?
Phase 5 (Create Project) ? Independent, can start after Phase 1
    ?
Phase 6 (Integration) ? Depends on Phase 5 (needs new project)
```

**Parallelization:** None recommended - sequential execution ensures clean audits and documentation

### External Dependencies
- **NuGet Packages:** Already on user's PC for NewDatabaseGenerator
- **TemplateBuilder:** Unknown NuGet dependencies (will discover during Phase 5)
- **Microsoft Access Database Engine:** Required for AccessSqlGeneratorControl (already installed)

### Governance Dependencies
- ForgeCharter.md (v1.1)
- Branch-Coding.md
- Branch-Documentation.md
- Branch-Architecture.md
- Constitution.md (v1.0.0)

---

## Implementation Strategy

### Phase 1: Setup (Estimated: 30 minutes)
**Steps:**
1. Create folder structure in RCHAutomation.Controls
2. Copy TheForge v010.md as template
3. Initialize v010.md with Phase 1 content
4. Add Forge metadata headers to v010.md
5. Document Phase 1 completion

**Risks:** None - folder creation is low-risk

---

### Phase 2: Code Audit - AccessSqlGeneratorControl (Estimated: 1 hour)
**Steps:**
1. Read AccessSqlGeneratorControl.vb
2. Identify top of file (before Imports or Namespace)
3. Add Forge metadata header
4. Verify class naming (AccessSqlGeneratorControl = compliant)
5. Compute character count via PowerShell
6. Update Character Count in header
7. Create v020.md documenting work
8. Build RCHAutomation.Controls to verify no breaks

**Risks:** 
- File locking if Designer opens (mitigation: edit .vb only, not Designer)
- Character count calculation error (mitigation: verify with multiple methods)

---

### Phase 3: Code Audit - Supporting Files (Estimated: 2 hours)
**Steps:**
1. Enumerate all .vb files in RCHAutomation.Controls (exclude Designer files for now)
2. For each file:
   - Add Forge metadata header
   - Verify naming compliance
   - Compute character count
   - Document violations (if any)
3. Audit .Designer.vb files separately (following ForgeCharter Section 4)
4. Verify namespace consistency (simple RootNamespace pattern)
5. Create v030.md documenting patterns/violations
6. Build RCHAutomation.Controls to verify

**Risks:**
- Many files = time-consuming (mitigation: batch similar files)
- Naming violations require refactoring (mitigation: document only, fix in future phase)
- Designer file editing complexity (mitigation: follow ForgeCharter Section 4 strictly)

---

### Phase 4: Documentation Audit (Estimated: 1 hour)
**Steps:**
1. Verify README.md compliance (already done)
2. Check for missing documentation (API docs, usage guides)
3. Create additional docs if needed in correct taxonomy folders
4. Validate all cross-references in README
5. Verify all character counts accurate
6. Create v040.md documenting audit
7. Run manual ForgeAudit checklist

**Risks:**
- Missing documentation requires creation (mitigation: minimal scope, defer to future)
- Taxonomy confusion (mitigation: reference Branch-Documentation.md)

---

### Phase 5: Create TemplateBuilderControl Project (Estimated: 1.5 hours)
**Steps:**
1. Create `NewDatabaseGenerator/TemplateBuilder/` folder
2. Create `TemplateBuilder.vbproj` based on RCHAutomation.Controls.vbproj template
3. Move TemplateBuilderControl.vb from NewDatabaseGenerator project
4. Move TemplateBuilderControl.resx from NewDatabaseGenerator project
5. Create INSTRUCTIONS.txt with manual .vbproj configuration steps for user
6. Update NewDatabaseGenerator.sln to include new project
7. Verify TemplateBuilder builds independently
8. Create v050.md documenting extraction

**Risks:**
- .vbproj configuration errors (mitigation: provide detailed INSTRUCTIONS.txt)
- Missing dependencies not discovered until build (mitigation: test build immediately)
- Solution file corruption (mitigation: backup .sln before editing)

**INSTRUCTIONS.txt Template:**
```
1. Open NewDatabaseGenerator.sln in text editor
2. Add this block before </Project>:
   <ProjectReference Include="TemplateBuilder\TemplateBuilder.vbproj" />
3. Save and reopen in Visual Studio
4. Verify TemplateBuilder project appears in Solution Explorer
5. Build solution to verify all projects compile
```

---

### Phase 6: NewDatabaseGenerator Integration (Estimated: 1 hour)
**Steps:**
1. Open MainTabbedForm.vb
2. Remove TemplateBuilderControl from Tab 2:
   - Remove `directoryTemplate` field declaration
   - Remove control instantiation in `InitializeComponent()`
   - Remove control from tab's Controls collection
3. Leave `directoryTemplateTab` intact (empty placeholder)
4. Create NewDatabaseGenerator v010.md documenting changes
5. Build NewDatabaseGenerator to verify
6. Build entire solution to verify
7. Create RCHAutomation.Controls v060.md documenting completion

**Risks:**
- Breaking NewDatabaseGenerator build (mitigation: careful removal, test immediately)
- Tab removal instead of emptying (mitigation: verify tab preserved)
- Designer file issues (mitigation: follow ForgeCharter Section 4)

---

## Timeline Estimate

| Phase | Description | Estimated Time |
|-------|-------------|----------------|
| Phase 1 | Setup version logging | 30 minutes |
| Phase 2 | Audit AccessSqlGeneratorControl | 1 hour |
| Phase 3 | Audit supporting files | 2 hours |
| Phase 4 | Documentation audit | 1 hour |
| Phase 5 | Create TemplateBuilder project | 1.5 hours |
| Phase 6 | NewDatabaseGenerator integration | 1 hour |
| **Total** | **Complete feature** | **~7 hours** |

**Contingency:** +2 hours for unexpected issues (naming violations, build errors, missing dependencies)

**Total with Buffer:** ~9 hours

---

## Risk Assessment

### High Risk
- **Phase 3:** Many files to audit - time-consuming and error-prone
  - **Mitigation:** Batch files, create checklist, verify after each batch
- **Phase 5:** .vbproj creation - configuration complexity
  - **Mitigation:** Detailed INSTRUCTIONS.txt, test build immediately

### Medium Risk
- **Phase 2/3:** Designer file locking during edits
  - **Mitigation:** Follow ForgeCharter Section 4 (edit Designer.vb only)
- **Phase 6:** Breaking NewDatabaseGenerator build
  - **Mitigation:** Test builds after each change

### Low Risk
- **Phase 1:** Folder creation
- **Phase 4:** Documentation review (mostly verification)

---

## Success Metrics

### Per-Phase Metrics
- ? Phase completes without build errors
- ? Version log (vXXX.md) created and documented
- ? Character counts accurate on all modified files
- ? No Forge governance violations introduced

### Overall Metrics
- ? All 6 phases complete (v0.6.0)
- ? ForgeAudit manual checklist passes 100%
- ? Complete version history: v010 ? v020 ? v030 ? v040 ? v050 ? v060
- ? All projects build successfully
- ? RCHAutomation.Controls ready for Dashboard integration
- ? TemplateBuilder extracted and independent

---

## Notes

- **No automation tools** used to reduce premium costs and ensure Forge rule compliance
- **Manual ForgeAudit** after each phase (no automated script)
- **INSTRUCTIONS.txt** approach for .vbproj editing (user performs manual steps)
- **Sequential execution** recommended (no parallelization for safety)
- **TemplateBuilder folder** created at same root as other test modules per user requirement

---

**End of Implementation Plan**
