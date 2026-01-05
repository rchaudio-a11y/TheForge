# Spec-Kit Tutorial: Adding README Files to New Projects

**Document Type:** Tutorial Guide  
**Purpose:** Step-by-step guide for using Spec-Kit to add README files to NewDatabaseGenerator and RCHAutomation.Controls  
**Created:** 2025-01-04  
**Last Updated:** 2025-01-04  
**Status:** Active Tutorial  
**Character Count:** 21847  
**Related:** SPECKIT_SETUP.md, ForgeCharter.md, Branch-Documentation.md

---

## ?? Overview

This tutorial teaches you how to use **Spec-Kit** to create README files for your newly integrated projects while maintaining compliance with **The Forge** governance system.

**Projects to Document:**
1. NewDatabaseGenerator (main application)
2. RCHAutomation.Controls (controls library)

**What You'll Learn:**
- How to use Spec-Kit workflow commands
- How to integrate Spec-Kit with Forge governance
- How to create Forge-compliant documentation

**Time Estimate:** 30-45 minutes

---

## ? Prerequisites Checklist

Before starting, ensure:

- [ ] Spec-Kit is installed (verify: `SPECKIT_SETUP.md` exists)
- [ ] You have ForgeCharter.md and Branch-Documentation.md open (for reference)
- [ ] Visual Studio is open with TheForge solution loaded
- [ ] GitHub Copilot extension is active in Visual Studio

---

## ?? Learning Objectives

By the end of this tutorial, you will:
1. ? Understand the Spec-Kit workflow (specify ? plan ? tasks ? implement)
2. ? Create your first Spec-Kit specification
3. ? Generate implementation tasks from specs
4. ? Implement tasks while respecting Forge governance
5. ? Run ForgeAudit 2.0 to verify compliance

---

## ?? Part 1: Understanding the Workflow

### The Spec-Kit Workflow

```
Step 1: /speckit.specify    ? Define WHAT to build (requirements)
Step 2: /speckit.plan       ? Define HOW to build it (tech plan)
Step 3: /speckit.tasks      ? Generate action items (task breakdown)
Step 4: /speckit.implement  ? Execute the tasks (implementation)
Step 5: ForgeAudit 2.0      ? Verify compliance (governance check)
```

### How Spec-Kit and Forge Work Together

```
Spec-Kit (Planning)          The Forge (Governance)
    ?                              ?
"Create README.md"    +    "Use metadata headers"
    ?                              ?
         Result: Forge-compliant README.md
```

**Key Principle:** Spec-Kit defines *what* and *when*, Forge defines *how* and *standards*.

---

## ?? Part 2: Step-by-Step Tutorial

### Step 1: Define the Specification

**What You'll Do:** Create a specification document that defines the README requirements.

**Command to Run:**

1. Open **GitHub Copilot Chat** in Visual Studio (View ? GitHub Copilot Chat)
2. Type the following command:

```
/speckit.specify

Feature: Add README files to NewDatabaseGenerator projects

User Story 1 (P1): As a developer new to this solution, I want README files in NewDatabaseGenerator/ and RCHAutomation.Controls/ folders so I can understand what each project does and how they integrate with TheForge.

Acceptance Criteria:
- NewDatabaseGenerator/README.md exists and contains:
  - Project purpose and description
  - Key features
  - Dependencies (including RCHAutomation.Controls reference)
  - How it integrates with TheForge solution
  - How to build and run
  - Architecture overview
  
- RCHAutomation.Controls/README.md exists and contains:
  - Library purpose and description
  - Available controls list
  - Usage examples
  - Dependencies
  - How NewDatabaseGenerator uses it
  - API documentation reference

Both READMEs must follow:
- Markdown formatting standards
- Forge metadata header requirements (per ForgeCharter Section 9)
- Branch-Documentation rules
```

**What Spec-Kit Will Do:**
- Create `.specify/features/add-readme-files/spec.md`
- Parse your user story and acceptance criteria
- Ask clarifying questions if needed

**Expected Output:**
```
? Created: .specify/features/add-readme-files/spec.md
?? Feature: add-readme-files
?? User Stories: 1 (Priority: P1)
```

**Your Action:**
- [ ] Run the `/speckit.specify` command above
- [ ] Review the generated `spec.md` file
- [ ] Verify user story and acceptance criteria are captured correctly

---

### Step 2: Create Implementation Plan

**What You'll Do:** Generate a technical implementation plan based on the specification.

**Command to Run:**

In GitHub Copilot Chat, type:

```
/speckit.plan
```

**What Spec-Kit Will Do:**
- Read your `spec.md`
- Analyze the requirements
- Create `plan.md` with:
  - Tech stack (Markdown, Forge metadata)
  - File structure (where files will be created)
  - Libraries/dependencies (none for this task)
  - Implementation strategy

**Expected Output:**
```
? Created: .specify/features/add-readme-files/plan.md

Tech Stack:
- Markdown (.md files)
- Forge metadata headers (per ForgeCharter)

Files to Create:
- NewDatabaseGenerator/README.md
- RCHAutomation.Controls/README.md

Dependencies:
- ForgeCharter.md (for metadata header format)
- Branch-Documentation.md (for content structure)
```

**Your Action:**
- [ ] Run `/speckit.plan`
- [ ] Review `plan.md`
- [ ] **IMPORTANT:** Check if plan conflicts with Forge rules
  - If plan suggests non-Forge-compliant structure, note it for editing
  - Compare with ForgeCharter Section 9 (metadata headers)
  - Compare with Branch-Documentation Section 4 (documentation rules)

**Forge Compliance Check:**
```markdown
? Plan mentions metadata headers? (Required per ForgeCharter 9.1)
? Plan follows Branch-Documentation taxonomy? (Should be in project root, not /Documentation/)
? Plan includes Character Count field? (Required per ForgeCharter 9.2)
```

---

### Step 3: Generate Task Breakdown

**What You'll Do:** Convert the plan into actionable, ordered tasks.

**Command to Run:**

```
/speckit.tasks
```

**What Spec-Kit Will Do:**
- Read `spec.md` and `plan.md`
- Generate `tasks.md` with checklist format
- Organize tasks by phase and user story
- Mark parallelizable tasks with `[P]`
- Add story labels `[US1]`

**Expected Output:**
```
? Created: .specify/features/add-readme-files/tasks.md

Total Tasks: 8
Phases: 3 (Setup, User Story 1, Polish)
Parallelizable Tasks: 2
```

**Expected Task Structure:**
```markdown
## Phase 1: Setup
- [ ] T001 Review NewDatabaseGenerator project structure
- [ ] T002 Review RCHAutomation.Controls project structure
- [ ] T003 Review ForgeCharter metadata header requirements

## Phase 2: User Story 1 - Developer README Files
- [ ] T004 [P] [US1] Create NewDatabaseGenerator/README.md with metadata header
- [ ] T005 [P] [US1] Create RCHAutomation.Controls/README.md with metadata header
- [ ] T006 [US1] Add project descriptions and key features
- [ ] T007 [US1] Document dependencies and integration points
- [ ] T008 [US1] Add build/run instructions

## Phase 3: Polish
- [ ] T009 Verify both READMEs follow Markdown standards
- [ ] T010 Run ForgeAudit 2.0 on new documentation files
```

**Your Action:**
- [ ] Run `/speckit.tasks`
- [ ] Review `tasks.md`
- [ ] **IMPORTANT:** Verify Forge compliance of tasks:
  - [ ] Task T004 mentions metadata header? ?
  - [ ] Task T005 mentions metadata header? ?
  - [ ] Task T010 includes ForgeAudit? ?

**If Tasks Are Not Forge-Compliant:**

Edit `tasks.md` manually. Example fix:

```markdown
? BEFORE:
- [ ] T004 [P] [US1] Create NewDatabaseGenerator/README.md

? AFTER:
- [ ] T004 [P] [US1] Create NewDatabaseGenerator/README.md with Forge metadata header per ForgeCharter Section 9.2
```

---

### Step 4: Implement the Tasks

**What You'll Do:** Execute each task in order, following both Spec-Kit tasks and Forge governance.

**Command to Run:**

```
/speckit.implement
```

**What Spec-Kit Will Do:**
- Guide you through each task in `tasks.md`
- Mark tasks complete as you go
- Ask if you want to proceed to next task

**Implementation with Forge Compliance:**

For **Task T004** (Create NewDatabaseGenerator/README.md):

Spec-Kit will suggest:
```markdown
# NewDatabaseGenerator

## Overview
This is the NewDatabaseGenerator project...
```

**YOU MUST ADD** Forge metadata header:
```markdown
# NewDatabaseGenerator

**Document Type:** Project Documentation  
**Purpose:** README for NewDatabaseGenerator main application  
**Created:** 2025-01-04  
**Last Updated:** 2025-01-04  
**Status:** Active  
**Character Count:** TBD  
**Related:** RCHAutomation.Controls, TheForge.sln

---

## Overview
This is the NewDatabaseGenerator project...
```

**Your Action (Per Task):**

**T001-T003 (Setup):**
- [ ] Open ForgeCharter.md (already open)
- [ ] Open Branch-Documentation.md (already open)
- [ ] Review Section 9 (Metadata Headers)
- [ ] Review Section 4 (Documentation Rules)

**T004 (Create NewDatabaseGenerator/README.md):**
- [ ] Run `/speckit.implement` and select task T004
- [ ] Let Spec-Kit generate initial content
- [ ] **ADD** Forge metadata header (see template above)
- [ ] Add project description
- [ ] List key features
- [ ] Document dependencies:
  - FastColoredTextBox.Net6
  - System.Data.OleDb
  - RCHAutomation.Controls (project reference)
- [ ] Explain integration with TheForge
- [ ] Add build instructions

**T005 (Create RCHAutomation.Controls/README.md):**
- [ ] Run `/speckit.implement` and select task T005
- [ ] Let Spec-Kit generate initial content
- [ ] **ADD** Forge metadata header
- [ ] Add library description
- [ ] List available controls (you may need to explore project)
- [ ] Add usage examples
- [ ] Document dependencies
- [ ] Explain how NewDatabaseGenerator uses it

**T006-T008 (Content):**
- [ ] Follow Spec-Kit guidance for content
- [ ] Ensure all sections are complete
- [ ] Cross-reference between READMEs

**T009 (Markdown Validation):**
- [ ] Check Markdown syntax (headings, lists, code blocks)
- [ ] Verify links work
- [ ] Check formatting consistency

---

### Step 5: Run ForgeAudit 2.0

**What You'll Do:** Verify your new README files comply with Forge governance.

**Command to Run:**

In GitHub Copilot Chat, type:

```
Run ForgeAudit 2.0 on NewDatabaseGenerator/README.md and RCHAutomation.Controls/README.md
```

**What ForgeAudit Will Do:**
- Check metadata headers (ForgeCharter Section 9)
- Verify Character Count field exists
- Auto-fix Character Count if TBD
- Check documentation structure (Branch-Documentation Section 4)
- Report violations

**Expected Output:**
```
? ForgeAudit 2.0 Report

Auto-Fixes Applied:
1. ? NewDatabaseGenerator/README.md - Character Count: TBD ? 2847
2. ? RCHAutomation.Controls/README.md - Character Count: TBD ? 3124

Content Violations: 0
Compliance Score: 100%
```

**If Violations Found:**

Example violation:
```
? Violation: Missing "Last Updated" field
File: NewDatabaseGenerator/README.md
Severity: MINOR
Fix: Add "**Last Updated:** 2025-01-04" to metadata header
```

**Your Action:**
- [ ] Fix any reported violations
- [ ] Re-run ForgeAudit 2.0
- [ ] Achieve 100% compliance before committing

---

## ? Part 3: Verification and Completion

### Final Checklist

Before marking the feature complete, verify:

**Spec-Kit Compliance:**
- [ ] All tasks in `tasks.md` are checked off
- [ ] Both README files exist in correct locations
- [ ] Content matches acceptance criteria from `spec.md`

**Forge Compliance:**
- [ ] Both READMEs have metadata headers (per ForgeCharter 9.1, 9.2)
- [ ] Character Count field is accurate (not TBD)
- [ ] Last Updated field is current date
- [ ] Document Type is correct ("Project Documentation")
- [ ] Related field references correct files
- [ ] ForgeAudit 2.0 shows 100% compliance

**Git Compliance:**
- [ ] Both README files are tracked by Git
- [ ] Spec-Kit files in `.specify/` are committed (optional but recommended)
- [ ] Commit message is descriptive

**Quality Check:**
- [ ] README content is accurate and helpful
- [ ] Links work (if any)
- [ ] Markdown renders correctly
- [ ] Grammar and spelling are correct

---

## ?? Part 4: Understanding What You Created

### File Structure After Completion

```
TheForge/
??? .specify/                           ? Spec-Kit workspace
?   ??? features/
?       ??? add-readme-files/
?           ??? spec.md                 ? Requirements
?           ??? plan.md                 ? Technical plan
?           ??? tasks.md                ? Task breakdown
??? NewDatabaseGenerator/
?   ??? NewDatabaseGenerator/
?   ??? RCHAutomation.Controls/
?   ?   ??? README.md                   ? ? NEW FILE (Forge-compliant)
?   ??? README.md                       ? ? NEW FILE (Forge-compliant)
??? TheForge/
```

### What Each File Does

**`.specify/features/add-readme-files/spec.md`**
- Purpose: Requirements and user stories
- Used by: Spec-Kit for planning
- Maintained by: You (when requirements change)

**`.specify/features/add-readme-files/plan.md`**
- Purpose: Technical implementation plan
- Used by: Spec-Kit for generating tasks
- Maintained by: You (when approach changes)

**`.specify/features/add-readme-files/tasks.md`**
- Purpose: Actionable task checklist
- Used by: You during implementation
- Maintained by: Spec-Kit (marks tasks complete)

**`NewDatabaseGenerator/README.md`**
- Purpose: Project documentation
- Used by: Developers new to the project
- Maintained by: You (keep updated as project evolves)

**`RCHAutomation.Controls/README.md`**
- Purpose: Library documentation
- Used by: Developers using the controls library
- Maintained by: You (keep updated as API changes)

---

## ?? Part 5: What You Learned

### Spec-Kit Skills

You now know how to:
1. ? Create specifications with `/speckit.specify`
2. ? Generate implementation plans with `/speckit.plan`
3. ? Break work into tasks with `/speckit.tasks`
4. ? Implement features with `/speckit.implement`
5. ? Navigate the `.specify/` workspace

### Forge Integration Skills

You now understand:
1. ? How Spec-Kit and Forge work together
2. ? When to apply Forge governance (during implementation)
3. ? How to add Forge metadata headers to documentation
4. ? How to use ForgeAudit 2.0 to verify compliance
5. ? The importance of Character Count and Last Updated fields

### Best Practices

You learned these patterns:
1. ? Always review Spec-Kit output for Forge compliance
2. ? Edit generated files if they conflict with governance
3. ? Run ForgeAudit 2.0 before committing
4. ? Keep `.specify/` files as a record of requirements
5. ? Update Character Count when editing files

---

## ?? Part 6: Next Steps

### Practice More with Spec-Kit

Try these progressively harder tasks:

**Easy:**
1. Add CHANGELOG.md files to both projects
2. Create CONTRIBUTING.md for NewDatabaseGenerator
3. Add API documentation for RCHAutomation.Controls

**Medium:**
1. Add metadata headers to NewDatabaseGenerator code files
2. Create a new Forge Module using Spec-Kit
3. Refactor project structure using Spec-Kit

**Advanced:**
1. Implement a new feature in NewDatabaseGenerator
2. Create integration tests using Spec-Kit + Forge
3. Build a CI/CD pipeline specification

### Explore Advanced Spec-Kit Commands

**`/speckit.clarify`** - Ask clarifying questions before planning:
```
/speckit.clarify

I want to add unit tests to RCHAutomation.Controls. What testing framework should I use?
```

**`/speckit.analyze`** - Check consistency across specs:
```
/speckit.analyze

Check if all Spec-Kit features are Forge-compliant
```

**`/speckit.checklist`** - Generate quality checklists:
```
/speckit.checklist

Generate a code review checklist for NewDatabaseGenerator
```

---

## ?? Part 7: Reference Materials

### Quick Command Reference

| Command | Purpose | When to Use |
|---------|---------|-------------|
| `/speckit.specify` | Define requirements | Starting new feature |
| `/speckit.plan` | Create tech plan | After spec approved |
| `/speckit.tasks` | Generate task list | Before implementation |
| `/speckit.implement` | Execute tasks | During development |
| `/speckit.clarify` | Ask questions | Before planning (optional) |
| `/speckit.analyze` | Check consistency | After tasks (optional) |
| `ForgeAudit 2.0` | Verify compliance | Before committing |

### Forge Rules Quick Reference

**ForgeCharter Section 9: Metadata Headers**
- All documentation files must have metadata headers
- Character Count: TBD (updated by ForgeAudit)
- Last Updated: YYYY-MM-DD
- Document Type: [type]
- Purpose: [purpose]

**Branch-Documentation Section 4: Documentation Rules**
- Use clear, hierarchical structure
- Follow markdown standards
- Reference related documents by name
- Keep content modular and deterministic

**ForgeAudit 2.0: Auto-Fix Rules**
- Character Count: Auto-calculated
- Last Updated: Auto-updated when file modified
- Spelling errors: Auto-corrected in documentation
- IssueSummary.md: Auto-updated with new patterns

---

## ? Part 8: Troubleshooting

### Common Issues and Solutions

**Issue 1: Spec-Kit creates files without metadata headers**

**Solution:**
```markdown
1. Let Spec-Kit create the file
2. Manually add Forge metadata header at the top
3. Run ForgeAudit 2.0 to auto-calculate Character Count
```

**Issue 2: Spec-Kit conflicts with Forge naming rules**

**Example:** Spec-Kit suggests `DatabaseHelper.vb` but Forge forbids "Helper"

**Solution:**
```markdown
1. Edit plan.md before running /speckit.tasks
2. Change "DatabaseHelper" to "DatabaseService"
3. Continue with /speckit.tasks
```

**Issue 3: ForgeAudit reports violations after Spec-Kit implementation**

**Solution:**
```markdown
1. Read the violation report carefully
2. Fix each violation manually
3. Re-run ForgeAudit 2.0
4. Repeat until 100% compliance
```

**Issue 4: Spec-Kit modifies Forge governance files**

**Solution:**
```markdown
1. git checkout TheForge/Prompts/* (restore Forge files)
2. Tell Spec-Kit: "Do not modify files in TheForge/Prompts/"
3. Continue with implementation
```

**Issue 5: Can't find generated Spec-Kit files**

**Solution:**
```markdown
1. Check .specify/features/[feature-name]/ folder
2. Feature name is derived from your /speckit.specify command
3. Use File Explorer to search for "spec.md"
```

---

## ?? Success Criteria

You've successfully completed this tutorial when:

? **Both README files exist:**
- NewDatabaseGenerator/README.md
- RCHAutomation.Controls/README.md

? **Both READMEs are Forge-compliant:**
- Have metadata headers
- Character Count is accurate
- Pass ForgeAudit 2.0 with 100% score

? **Spec-Kit workspace is complete:**
- .specify/features/add-readme-files/spec.md exists
- .specify/features/add-readme-files/plan.md exists
- .specify/features/add-readme-files/tasks.md exists
- All tasks are checked off

? **You understand the workflow:**
- Can explain Spec-Kit ? Forge relationship
- Know when to use each system
- Can run commands independently

---

## ?? Conclusion

Congratulations! You've learned how to use Spec-Kit while maintaining Forge governance compliance.

**Key Takeaways:**
1. Spec-Kit handles *what* and *when* (planning)
2. The Forge handles *how* and *standards* (governance)
3. Both systems complement each other
4. Always run ForgeAudit 2.0 before committing
5. Edit Spec-Kit output when it conflicts with Forge rules

**You're now ready to:**
- Create specifications for new features
- Generate implementation plans
- Break work into actionable tasks
- Implement while respecting governance
- Maintain both Spec-Kit and Forge documentation

**Need Help?**
- Review: `SPECKIT_SETUP.md`
- Governance: `TheForge/Prompts/ForgeCharter.md`
- Documentation: `TheForge/Prompts/Branch-Documentation.md`
- Audit: `TheForge/Prompts/ForgeAudit.md`

---

**Tutorial Complete! Ready to start?** ??

Follow the steps in order, check off items as you complete them, and remember: **Spec-Kit plans, Forge governs.**

**Next:** Open GitHub Copilot Chat and type `/speckit.specify` to begin!

---

**End of Tutorial**
