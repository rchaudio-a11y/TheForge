# Forge-Spec-Kit Unified Workflow

**Document Type:** Workflow Specification  
**Purpose:** Complete end-to-end workflow for feature development in TheForge ecosystem  
**Created:** 2025-01-04  
**Last Updated:** 2025-01-04  
**Status:** Active  
**Character Count:** TBD  
**Related:** ForgeCharter.md, copilot-instructions.md, SPECKIT_SETUP.md, File-Structure-Guide.md

---

## 1. Introduction

This document defines the complete, end-to-end workflow for feature development within **TheForge** ecosystem. It unifies:

- **Forge Governance** (rules, structure, compliance)  
- **Spec-Kit** (planning, specification, task generation)  
- **Visual Studio** (implementation environment)  
- **ForgeAudit 2.0** (compliance enforcement)  
- **Copilot Router** (task delegation and rule enforcement)

The workflow ensures all development is deterministic, auditable, modular, and fully compliant with ForgeCharter governance.

---

## 2. Workflow Overview

### 2.1 High-Level Sequence

```
Constitution ? Specification ? Planning ? Tasks ? Implementation ? Audit ? Commit
```

### 2.2 Core Principles

- **ForgeCharter.md** is the supreme authority
- **Branch-*.md** files specialize rules but never override the Charter
- **Spec-Kit** handles planning and structure
- **Visual Studio** handles implementation
- **ForgeAudit** ensures compliance
- **Copilot Router** ensures correct task routing

---

## 3. Governance Framework

### 3.1 ForgeCharter.md
Defines universal rules for structure, naming, metadata, documentation, and compliance.

### 3.2 Branch Files
- **Branch-Architecture.md** — structure and layout rules
- **Branch-Coding.md** — naming, coding, and forbidden terms
- **Branch-Documentation.md** — documentation taxonomy and formatting

### 3.3 ForgeAudit.md
Automated compliance checker validating all files against governance rules.

### 3.4 copilot-instructions.md
Routes tasks to the correct branch or Spec-Kit agent.

---

## 4. Spec-Kit Integration Model

### 4.1 Role of Spec-Kit
Spec-Kit provides structured planning and task generation. It does **not** integrate with Visual Studio and is executed via CLI or Copilot agents.

### 4.2 Role of Visual Studio
Visual Studio is the implementation environment for:

- Code
- Documentation
- Project structure
- Debugging

### 4.3 Role of Forge
Forge enforces governance, metadata, structure, and compliance.

---

## 5. Detailed Workflow Steps

---

### 5.1 Step 1: Constitution

**Command:** `/speckit.constitution`  
**Output:** `TheForge/Prompts/SpecKit/memory/constitution.md`

**Purpose:**  
Establish project principles, constraints, and governance context.

**References:**
- ForgeCharter.md
- Branch-*.md
- TagRegistry.md

---

### 5.2 Step 2: Specification

**Command:** `/speckit.specify`  
**Output:** `TheForge/Prompts/SpecKit/features/<feature>/spec.md`

**Purpose:**  
Define user stories, acceptance criteria, constraints, and success metrics.

**References:**
- constitution.md
- ForgeCharter Section 9

---

### 5.3 Step 3: Planning

**Command:** `/speckit.plan`  
**Output:** `TheForge/Prompts/SpecKit/features/<feature>/plan.md`

**Purpose:**  
Create the technical implementation plan.

**Includes:**
- File structure
- Dependencies
- Architecture decisions
- Compliance requirements
- Timeline and risks

**References:**
- spec.md
- Branch-Architecture
- Branch-Coding
- Branch-Documentation

---

### 5.4 Step 4: Task Breakdown

**Command:** `/speckit.tasks`  
**Output:** `TheForge/Prompts/SpecKit/features/<feature>/tasks.md`

**Purpose:**  
Convert the plan into actionable tasks.

**Includes:**
- Task IDs (T001, T002, etc.)
- Phases (Setup, Implementation, Polish)
- Parallelization markers [P]
- Story labels [US1], [US2]
- Compliance checkpoints

---

### 5.5 Step 5: Implementation

**Command:** `/speckit.implement`  
**Output:** Code + documentation in project directories

**Purpose:**  
Execute tasks using Visual Studio.

**Rules:**
- Follow Branch-Coding for code
- Follow Branch-Documentation for docs
- Follow Branch-Architecture for structure
- Maintain metadata headers
- Keep submodules clean
- Never overwrite governance files

---

### 5.6 Step 6: Audit

**Command:**
```
Run ForgeAudit 2.0 on [filename]
```

**Output:**
- Compliance report
- Auto-fixes (Character Count, Last Updated)
- Updated metadata
- Violation list

**Purpose:**  
Ensure all files meet ForgeCharter and Branch rules.

**Target:** 100% compliance score

---

### 5.7 Step 7: Commit + PR

**Prerequisites:**
- ? ForgeAudit passes with 100% compliance
- ? All tasks in tasks.md checked off
- ? All files have accurate metadata
- ? No governance files modified

Only after all prerequisites met.

---

## 6. File Flow Summary

| Step | Input | Output | Directory |
|------|--------|---------|-----------|
| Constitution | ForgeCharter | constitution.md | SpecKit/memory |
| Specification | constitution.md | spec.md | SpecKit/features/<name> |
| Planning | spec.md | plan.md | SpecKit/features/<name> |
| Tasks | plan.md | tasks.md | SpecKit/features/<name> |
| Implementation | tasks.md | code/docs | Project directories |
| Audit | all files | compliance report | .github/validation/reports |

---

## 7. Responsibilities by Role

### Developers
- Use Spec-Kit for planning
- Use Visual Studio for implementation
- Follow Branch-* rules
- Run ForgeAudit before commit

### AI Agents
- Load copilot-instructions.md
- Route tasks correctly
- Apply Spec-Kit agents
- Enforce governance
- Validate with ForgeAudit

### Project Managers
- Track features via `TheForge/Prompts/SpecKit/features/`
- Review specs, plans, tasks
- Monitor compliance reports

---

## 8. Why This Workflow Works

**Separation of Concerns:**
- Spec-Kit is CLI-driven (planning)
- Visual Studio is IDE-driven (implementation)
- Forge is governance-driven (compliance)

**This separation ensures:**
- ? Determinism
- ? Auditability
- ? Modularity
- ? Reproducibility
- ? Safety
- ? Long-term maintainability

---

## 9. Integration Points

### 9.1 Spec-Kit ? Forge
- Spec-Kit references ForgeCharter during planning
- Spec-Kit includes Forge compliance checkpoints in tasks
- Spec-Kit applies Branch-* rules during implementation

### 9.2 Forge ? Visual Studio
- Visual Studio applies Branch-* rules during coding
- Visual Studio maintains metadata headers
- Visual Studio respects project structure rules

### 9.3 ForgeAudit ? All
- ForgeAudit validates Spec-Kit outputs
- ForgeAudit validates Visual Studio outputs
- ForgeAudit enforces ForgeCharter compliance

---

## 10. Example: add-readme-files Feature

**Current Status:** Steps 1-4 complete, Step 5 (Implement) next

### Step 1: Constitution ?
**Output:** `memory/constitution.md`  
**Established:** Project principles, Forge compliance requirements

### Step 2: Specification ?
**Output:** `features/add-readme-files/spec.md`  
**Defined:** User Story 1 (P1) - Developer README files

### Step 3: Planning ?
**Output:** `features/add-readme-files/plan.md`  
**Created:** 4-phase plan, 3-hour timeline, Forge compliance strategy

### Step 4: Tasks ?
**Output:** `features/add-readme-files/tasks.md`  
**Generated:** 12 tasks in 3 phases, 2 parallelizable

### Step 5: Implementation ?
**Next:** Create README files with Forge metadata headers

### Step 6: Audit ?
**Next:** Run ForgeAudit 2.0 on both READMEs

### Step 7: Commit ?
**Next:** Commit after 100% compliance

---

## 11. Quick Reference

### Commands
```
/speckit.constitution   - Step 1
/speckit.specify        - Step 2
/speckit.plan           - Step 3
/speckit.tasks          - Step 4
/speckit.implement      - Step 5
Run ForgeAudit 2.0      - Step 6
git commit              - Step 7
```

### Key Files
- **Master Governance:** `ForgeCharter.md`
- **Router:** `.github/copilot-instructions.md`
- **Audit System:** `ForgeAudit.md`
- **Setup Guide:** `SPECKIT_SETUP.md`
- **File Guide:** `File-Structure-Guide.md`

### File Locations
- **Spec-Kit:** `TheForge/Prompts/SpecKit/`
- **Governance:** `TheForge/Prompts/`
- **Documentation:** `TheForge/Documentation/`

---

## 12. Summary

The Forge-Spec-Kit Unified Workflow is the backbone of TheForge ecosystem. It ensures every feature follows a consistent, structured, and compliant development process.

**This workflow is:**
- ? Mandatory for all new features
- ? Recommended for refactoring
- ? Recommended for documentation updates
- ? Recommended for architectural changes

**Success Criteria:**
- ? All steps completed in order
- ? ForgeAudit 100% compliance
- ? All tasks checked off
- ? Metadata accurate
- ? Governance files untouched

---

**Last Updated:** 2025-01-04  
**Maintained By:** The Forge System  
**Status:** ? Active and Current

---

**End of Unified Workflow Guide**
