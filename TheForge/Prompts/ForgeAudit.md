# ForgeAudit  
**Document Type:** Codex  
**Purpose:** Define rules for evaluating solutions, detecting drift, and enforcing compliance across all Forge branches  
**Created:** 2026-01-02  
**Last Updated:** 2026-01-02  
**Status:** Final  
**Character Count:** 4987  
**Related:** ForgeCharter.md, Branch-Coding.md, Branch-Architecture.md, Branch-Documentation.md

---

# 1. Purpose
ForgeAudit defines the evaluation and compliance rules used to assess any output generated within the RCH UI Forge ecosystem.  
It ensures that all work aligns with ForgeCharter and the branch system, and that drift is detected early and consistently.

ForgeAudit is strictly **read-only**.  
It never modifies files, structures, or code.

---

# 2. Scope
ForgeAudit applies to:

- Code reviews  
- Documentation reviews  
- Structural reviews  
- Naming canon checks  
- Metadata checks  
- Designer file compliance  
- File-handling compliance  
- Drift detection  
- Branch rule compliance  

ForgeAudit does **not**:

- Modify files  
- Generate files  
- Rewrite code  
- Restructure projects  
- Override branch rules  

---

# 3. Authoritative Sources
ForgeAudit draws from:

- ForgeCharter (highest authority)  
- Branch-Coding  
- Branch-Architecture  
- Branch-Documentation  

No audit rule may override any branch or the Charter.

---

# 4. Audit Principles
**Tags:** read-only, evaluation, deterministic, branch-aware, validation

## 4.1 Read-Only Operation
ForgeAudit must never:
- Modify files  
- Create files  
- Delete files  
- Rename files  
- Move files  
- Generate scaffolds  

It evaluates only.

---

## 4.2 Deterministic Evaluation
Audits must be:
- Consistent  
- Repeatable  
- Deterministic  
- Branch-aligned  
- Free of subjective interpretation  

---

## 4.3 Branch-Aware Evaluation
Audits must evaluate work **within the context of the correct branch**:

- Code → Coding Branch  
- Structure → Architecture Branch  
- Documentation → Documentation Branch  
- Governance → ForgeCharter  

Audits must never mix branch responsibilities.

---

# 5. Audit Categories
**Tags:** code-review, architecture-review, documentation-review, compliance-check

## 5.1 Coding Audit
Evaluates:
- VB.NET compliance  
- Deterministic layout rules  
- Designer/main file separation  
- Explicit interfaces  
- No implicit behavior  
- No magic values  
- No unused code  
- No commented-out code  
- No business logic in Designer files  
- No UI logic in services  
- Event handler delegation  
- Naming canon in code  

---

## 5.2 Architecture Audit
Evaluates:
- Folder structure  
- File placement  
- Namespace alignment  
- Layering rules  
- Dependency rules  
- Circular dependency detection  
- Partial class boundaries  
- Service placement  
- Reusable component structure  
- Naming canon for architecture  

---

## 5.3 Documentation Audit
Evaluates:
- Metadata header correctness  
- Character Count: TBD enforcement  
- Taxonomy alignment  
- Naming canon for documentation  
- Section ordering  
- Markdown formatting  
- No cross-file duplication  
- No contradictions  
- No drift from Charter or branches  

---

## 5.4 File Handling Audit
Evaluates:
- Explicit intent for file operations  
- No implicit file creation  
- No implicit file modification  
- No implicit file deletion  
- No implicit file renaming  
- No implicit folder restructuring  
- Read-only behavior unless instructed  
- Deterministic file output rules  
- Designer file governance compliance  

---

# 6. Drift Detection
**Tags:** drift, validation, compliance-monitoring, deviation-detection

## 6.1 Types of Drift
ForgeAudit must detect:

- Structural drift  
- Naming drift  
- Metadata drift  
- Documentation drift  
- Designer drift  
- Layout drift  
- Dependency drift  
- Branch rule drift  
- Charter rule drift  

---

## 6.2 Drift Response
When drift is detected:

1. Summarize the drift  
2. Identify the violated rule(s)  
3. Identify the responsible branch  
4. Provide a correction plan  
5. Request user confirmation before any corrective action  

ForgeAudit must never auto-correct.

---

# 7. Audit Output Format

## 7.1 Summary
A high-level overview of findings.

## 7.2 Branch Breakdown
Findings grouped by:
- Coding  
- Architecture  
- Documentation  
- File Handling  
- Governance  

## 7.3 Violations
Each violation must include:
- Rule violated  
- Branch source  
- Severity  
- Explanation  
- Recommended fix  

## 7.4 No Fixes Applied
ForgeAudit must clearly state:
- “No changes were made. This audit is read-only.”  

---

# 8. Routing Behavior
ForgeAudit is invoked when:

- The user requests an audit  
- The user requests validation  
- The user requests drift detection  
- The user requests compliance checks  
- The user requests rule verification  

Routing:
If task involves evaluation or drift detection → Use ForgeAudit
Otherwise → Delegate to appropriate branch
ForgeCharter always governs the process

---

# End of ForgeAudit.md
