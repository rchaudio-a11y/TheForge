# TheForge Project Constitution

## Core Principles

### I. Forge Governance Supreme Authority
All features, code, and documentation must comply with ForgeCharter.md and Branch-* rules.

**Rules:**
- ForgeCharter.md is the ultimate authority
- Branch files (Architecture, Coding, Documentation) specialize but never override ForgeCharter
- All work must follow the Forge rule precedence hierarchy: ForgeCharter → Branch → Specific
- No implicit rule interpretation or modification

### II. Deterministic and Auditable
All work must be deterministic (reproducible), auditable (traceable), and compliant (100% ForgeAudit pass).

**Rules:**
- Every file must pass ForgeAudit 2.0 before commit
- All changes must be traceable to requirements
- No hidden or implicit file operations
- Version control for all governance and code files

### III. Metadata Headers Mandatory
All files must include Forge metadata headers per ForgeCharter Section 9.

**Required Fields:**
1. Document Type
2. Purpose
3. Created (YYYY-MM-DD)
4. Last Updated (YYYY-MM-DD)
5. Status (Active, Draft, Deprecated, Archived)
6. Character Count (auto-calculated by ForgeAudit)
7. Related (cross-references)

### IV. Explicit Intent Required
No implicit file creation, modification, or deletion per ForgeCharter Section 8.

**Rules:**
- All file operations must be explicitly requested
- AI must confirm file paths before operations
- No convenience files without approval
- Document intent for all structural changes

### V. Branch Independence
Respect branch boundaries and rule precedence hierarchy.

**Rules:**
- Code changes → Branch-Coding.md
- Structure changes → Branch-Architecture.md
- Documentation changes → Branch-Documentation.md
- Multi-domain tasks → Load all applicable branches
- Conflicts → ForgeCharter wins

## Technical Constraints

### Language & Framework
- **Primary Language:** VB.NET
- **Secondary Language:** C# (when justified)
- **Framework:** .NET 6+
- **IDE:** Visual Studio 2022+
- **Version Control:** Git

### Architecture
- **Modularity:** Follow Branch-Architecture.md rules
- **Designer Files:** Follow ForgeCharter Section 4 (only .Designer.vb files modified)
- **Naming:** No forbidden terms (Helper, Manager, Utils, Processor, Handler, Controller) unless justified
- **Organization:** Taxonomy-based placement per Branch-Architecture

### Documentation
- **Format:** Markdown (.md)
- **Character Limit:** 10,000 characters per file (ForgeCharter Section 9.4)
- **Taxonomy:** Follow Branch-Documentation.md (Codex, Grimoire, Tomes, Chronicle)
- **Headers:** Forge metadata headers mandatory (see Principle III)
- **Cross-references:** Related field must list connected files

## Development Workflow

### Spec-Kit Integration
1. Constitution → Define project principles (this file)
2. Specify → Create spec.md with user stories
3. Plan → Create plan.md with technical design
4. Tasks → Create tasks.md with atomic task breakdown
5. Implement → Execute tasks following Forge governance
6. Audit → Run ForgeAudit 2.0 on all changes
7. Commit → Only after 100% compliance

### Quality Gates
- **Pre-Implementation:** All specs, plans, tasks reviewed and approved
- **During Implementation:** Forge metadata headers added to all files
- **Post-Implementation:** ForgeAudit 2.0 must report 100% compliance
- **Pre-Commit:** No governance files modified without explicit approval

### Forge Vocabulary
**Prefer:**
- Forge, Charter, Branch, Audit, Module, Service, Repository, Component

**Avoid (unless justified):**
- Helper, Manager, Utility, Factory, Handler, Processor, Controller

## Governance

### Amendment Process
1. Identify need for constitutional change
2. Document proposed amendment in Forge-Improvement-Suggestions
3. Review against ForgeCharter Section 11
4. Update constitution with version increment
5. Update Last Amended date
6. Notify team of changes

### Compliance
- This constitution supersedes all other project practices except ForgeCharter.md
- All Spec-Kit workflows must reference this constitution
- All AI agents must load this constitution before task execution
- Violations must be documented in IssueSummary.md

### References
- **ForgeCharter.md** - Supreme governance authority
- **Branch-Architecture.md** - Structure and organization rules
- **Branch-Coding.md** - Code standards and naming
- **Branch-Documentation.md** - Documentation taxonomy and standards
- **ForgeAudit.md** - Compliance verification system

---

**Version:** 1.0.0  
**Ratified:** 2026-01-04  
**Last Amended:** 2026-01-04
