# Branch-Documentation  
**Document Type:** Codex  
**Purpose:** Define rules for documentation structure, metadata, taxonomy, file creation, and documentation discipline  
**Created:** 2026-01-02  
**Last Updated:** 2026-01-02  
**Status:** Final  
**Character Count:** TBD  
**Related:** ForgeCharter.md, Branch-Coding.md, Branch-Architecture.md, ForgeAudit.md

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
- Compute the character count  
- Remove the field  
- Replace `TBD` with a number  
- Omit the field  

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

## 4.3 Taxonomy Rules
Documentation must follow a stable taxonomy:

- High-level governance → ForgeCharter  
- Branch rules → Branch-* files  
- Audit rules → ForgeAudit  
- Project documentation → User-defined folders  
- No implicit folder creation  
- No implicit taxonomy restructuring  

The Forge must not:
- Move documentation between folders implicitly  
- Rename documentation folders implicitly  
- Split documentation implicitly  
- Merge documentation implicitly  

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

# 7. Documentation Drift Prevention
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

# 8. Routing Behavior
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
