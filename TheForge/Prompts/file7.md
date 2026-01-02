# file7.md — Documentation & Large-Output Extensions

This file extends the Forge rule stack for documentation and large text generation. It does not restate taxonomy or high-level documentation rules defined in Master.md or file1–file6; it only adds concrete behavioral constraints for Copilot.

----------------------------------------------------------------------
0. Rule precedence (file7.md scope)
----------------------------------------------------------------------

This file operates at the lowest precedence tier in the Forge rule stack:

Precedence order (highest to lowest):
1. Master.md — Canonical governance index
2. ForgeOrchestrator.md — Orchestration and execution rules
3. File6.md — Prime Directives (architectural principles)
4. file1–file5 — Core domain rules (Scriptorium, Layout, Taxonomy, Config)
5. file7.md — Documentation extensions (this file)
6. file8.md — Code implementation clarifications

Conflict resolution:
• If file7.md contradicts file1–file6: file1–file6 wins
• If file7.md contradicts File6.md Prime Directives: File6.md wins
• file7.md adds *behavioral constraints* for documentation, not new doc types
• At same precedence level: more specific beats more general

When in doubt: Consult file1.md (Scriptorium) and file4.md (Taxonomy) first.

----------------------------------------------------------------------
1. MANDATORY PRE-GENERATION CHECKS ⚠️
----------------------------------------------------------------------

Before calling create_file or providing final content to user, Copilot MUST complete this checklist:

### Pre-Generation Checklist

- [ ] **Estimate character count**
  - Rough calculation: ~80 chars/line × total line count
  - Or: count actual characters if content is ready
  
- [ ] **Check against 10k limit**
  - If <8,000 chars: ✅ Proceed with single file
  - If 8,000-9,999 chars: ⚠️ Warn user, consider splitting
  - If ≥10,000 chars: 🛑 STOP - Plan multi-file split NOW
  
- [ ] **For multi-file splits:**
  - Ask user for naming pattern before generating
  - Do NOT assume naming convention
  - Plan split points at section boundaries
  
- [ ] **Add character count to header**
  - Every generated file MUST include character count in metadata
  - If exceeding 10k: MUST include justification
  - If exceeding 10k: MUST include split recommendation

### Enforcement

• Copilot MUST NOT proceed with file generation if checklist incomplete
• Copilot MUST warn user if content approaches or exceeds limits
• Copilot MUST ask for explicit approval before creating 10k+ files

----------------------------------------------------------------------
2. File size behavior for generated text (STRICT ENFORCEMENT)
----------------------------------------------------------------------

These rules apply whenever Copilot generates or modifies documentation, reports, or other text files.

### Hard limit: 10,000 characters per file (STRICT)

• Copilot MUST NOT generate files exceeding 10,000 characters without explicit user approval

• Copilot MUST check character count before calling create_file

• Enforcement thresholds:
  - <8,000 chars: Safe - proceed normally
  - 8,000-9,999 chars: Warning zone - alert user, propose split
  - ≥10,000 chars: STOP - must split or get explicit approval

### If expected output would exceed 10,000 characters:

• Copilot MUST:
  - Stop generation immediately
  - Calculate how many parts are needed
  - Ask user for naming pattern (do NOT assume)
  - Propose split points at section/heading boundaries
  - Generate files only after user approval

• Copilot MUST NEVER:
  - Truncate content to "fit" the limit
  - Quietly omit sections without telling the user
  - Generate 10k+ files without warning
  - Assume a naming convention without asking

### Exceptions (require explicit user approval):

The following file types MAY exceed 10k with documented justification:

1. **Technical reference summaries** (e.g., file8-Summary.md, DocumentationRules.md)
   - MUST include character count in header
   - MUST include justification for exception
   - MUST include recommendation for future splitting

2. **Generated code files** (if language/framework requires single file)
   - MUST document reason for single-file requirement
   - MUST include character count in header

3. **Complete API documentation** (if splitting would break usability)
   - MUST include character count in header
   - MUST include navigation links if multi-file would be better

### Exception documentation format:

```markdown
# Document Title

**Document Type:** Technical Reference  
**Created:** YYYY-MM-DD  
**Character Count:** 20,732  
**Status:** EXCEEDS 10k limit (approved as consolidated reference)  
**Justification:** Comprehensive reference document for file8.md rules  
**Split Recommendation:** Consider splitting into 3 parts at next major revision  
**Related:** file8.md (Prompts/file8.md)
```

----------------------------------------------------------------------
3. File header requirements
----------------------------------------------------------------------

ALL generated documentation files MUST include this metadata header:

```markdown
# Document Title

**Document Type:** {Codex|Chronicle|Tome|Lore|Grimoire|Scriptorium|Technical}  
**Created:** YYYY-MM-DD  
**Last Updated:** YYYY-MM-DD (if applicable)  
**Character Count:** {actual count}  
**Status:** {Draft|Review|Final|Deprecated}  
**Related:** {Links to related documents}
```

### Character count field (MANDATORY):

• MUST be present in every generated file
• MUST show actual character count (not estimated)
• If >10,000: MUST add justification and split recommendation
• Format: `**Character Count:** 8,432` or `**Character Count:** 12,045 (EXCEEDS 10k - see justification)`

### Benefits:

- Makes file size violations immediately visible
- Forces Copilot to count before finalizing
- Provides audit trail for exceptions
- Helps users identify candidates for splitting

----------------------------------------------------------------------
4. Multi-file generation behavior
----------------------------------------------------------------------

When splitting documentation across multiple files:

### Preserve structure:
  - Do not break in the middle of a sentence or list.
  - Prefer breaking at heading or section boundaries.

### Maintain continuity:
  - Ensure headings, numbering, and cross-references make sense across parts.
  - Clearly indicate when content "continues in the next file" if the user asks for that style.

### Defer naming decisions:
  - If the repository or user specifies a naming pattern, follow it.
  - If not, ask rather than inventing a pattern that might conflict with existing conventions.

### Multi-file naming patterns (examples):

```
# Sequential numbering
Document-Part1.md
Document-Part2.md
Document-Part3.md

# Descriptive sections
ModuleSystem-Overview.md
ModuleSystem-Implementation.md
ModuleSystem-API.md

# Dated parts (for chronicles)
ReleaseNotes-2025-01-v1.md
ReleaseNotes-2025-01-v2.md
```

### Cross-file navigation:

Each part should include navigation metadata:

```markdown
# Document Title - Part 2 of 3

**This Document:** Implementation Details  
**Previous:** [Part 1: Overview](Document-Part1.md)  
**Next:** [Part 3: API Reference](Document-Part3.md)

## Table of Contents (All Parts)
- [Part 1: Overview](Document-Part1.md)
- [Part 2: Implementation Details](Document-Part2.md) ← You are here
- [Part 3: API Reference](Document-Part3.md)
```

----------------------------------------------------------------------
5. Milestone documentation behavior
----------------------------------------------------------------------

These extensions assume the existence of Chronicle and related docs as described in Master.md and file2/file4; they only add behavior for Copilot.

When the user requests milestone documentation (e.g., "create v0.9.2 entry"):

• Copilot should:
  - Create or update the appropriate Chronicle entry for that milestone.
  - Ensure the entry includes the standard milestone fields defined in existing Chronicle patterns.
  - Suggest updates to DevelopmentLog.index.md, IssueSummary.md, or VersionHistory.chronicle.md when new patterns, issues, or rule drift are documented.

• If the milestone doc risks exceeding the file size behavior above:
  - Propose splitting into multiple parts.
  - Explain how the parts should be named and linked.

----------------------------------------------------------------------
6. Documentation-code consistency behavior
----------------------------------------------------------------------

When there is tension between documentation and code:

• Copilot must:
  - Assume code is the current source of truth.
  - Treat outdated documentation as a candidate for revision, not enforcement.
  - Prefer aligning documentation to the behavior and architecture currently expressed in code.

• If documentation appears to describe an older architecture or rule set:
  - Note the discrepancy.
  - Propose updating the documentation to match current implementation.

----------------------------------------------------------------------
7. Compliance and self-enforcement
----------------------------------------------------------------------

### How Copilot should enforce these rules:

1. **Before generating any documentation:**
   - Run pre-generation checklist (Section 1)
   - Estimate or count characters
   - Plan multi-file split if needed

2. **During generation:**
   - Monitor character count
   - Stop at 10k unless exception approved
   - Add character count to header

3. **After generation:**
   - Verify character count in header matches actual
   - Confirm file doesn't exceed limits (or exception documented)
   - Provide user with character count summary

### Self-check questions:

Before calling create_file, ask yourself:

- ✅ Did I check the character count?
- ✅ Is the file under 10k characters?
- ✅ If over 10k: Did I get user approval?
- ✅ Did I add character count to the header?
- ✅ If over 10k: Did I add justification?

If any answer is NO: STOP and complete the missing step.

End of file7.md.