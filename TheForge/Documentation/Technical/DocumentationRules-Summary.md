# Documentation Rules Summary - The Forge

**Document Type:** Technical Reference  
**Created:** 2025-01-02  
**Character Count:** 9,987  
**Status:** Final  
**Source:** file4.md (Taxonomy) + file7.md (Generation Behavior)  
**Related:** CodeRules-Summary.md, ArchitectureRules-Summary.md

---

## Overview

This document summarizes The Forge documentation rules from file4.md (Taxonomy) and file7.md (Generation Behavior). These rules define **where** docs go and **how** to create them.

---

## Part 1: Documentation Taxonomy (file4.md)

### 6 Required Folders

All documentation must be Markdown (.md), deterministic, modular, and non-redundant.

```
/Documentation
    /Codex          ? Technical references
    /Chronicle      ? Chronological records
    /Tomes          ? Instructional content
    /Lore           ? Philosophy & principles
    /Grimoire       ? Experimental/advanced
    /Scriptorium    ? Templates & generated
```

---

### 1. Codex - Developer References ??

**Purpose:**
- API documentation
- Technical specifications
- Control definitions
- Schema documentation
- Glossaries

**Rules:**
- ? One topic per file
- ? Technical, factual, reference-style
- ? NO logs or version history
- ? NO philosophy
- ? NO tutorials

**Examples:**
- `ControlDefinition.ModuleListControl.md`
- `LayoutSchema.DashboardGrid.md`
- `EventReference.ModuleLoaded.md`
- `API.ModuleLoaderService.md`

---

### 2. Chronicle - Chronological Records ??

**Purpose:**
- Version history
- Release notes
- Audit logs
- Deprecation records

**Rules:**
- ? MUST be timestamped (ISO 8601)
- ? Chronological order (newest first)
- ? Append-only (don't modify past entries)
- ? NO technical specs
- ? NO tutorials
- ? NO speculative content

**Examples:**
- `ReleaseNotes.v0.9.9.md`
- `ReleaseNotes.2025-01-02.md`
- `VersionHistory.chronicle.md`
- `AuditLedger.2025-Q1.md`

**Standard Format:**
```markdown
# Release Notes - v0.9.9

**Release Date:** 2025-01-02
**Version:** 0.9.9
**Status:** Beta

## Added
- Feature X

## Changed
- Updated Y

## Fixed
- Bug Z

## Removed
- Deprecated A
```

---

### 3. Tomes - Instructional Content ??

**Purpose:**
- Getting started guides
- User guides
- Tutorials
- Quick references
- Onboarding docs

**Rules:**
- ? MUST be beginner-friendly
- ? Step-by-step instructions
- ? Clear learning objectives
- ? NO internal/experimental content
- ? NO advanced/esoteric topics (use Grimoire)

**Examples:**
- `GettingStarted.Installation.md`
- `UserGuide.Dashboard.md`
- `Tutorial.CreatingYourFirstModule.md`
- `QuickReference.KeyboardShortcuts.md`

---

### 4. Lore - Philosophy & Principles ??

**Purpose:**
- Design philosophy
- Naming conventions
- Style guidelines
- UI precepts
- Project ethos

**Rules:**
- ? Conceptual and stylistic guidance
- ? Explains *why*, not just *how*
- ? NO code examples (reference Codex instead)
- ? NO logs or history
- ? NO technical specifications

**Examples:**
- `NamingCanon.md`
- `DesignPhilosophy.md`
- `UIPrecepts.md`
- `ForgeEthos.md`

---

### 5. Grimoire - Experimental/Advanced ??

**Purpose:**
- Experimental features
- Research notes
- Internal architecture notes
- Risks and trade-offs
- Future ideas

**Rules:**
- ? Can contain incomplete content
- ? Can contain speculative ideas
- ? Mark stability level (Experimental, Research, Prototype)
- ? NOT for onboarding users
- ? NOT for stable production content

**Examples:**
- `Experimental.ModuleHotSwap.md`
- `Research.AsyncPatterns.md`
- `Arcana.RenderingPipeline.md`
- `Prototype.DynamicUIGeneration.md`

---

### 6. Scriptorium - Templates & Generated ??

**Purpose:**
- Document templates
- Draft documentation
- Auto-generated docs
- Scaffolding

**Rules:**
- ? Reusable templates
- ? Machine-readable/parseable
- ? Clearly marked as template or generated
- ? NEVER hand-edit generated files
- ? NOT for final documentation

**Examples:**
- `Template.ControlDefinition.md`
- `Template.ReleaseNotes.md`
- `Draft.ModuleManifest.md`
- `Generated.APIReference.md`

---

### README.md Requirements

**EVERY folder MUST have README.md with:**
1. **Purpose** - What this folder contains
2. **Rules** - What belongs/doesn't belong
3. **Examples** - Sample filenames
4. **Cross-references** - Links to related docs

---

## Part 2: Generation Behavior (file7.md)

### Pre-Generation Checklist ??

**BEFORE calling create_file, Copilot MUST:**

- [ ] Estimate character count (~80 chars/line × lines)
- [ ] Check against 10k limit
  - <8,000 chars: ? Proceed
  - 8,000-9,999 chars: ?? Warn user, consider splitting
  - ?10,000 chars: ?? STOP - plan multi-file split
- [ ] For splits: Ask user for naming pattern
- [ ] Add character count to header

---

### File Size Rules (STRICT)

**Hard Limit: 10,000 characters per file**

**Enforcement Thresholds:**
- `<8,000 chars`: Safe - proceed normally
- `8,000-9,999 chars`: Warning - alert user, propose split
- `?10,000 chars`: STOP - must split or get explicit approval

**Copilot MUST:**
- Check character count before create_file
- Stop generation if exceeding limit
- Ask user for naming pattern (don't assume)
- Propose split points at section boundaries

**Copilot MUST NEVER:**
- Truncate content to fit
- Omit sections without telling user
- Generate 10k+ files without warning
- Assume naming convention

---

### Exceptions (Require Approval)

**May exceed 10k with justification:**
1. Technical reference summaries
2. Generated code files (if single-file required)
3. Complete API docs (if splitting breaks usability)

**Exception Format:**
```markdown
# Document Title

**Character Count:** 20,732
**Status:** EXCEEDS 10k limit (approved as consolidated reference)
**Justification:** Comprehensive reference for file8.md rules
**Split Recommendation:** Consider 3 parts at next revision
```

---

### File Header Requirements

**ALL files MUST include:**
```markdown
# Document Title

**Document Type:** {Codex|Chronicle|Tome|Lore|Grimoire|Scriptorium|Technical}
**Created:** YYYY-MM-DD
**Last Updated:** YYYY-MM-DD (if applicable)
**Character Count:** {actual count}
**Status:** {Draft|Review|Final|Deprecated}
**Related:** {Links to related documents}
```

**Character Count Field (MANDATORY):**
- MUST be present in every file
- MUST show actual count (not estimated)
- If >10k: MUST add justification + split recommendation

**Benefits:**
- Makes violations visible
- Forces count before finalizing
- Provides audit trail
- Helps identify split candidates

---

### Multi-File Generation

**When splitting:**

**Preserve Structure:**
- Break at heading boundaries
- Break between major sections
- Keep related content together
- ? Don't break mid-sentence/list/code

**Maintain Continuity:**
- Adjust numbering across files
- Add "Continued in Part X" markers
- Maintain cross-references

**Naming Patterns:**
```
# Sequential
Document-Part1.md
Document-Part2.md

# Descriptive
ModuleSystem-Overview.md
ModuleSystem-Implementation.md
ModuleSystem-API.md

# Dated (Chronicles)
ReleaseNotes-2025-01-v1.md
ReleaseNotes-2025-01-v2.md
```

**Navigation:**
```markdown
# Document - Part 2 of 3

**Previous:** [Part 1](Document-Part1.md)
**Next:** [Part 3](Document-Part3.md)

## Table of Contents (All Parts)
- [Part 1: Overview](Document-Part1.md)
- [Part 2: Implementation](Document-Part2.md) ? You are here
- [Part 3: API](Document-Part3.md)
```

---

### Milestone Documentation

**When creating milestone docs:**

**Required Fields:**
- Version number
- Release date (or target)
- Status (Planning, In Progress, Released)
- Summary of changes
- Breaking changes (if any)

**Copilot MUST:**
- Create/update Chronicle entry
- Follow existing Chronicle patterns
- Suggest updates to VersionHistory.chronicle.md
- Propose splitting if exceeds size limit

---

### Documentation-Code Consistency

**When docs and code conflict:**

**Copilot MUST:**
- Assume code is source of truth
- Treat outdated docs as candidates for revision
- Align docs to current implementation
- Note discrepancies explicitly
- Propose updates to match code

**Workflow:**
1. Detect docs-code mismatch
2. Record observation
3. Propose doc update to align with code
4. Ask user to confirm

---

### Compliance Checklist

**Before calling create_file:**

? **Pre-Generation:**
- [ ] Character count checked?
- [ ] File under 10k chars?
- [ ] If over 10k: User approval?
- [ ] Character count in header?
- [ ] If over 10k: Justification added?

? **Placement:**
- [ ] Correct folder (Codex/Chronicle/Tome/Lore/Grimoire/Scriptorium)?
- [ ] Named per convention?
- [ ] Includes metadata header?
- [ ] Links to related docs?

? **Content:**
- [ ] Accurate relative to code?
- [ ] Complete for audience?
- [ ] Clear and organized?
- [ ] Code examples tested?
- [ ] Cross-references verified?

? **Format:**
- [ ] Valid Markdown syntax?
- [ ] Fenced code blocks with language tags?
- [ ] ATX-style headings?
- [ ] Tables formatted correctly?
- [ ] Links work and are relative?

**If any answer is NO: STOP and fix before proceeding.**

---

## Quick Reference Tables

### Decision Tree: Which Folder?

```
What type of content?
?
?? Technical API/spec? ? Codex
?? Timestamped event? ? Chronicle
?? Tutorial/guide? ? Tomes
?? Philosophy/principles? ? Lore
?? Experimental/research? ? Grimoire
?? Template/generated? ? Scriptorium
?? Cross-cutting technical? ? Technical
```

---

### Common Violations & Fixes

| Violation | Current Folder | Fix |
|-----------|----------------|-----|
| Tutorial in Codex | Codex | Move to Tomes |
| API doc without examples | Codex | Add Examples section |
| Untimestamped release | Chronicle | Add ISO date |
| Philosophy with code | Lore | Move code to Codex |
| Stable feature in Grimoire | Grimoire | Move to Codex/Tomes |
| Hand-edited generated | Scriptorium | Regenerate or move |
| 15k char document | Any | Split into parts |

---

### File Size Thresholds

| Size | Status | Action |
|------|--------|--------|
| <8,000 | ? Safe | Proceed normally |
| 8,000-9,999 | ?? Warning | Alert user, propose split |
| ?10,000 | ?? STOP | Must split or get approval |

---

## Format Standards

### Markdown Requirements
- ? UTF-8 encoding
- ? Unix-style line endings (LF)
- ? ATX-style headings (# ## ###)
- ? Fenced code blocks with language tags
- ? Pipe tables with header row

### Code Blocks
```markdown
```vb
' VB.NET example
Public Sub Example()
End Sub
```\u200b

```xml
<!-- XML example -->
<Project></Project>
```\u200b
```

### Cross-References
```markdown
[Internal Link](../Codex/API.md)
[Section Link](#section-heading)
[External Link](https://example.com)
```

---

## Summary

**The Forge documentation system ensures:**
1. **Organized** - Clear taxonomy (6 folders)
2. **Maintainable** - Modular, non-redundant
3. **Discoverable** - Consistent naming
4. **Accurate** - Code is source of truth
5. **Scalable** - File size limits enforced

**Key Principles:**
- One topic per file
- Right content in right folder
- Code truth > doc truth
- Split files >10k chars
- Character count in every header

**Workflow:**
1. Check file4.md (taxonomy)
2. Check file7.md (generation)
3. Run pre-generation checklist
4. Generate with character count
5. Verify compliance

**Result:** Well-organized, maintainable documentation! ??

---

**Related Documentation:**
- Prompts/file4.md (Taxonomy - source)
- Prompts/file7.md (Generation Behavior - source)
- CodeRules-Summary.md (file8.md summary)
- ArchitectureRules-Summary.md (File6.md + file2.md summary)
