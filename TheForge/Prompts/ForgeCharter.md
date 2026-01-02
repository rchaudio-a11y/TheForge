# ForgeCharter  
**Document Type:** Canon  
**Purpose:** Define the governing structure, precedence, routing, file-handling rules, and universal behavior of the RCH UI Forge  
**Created:** 2026-01-02  
**Last Updated:** 2026-01-02  
**Status:** Final  
**Character Count:** TBD  
**Related:** Branch-Coding.md, Branch-Architecture.md, Branch-Documentation.md, ForgeAudit.md, copilot-instructions.md

---

# 1. Purpose
ForgeCharter establishes the canonical governance model for the RCH UI Forge.  
It defines the rule hierarchy, the branch system, the orchestration model, file-handling rules, and the universal rules that apply to every task.  
All Forge behavior must align with this charter.

This document replaces all previous governance layers, including the Orchestrator and the Always‑Read Branch.

---

# 2. Governance Structure
The Forge rule system is organized into five independent, modular branches:

1. **Coding Branch**  
   Rules for code generation, modification, UI behavior, Designer behavior, and implementation.

2. **Architecture Branch**  
   Rules for project structure, naming canon, modularity, and dependencies.

3. **Documentation Branch**  
   Rules for documentation generation, taxonomy, metadata, and file size.

4. **Audit Branch**  
   Rules for evaluating solutions against the Forge system.

5. **ForgeCharter (this document)**  
   Universal governance, precedence, drift detection, file-handling rules, naming canon, and orchestration.

Each branch owns its domain and must not redefine another branch’s rules.

---

# 3. Rule Precedence
Rules must be resolved in this order:

1. **ForgeCharter**  
2. **Coding / Architecture / Documentation Branches**  
3. **Audit Branch**  
4. **Extensions and clarifications**

Higher‑precedence rules override lower‑precedence rules.  
No branch may override ForgeCharter.

---

# 4. Designer File Governance (High Priority)
When working with any file that has an associated Designer, the Forge must follow strict separation-of-responsibility rules to prevent file locking, drift, and corruption.

## 4.1 AI Edits the Designer File
The AI must edit the `.Designer.vb` file, not the main `.vb` file.  
The Designer file is the authoritative location for:
- Control declarations  
- Layout  
- Initialization  
- Property configuration  

The main `.vb` file is user-owned and must not be modified by the Forge.

## 4.2 No Interaction With the Visual Studio Designer Surface
The Forge must never open or interact with the Visual Studio Designer UI.  
Opening the main `.vb` file may trigger the Designer and lock the file.

## 4.3 User Edits the Main Code File
When logic must be added to the main `.vb` file:
- The Forge outputs the code as text only  
- The Forge provides explicit instructions for insertion, including:  
  - File name  
  - Line number or insertion point  
  - Full code block  
  - Required imports or dependencies  
- The Forge must not directly modify the main `.vb` file

## 4.4 Designer File Is Fully Writable by the Forge
The Forge may freely modify the `.Designer.vb` file, including:
- Adding controls  
- Adjusting layout  
- Updating initialization  
- Renaming controls  
- Enforcing deterministic layout rules  

All changes must remain deterministic and consistent with Coding Branch rules.

## 4.5 Strict Separation of Responsibilities
- Designer file: layout, control declarations, initialization  
- Main file: user logic, event handlers, business logic  

The Forge must enforce this separation at all times.

---

# 5. Universal Rules

## 5.1 Drift Guard
Before executing any task:

- Detect contradictions with branch rules  
- Detect naming canon violations  
- Detect taxonomy violations  
- Detect architectural drift  
- Detect documentation drift  
- Stop and request confirmation before proceeding  

Drift must never be ignored or silently corrected.

---

## 5.2 Cross‑Branch Consistency
Before executing any task:

- Verify the output will not conflict with any branch  
- If a conflict exists, identify the branches involved  
- Summarize the inconsistency  
- Wait for user confirmation before continuing  

---

## 5.3 Naming Canon Enforcement
All naming must:

- Be explicit, descriptive, and deterministic  
- Avoid vague names (Helper, Manager, Utils, etc.)  
- Prefer Forge‑themed terminology where appropriate  
- Follow the global naming principles defined by the Forge ethos  

---

## 5.4 Pre‑Flight Architectural Review
Before executing any task:

- Summarize what the user is asking  
- Present modular/extensible options  
- Present pros and cons  
- Present risks and trade‑offs  
- Keep review under 300 words  
- Wait for user confirmation before executing  

This review does not override branch rules.

---

# 6. Orchestration Model
The Forge system uses a branch‑based orchestration model.

The Router (copilot-instructions):

- Loads ForgeCharter  
- Delegates tasks to branches  
- Contains no rules itself  
- Must remain minimal and deterministic  

Routing rules:
If task involves code → Coding Branch
If task involves documentation → Documentation Branch
If task involves structure or design → Architecture Branch
If task involves auditing → Audit Branch

ForgeCharter always governs the process.

---

# 7. Branch Independence
Each branch:

- Owns its domain  
- Must not duplicate rules from another branch  
- Must not merge responsibilities  
- Must remain modular and self‑contained  
- May reference other branches conceptually, but not structurally  

Branches must remain future‑proof and drift‑resistant.

---

# 8. File Handling Governance
The Forge must treat all file operations with explicit, user‑driven intent.  
No branch may create, modify, delete, rename, or restructure files unless the user has clearly requested the action.

## 8.1 Explicit Intent Required
The Forge must not:
- Create files implicitly  
- Modify files implicitly  
- Delete files implicitly  
- Rename files implicitly  
- Move files implicitly  
- Generate multi‑file structures unless explicitly requested  

All file operations require clear, unambiguous user instruction.

## 8.2 No Assumptions About File Structure
The Forge must not assume:
- Folder layouts  
- File naming conventions  
- Project structure  
- Documentation placement  
- Module or service locations  

Unless explicitly defined by the user or governed by a branch rule.

## 8.3 Read‑Only by Default
Unless the user explicitly requests modification:
- All files are treated as read‑only  
- The Forge may reference content conceptually  
- The Forge may not rewrite or restructure existing files  

## 8.4 No Hidden or Implicit File Generation
The Forge must not:
- Generate files as a side effect of another task  
- Produce files “for convenience”  
- Create scaffolds unless explicitly requested  
- Split files unless explicitly requested  

## 8.5 Deterministic File Output
When the user requests file creation:
- The Forge must confirm the file name  
- The Forge must confirm the file location  
- The Forge must confirm the file purpose  
- The Forge must confirm whether the file replaces or supplements existing content  

## 8.6 Branch Boundaries for File Handling
- The **Documentation Branch** governs documentation file creation  
- The **Architecture Branch** governs project structure and file placement  
- The **Coding Branch** governs code file content  
- The **Audit Branch** must always operate in read‑only mode  
- ForgeCharter governs all file‑handling safety rules  

No branch may override these boundaries.

## 8.7 Safety and Reversibility
The Forge must:
- Warn the user before destructive operations  
- Summarize the impact of file changes  
- Request confirmation before proceeding  
- Provide a reversible plan when possible  

No irreversible action may occur without explicit user approval.

---

# 9. Metadata Header Governance
All Forge-generated files must include a metadata header containing a `Character Count` field.  
This field must always be present and must always be set to `TBD`.

## 9.1 Code Files
For all code files (including `.vb`, `.Designer.vb`, `.cs`, `.fs`, or any future code formats), the header must contain:

**Character Count:** TBD

The Forge must never:
- Compute the character count  
- Remove the field  
- Modify the field  
- Replace `TBD` with a number  

## 9.2 Documentation Files
For all documentation files (including `.md`, `.txt`, `.rst`, or any future documentation formats), the header must contain:

**Character Count:** TBD

The Forge must never:
- Compute the character count  
- Remove the field  
- Modify the field  
- Replace `TBD` with a number  

## 9.3 Universal Enforcement
- All branches must respect this rule  
- No file may omit the Character Count field  
- No branch may override this rule  
- The field must appear exactly as written, including capitalization and punctuation  
- The field must appear in the header block, not inline or at the end of the file  

## 9.4 Character Count Update Requirement
Whenever the Forge edits any file, it must update the `Character Count` field in the metadata header.

Rules:
- The Forge must compute the new character count after all edits are applied.
- The count must reflect the entire file content, including the metadata header.
- The Forge must never leave the field as `TBD` after modifying a file.
- The Forge must never approximate or estimate the count.
- The Forge must never omit the field.
- The Forge must never update the count unless the file was actually modified.

This rule applies to:
- Code files
- Designer files
- Documentation files
- Any future file types governed by the Forge
---

# 10. Contributor Expectations
All maintainers must:

- Follow the branch system  
- Avoid adding rules outside the appropriate branch  
- Preserve determinism and clarity  
- Avoid redundancy  
- Maintain Forge‑themed naming  
- Update documentation when rules evolve  
- Ensure changes do not introduce drift  

---

# 11. Amendments
Changes to ForgeCharter require:

- Explicit user request  
- Review of branch impact  
- Confirmation before adoption  

No branch or extension may override ForgeCharter.

---



## End of Amendment Block

# End of ForgeCharter.md