# Branch-Documentation  
**Document Type:** Codex  
**Purpose:** Define rules for documentation structure, metadata, taxonomy, file creation, and documentation discipline  
**Created:** 2025-01-02  
**Last Updated:** 2026-01-03  
**Status:** Final  
**Character Count:** 9847  
**Related:** ForgeCharter.md, Branch-Coding.md, Branch-Architecture.md, ForgeAudit.md, CONSTITUTION.md

---

# 1. Purpose
The Documentation Branch defines the rules governing all documentation produced within the RCH UI Forge ecosystem.  
It ensures clarity, consistency, taxonomy integrity, metadata correctness, and drift-free documentation behavior.

This branch governs **documentation only**.  
It does not define code rules, architectural rules, or audit rules.

---

# 2. Scope
The Documentation Branch applies to:

- Markdown files  
- Text files  
- Documentation metadata  
- Documentation taxonomy  
- Documentation file creation  
- Documentation file updates  
- Documentation structure and formatting  
- Documentation naming canon  
- Documentation drift detection  

This branch does **not** apply to:

- Code files (Coding Branch)  
- Project structure (Architecture Branch)  
- Audit behavior (ForgeAudit)  

---

# 3. Authoritative Sources
The Documentation Branch draws from:

- ForgeCharter (universal governance)  
- Metadata Header Governance  
- File Handling Governance  
- Documentation taxonomy principles  
- RCH UI Forge naming canon  

No other source may override this branch.

---

# 4. Documentation Rules
**Tags:** metadata, headers, markdown, structure, character-count

## 4.1 Metadata Header Requirements
All documentation files must begin with a metadata header containing:

- **Document Type:**  
- **Purpose:**  
- **Created:**  
- **Last Updated:**  
- **Status:**  
- **Character Count:** TBD  
- **Related:** (optional but recommended)

The Forge must never:
- Remove the field  
- Omit the field from new files

Per ForgeCharter Section 9.4, the Character Count must be updated to the actual computed value whenever a file is edited

**Examples:**

```markdown
# MyDocument.md

**Document Type:** Technical Guide  
**Purpose:** Explain module loading architecture  
**Created:** 2026-01-02  
**Last Updated:** 2026-01-02  
**Status:** Draft  
**Character Count:** 3847  
**Related:** ModuleLoaderService.vb, Branch-Architecture.md

---

## Content starts here...
```

```vb
' MyCodeFile.vb
'
' Document Type: Service Implementation
' Purpose: Load and manage external modules
' Created: 2026-01-02
' Last Updated: 2026-01-02
' Status: Production
' Character Count: 5623
' Related: IModuleLoader.vb, ModuleMetadata.vb

Namespace RCH.Forge.Services
    Public Class ModuleLoaderService
```  

---

## 4.2 Documentation Structure
Documentation must be:

- Clear  
- Hierarchical  
- Modular  
- Deterministic  
- Easy to navigate  

Sections must follow a logical order:
1. Purpose  
2. Scope  
3. Authoritative Sources  
4. Rules  
5. Examples (optional)  
6. Related Documents  

---

## 4.3 Taxonomy Rules (Enhanced)
**Tags:** taxonomy, organization, folder-structure, file-placement, constitutional-categories

Documentation must follow this stable taxonomy (per Constitution Section 7.2):

| Category | Purpose | Examples |
|----------|---------|----------|
| **Codex** | Technical references | API docs, interface specs, technical guides |
| **Chronicle** | Version history, logs | VersionHistory.chronicle.md, audit logs, development logs |
| **Tomes** | User guides, tutorials | ForgeTome.md, onboarding guides, user documentation |
| **Lore** | Design philosophy, principles | NamingCanon.md, architectural philosophy, governance rationale |
| **Grimoire** | Experimental features, research | Prototypes, proof-of-concepts, exploratory work |
| **Scriptorium** | Templates, generated docs | Scaffolds, auto-generated documentation, templates |

### Taxonomy Enforcement
- High-level governance → ForgeCharter (Prompts/)
- Branch rules → Branch-* files (Prompts/)
- Audit rules → ForgeAudit (Prompts/)
- Project documentation → Taxonomy folders (Documentation/)
- Constitution → `.github/CONSTITUTION.md`
- No implicit folder creation  
- No implicit taxonomy restructuring  

The Forge must not:
- Move documentation between folders implicitly  
- Rename documentation folders implicitly  
- Split documentation implicitly  
- Merge documentation implicitly  
- Create new taxonomy categories without explicit approval

**Constitutional Reference:** See CONSTITUTION.md Section 7.2 for complete taxonomy definitions.

---

## 4.4 Naming Canon for Documentation
Documentation file names must:

- Use PascalCase  
- Reflect purpose  
- Avoid vague names  
- Avoid abbreviations  
- Match the primary subject  

Examples:
- `ForgeCharter.md`  
- `Branch-Coding.md`  
- `Branch-Architecture.md`  
- `Branch-Documentation.md`  
- `ForgeAudit.md`  

---

## 4.5 Documentation Content Rules
Documentation must:

- Use clear, concise language  
- Avoid redundancy  
- Avoid contradictions  
- Avoid cross-file duplication  
- Reference other documents by name, not by path  
- Use consistent terminology  

Documentation must not:

- Contain code unless explicitly required  
- Contain implementation details (Coding Branch)  
- Contain structural rules (Architecture Branch)  

---

# 5. File Handling Rules (Documentation-Specific)

## 5.1 Explicit Intent Required
The Forge must not create, modify, delete, or rename documentation files unless explicitly instructed.

## 5.2 Read-Only by Default
Documentation files are read-only unless the user requests modification.

## 5.3 No Implicit File Generation
The Forge must not:
- Generate documentation as a side effect  
- Create scaffolds implicitly  
- Split files implicitly  
- Merge files implicitly  

## 5.4 Deterministic Output
When creating documentation:
- Confirm file name  
- Confirm file location  
- Confirm purpose  
- Confirm whether it supplements or replaces existing documentation  

---

# 6. Formatting Rules

## 6.1 Markdown Standards
Documentation must use:

- `#` for top-level headings  
- `##` for secondary headings  
- `###` for tertiary headings  
- Code blocks for code  
- Bullet lists for enumerations  
- No trailing whitespace  
- No inconsistent indentation  

---

## 6.2 Line Length & Readability
- No line should exceed 120 characters  
- Paragraphs should be short  
- Lists should be used for clarity  
- Headings must be descriptive  

---

# 7. Common Mistakes
**Tags:** errors, documentation-drift, audit-assumptions, metadata-compliance

**Note:** See `Documentation/Chronicle/DevelopmentLog/IssueSummary.md` for full patterns and solutions.

## 7.1 Documentation Drift
❌ Creating parallel documentation systems (e.g., VersionHistory + DevelopmentLog)  
❌ Scattering related files across multiple folders  
❌ Inconsistent naming conventions within a category  
❌ Deleting outdated files instead of deprecating  
✅ Single source of truth for each topic  
✅ Consolidate related docs into logical folders  
✅ Deprecate with redirect content, don't delete  
✅ Schedule periodic reorganization milestones  

## 7.2 Audit Assumptions
❌ Assuming files don't exist without checking  
❌ Planning work without verifying current state  
❌ Not crediting work done in earlier sessions  
✅ Verify audit assumptions with searches before starting  
✅ Check if work already exists  
✅ Update audit methodology when gaps found  

## 7.3 Metadata Headers
❌ Omitting Character Count field  
❌ Leaving Character Count as TBD after editing  
❌ Missing Last Updated date after changes  
✅ All files must have metadata headers  
✅ Update Character Count after every edit  
✅ Update Last Updated date when modifying  

---

# 8. Documentation Drift Prevention
Before generating or modifying documentation, the Forge must:

- Detect taxonomy drift  
- Detect naming drift  
- Detect metadata drift  
- Detect structural drift  
- Detect cross-file duplication  
- Detect contradictions with other branches  

If drift is detected:
- Summarize the issue  
- Request confirmation  
- Never auto-correct silently  

---

# 9. Documentation Workflow Efficiency
**Tags:** workflow, build, efficiency, iteration, separation-of-concerns

## 9.1 Build Verification for Documentation
Documentation-only changes do **not** require build verification:
- Focus on content quality, not compilation
- Build only if code was also modified in the same change
- Allows rapid iteration on documentation without build overhead

## 9.2 Rationale
- Documentation files (`.md`, `.txt`) are not compiled
- Building wastes time and provides no value for docs-only changes
- Separation of concerns: documentation quality ≠ compilation success
- Enables fast documentation updates without waiting for builds

## 9.3 When to Build Anyway
Build if:
- Documentation **and** code changed in same session
- Adding code examples that should be validated
- Updating API documentation after code changes
- Modifying embedded code snippets that reference actual code

**Examples:**
```
❌ Build NOT Required:
- Updated README.md
- Modified ForgeTome.md in /Documentation/Tomes/
- Enhanced CONSTITUTION.md
- Updated Branch-Documentation.md
- Fixed typos in VersionHistory.chronicle.md

✅ Build Required (Mixed Changes):
- Updated ModuleLoaderService.vb AND updated API documentation
- Added new IModule method AND updated interface documentation
- Modified UI control AND updated user guide with new screenshots
```

**Constitutional Reference:** See CONSTITUTION.md Section 4.1 for complete build verification rules.

---

# 10. Routing Behavior
The Documentation Branch is invoked when:

- The user requests documentation  
- The user requests documentation updates  
- The user requests documentation structure  
- The user requests metadata headers  
- The user requests taxonomy decisions  
- The user requests documentation formatting  

Routing:
If task involves documentation → Use Branch-Documentation
Otherwise → Delegate to appropriate branch
ForgeCharter always governs the process

---

# End of Branch-Documentation.md
