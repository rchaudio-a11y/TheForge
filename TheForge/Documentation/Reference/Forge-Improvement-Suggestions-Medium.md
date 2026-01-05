# Forge System Improvement Suggestions - Medium Priority

**Document Type:** Reference Documentation  
**Purpose:** Important system improvements for TheForge governance system  
**Created:** 2025-01-04  
**Last Updated:** 2025-01-04  
**Status:** Active  
**Character Count:** 6165  
**Related:** Forge-Improvement-Suggestions-Overview.md, ForgeCharter.md

---

## ?? MEDIUM Priority Suggestions

### 3. Create Missing constitution.md

**Issue:** Workflow references a file that doesn't exist yet.

**Current State:**
- `Unified-Workflow-Guide.md` (Section 5.1) references `TheForge/Prompts/SpecKit/memory/constitution.md`
- File does not exist (Step 1 of workflow hasn't been executed)
- Spec-Kit agents expect this file to exist before running `/speckit.specify`

**Problem:**
- Workflow is blocked until constitution.md exists
- Unclear whether this should be global or per-feature
- Tutorial assumes it exists but doesn't explain how to create it

**Recommended Solution:**

**Option A: Create Global Constitution** ? **Recommended**

Create: `TheForge/Prompts/SpecKit/memory/constitution.md`

```markdown
# TheForge Project Constitution

**Document Type:** Constitution  
**Purpose:** Project-wide principles and constraints for all features  
**Created:** 2025-01-04  
**Last Updated:** 2025-01-04  
**Status:** Active  
**Character Count:** TBD  
**Related:** ForgeCharter.md, Branch-*.md

---

## Project Principles

### 1. Forge Governance Supreme Authority
All features, code, and documentation must comply with ForgeCharter.md and Branch-* rules.

### 2. Deterministic and Auditable
All work must be:
- Deterministic (reproducible)
- Auditable (traceable)
- Compliant (100% ForgeAudit pass)

### 3. Metadata Headers Mandatory
All files must include Forge metadata headers per ForgeCharter Section 9.

### 4. Explicit Intent Required
No implicit file creation, modification, or deletion per ForgeCharter Section 8.

### 5. Branch Independence
Respect branch boundaries and rule precedence hierarchy.

---

## Project Constraints

### Technical Constraints
- **Language:** VB.NET (primary), C# (secondary)
- **Framework:** .NET 6+
- **IDE:** Visual Studio 2022+
- **Version Control:** Git

### Architectural Constraints
- **Modularity:** Follow Branch-Architecture.md rules
- **Naming:** No forbidden terms (Helper, Manager, Utils, etc.)
- **Designer Files:** Follow ForgeCharter Section 4 governance

### Documentation Constraints
- **Format:** Markdown (.md)
- **Character Limit:** 10,000 characters per file
- **Taxonomy:** Follow Branch-Documentation.md taxonomy
- **Headers:** Forge metadata headers mandatory

---

## Feature-Specific Considerations

When starting a new feature:
1. Review this constitution
2. Add feature-specific constraints if needed
3. Reference ForgeCharter for rule precedence
4. Include Forge compliance checkpoints in tasks

---

## Forge Vocabulary

Prefer Forge-themed terminology:
- ? Forge, Charter, Branch, Audit, Module
- ? Helper, Manager, Utility, Factory (unless justified)

---

**End of Constitution**
```

**Option B: Per-Feature Constitution**
- Create constitution.md for each feature in `.specify/features/[feature-name]/`
- Allows feature-specific constraints
- More flexible but requires more setup

**Action Required:**
1. Choose global or per-feature approach
2. Create constitution.md with project principles
3. Reference in all Spec-Kit workflows
4. Update tutorial to mention constitution creation

**Estimated Time:** 30 minutes

---

### 4. Add Version Tracking to Branch Files

**Issue:** Branch files lack version tracking unlike ForgeCharter.

**Current State:**
- ForgeCharter.md has version tracking (v1.1) and amendment block
- Branch-Architecture.md, Branch-Coding.md, Branch-Documentation.md have no versions
- No way to track when rules change
- No amendment history

**Recommended Solution:**

Add to each Branch-*.md file header:

```markdown
# Branch-[Name]

**Document Type:** Governance Branch  
**Version:** v1.0  
**Purpose:** [existing purpose]  
**Created:** [date]  
**Last Updated:** [date]  
**Status:** Active  
**Character Count:** TBD  
**Related:** ForgeCharter.md
```

Add amendment section at end:

```markdown
---

## Version History

### v1.0 - [Date]
- Initial version
- Established [branch-specific] governance rules

---

**End of Branch-[Name]**
```

**Action Required:**
1. Update Branch-Architecture.md with version v1.0
2. Update Branch-Coding.md with version v1.0
3. Update Branch-Documentation.md with version v1.0
4. Add amendment blocks to each
5. Update Character Counts

**Estimated Time:** 20 minutes (all three files)

---

### 5. Create a Forge Configuration File

**Issue:** System configuration is scattered across multiple files.

**Current State:**
- File location decisions are implicit
- Version compatibility is undocumented
- System defaults are inferred from ForgeCharter
- No single source of truth for configuration

**Recommended Solution:**

Create: `TheForge/Prompts/ForgeConfig.md`

Content should include:
- File Locations (Canonical)
- Version Compatibility Matrix
- System Defaults
- Integration Points
- Default Workflows
- Environment Variables (Optional)
- Configuration Updates
- Version History

**Action Required:**
1. Create ForgeConfig.md
2. Document all file location decisions
3. Reference in copilot-instructions.md
4. Update documentation to reference ForgeConfig

**Estimated Time:** 45 minutes

---

### 6. Improve copilot-instructions.md Routing Logic

**Issue:** Current routing is simplistic and doesn't handle edge cases.

**Problem:**
- Doesn't handle tasks that span multiple domains
- No priority rules for overlapping concerns
- Designer files need special handling (ForgeCharter Section 4)
- Metadata headers need special handling (ForgeCharter Section 9)

**Recommended Solution:**

Update `.github/copilot-instructions.md` with priority routing:

### Priority 1: ForgeCharter Sections (Highest)
If task involves:
- Designer files ? ForgeCharter Section 4
- Metadata headers ? ForgeCharter Section 9
- File operations ? ForgeCharter Section 8
- Amendments ? ForgeCharter Section 11

### Priority 2: Multi-Domain Tasks
If task involves multiple domains:
- Code AND documentation ? Load both Branch files
- Structure AND code ? Load both Branch files
- All three ? Load all three branches

### Priority 3: Single-Domain Tasks
- Code only ? Branch-Coding.md
- Documentation only ? Branch-Documentation.md
- Structure only ? Branch-Architecture.md
- Audit only ? ForgeAudit.md

### Priority 4: Spec-Kit Workflow
- `/speckit.*` commands ? Load appropriate agent

**Action Required:**
1. Update `.github/copilot-instructions.md` with priority rules
2. Test with multi-domain tasks
3. Document edge cases
4. Update Character Count

**Estimated Time:** 30 minutes

---

**Character Count:** TBD

---

**End of Medium Priority Suggestions**
