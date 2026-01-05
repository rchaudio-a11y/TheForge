# Forge System Improvement Suggestions - Low Priority

**Document Type:** Reference Documentation  
**Purpose:** Enhancement suggestions for TheForge governance system  
**Created:** 2025-01-04  
**Last Updated:** 2025-01-04  
**Status:** Active  
**Character Count:** 3465
**Related:** Forge-Improvement-Suggestions-Overview.md, ForgeCharter.md

---

## ?? LOW Priority Suggestions

### 7. Document the Spec-Kit File Move

**Issue:** Files were moved but migration isn't documented.

**Current State:**
- Original Spec-Kit files: `.github/agents/`, `.github/prompts/`
- Copied to: `TheForge/Prompts/SpecKit/agents/`, `TheForge/Prompts/SpecKit/prompts/`
- Both exist but only `.github/` is used by Copilot
- No documentation explains why both exist

**Recommended Solution:**

Create: `TheForge/Prompts/SpecKit/MIGRATION.md`

Should document:
- Why two locations exist
- Which location is active vs. reference
- How to keep files in sync
- File ownership rules

**Action Required:**
1. Create MIGRATION.md
2. Document sync process
3. Add sync script (optional)
4. Update SPECKIT_SETUP.md to reference migration doc

**Estimated Time:** 25 minutes

---

### 8. Add a Governance Compliance Checklist

**Issue:** No quick reference for pre-commit validation.

**Recommended Solution:**

Create: `TheForge/Prompts/COMPLIANCE-CHECKLIST.md`

Should include checklists for:
- Metadata Headers (ForgeCharter Section 9)
- File Handling (ForgeCharter Section 8)
- Naming (ForgeCharter Section 5.3)
- Branch Rules
- Designer Files (ForgeCharter Section 4)
- Audit requirements
- Git requirements
- Spec-Kit workflow (if applicable)

Plus:
- Quick audit commands
- Common violations to avoid
- Emergency rollback procedures

**Action Required:**
1. Create COMPLIANCE-CHECKLIST.md
2. Reference in ForgeAudit.md
3. Include in onboarding documentation
4. Print and post near workstation (optional)

**Estimated Time:** 20 minutes

---

### 9. Clarify .specify/ vs TheForge/Prompts/SpecKit/features/ Structure

**Issue:** Documentation references two different feature directories.

**Current State:**
- `tasks.md` exists in `.specify/features/add-readme-files/`
- Documentation references `TheForge/Prompts/SpecKit/features/`
- Unclear which is correct or if both should exist

**Recommended Solution:**

**Decision:** Keep `.specify/` as primary working directory

**Reasoning:**
- Spec-Kit CLI expects `.specify/` by default
- Hidden directories reduce clutter in Solution Explorer
- Matches Spec-Kit convention

**Documentation Updates:**

Update all references in:
- `Unified-Workflow-Guide.md`
- `File-Structure-Guide.md`
- `SPECKIT_SETUP.md`

Change from: `TheForge/Prompts/SpecKit/features/`  
To: `.specify/features/`

**Action Required:**
1. Verify `.specify/features/` is the working directory
2. Update all documentation references
3. Remove `TheForge/Prompts/SpecKit/features/` if empty
4. Document decision in ForgeConfig.md

**Estimated Time:** 15 minutes

---

### 10. Create a Quick Start Guide

**Issue:** No 5-minute getting-started guide for new users.

**Recommended Solution:**

Create: `TheForge/Documentation/QUICKSTART.md`

Should include:
- 6-step feature workflow (1 min each)
- Key files to know
- Common commands
- Forge metadata header template
- Golden rules
- Help resources
- Troubleshooting section

**Action Required:**
1. Create QUICKSTART.md
2. Link from README.md
3. Add to onboarding materials
4. Test with new team member

**Estimated Time:** 30 minutes

---

**Character Count:** TBD

---

**End of Low Priority Suggestions**
