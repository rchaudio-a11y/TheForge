# Forge Revision - [Issue Title]

**Document Type:** Revision Document (Chronicle)  
**Date:** YYYY-MM-DD  
**Type:** [Bug Fix | Enhancement | Refactor | Conflict Resolution]  
**Severity:** [Low | Medium | High | Critical]  
**Status:** [In Progress | Resolved | Monitoring]  
**Character Count:** TBD  
**Related:** [List related files or documents]

---

## Quick Summary

[One-paragraph summary of what went wrong and how it was fixed]

---

## What Went Wrong

### Description
[Detailed description of the issue, conflict, or problem that was discovered]

### Symptoms
- [Observable symptom 1]
- [Observable symptom 2]
- [Observable symptom 3]

### Discovery Method
[How was this issue found? Validation report? Manual review? User report?]

### Impact
**Affected Components:**
- [Component 1]
- [Component 2]

**Severity Justification:**
[Why this severity level was assigned]

---

## Root Cause

### Analysis
[Deep dive into why this happened]

### Contributing Factors
1. **[Factor 1]**
   - Description: [Details]
   - Impact: [How this contributed]

2. **[Factor 2]**
   - Description: [Details]
   - Impact: [How this contributed]

### Timeline (If Applicable)
- **[Date/Time]:** [Event]
- **[Date/Time]:** [Event]
- **[Date/Time]:** [Event]

---

## Resolution

### Solution Implemented
[Detailed description of how the issue was fixed]

### Changes Made

#### Files Modified
1. **[FileName]**
   - **Section:** [Section number/name]
   - **Change:** [Description of change]
   - **Justification:** [Why this change resolves the issue]

2. **[FileName]**
   - **Section:** [Section number/name]
   - **Change:** [Description of change]
   - **Justification:** [Why this change resolves the issue]

#### Files Created (If Applicable)
- **[FileName]:** [Purpose]

#### Files Deleted (If Applicable)
- **[FileName]:** [Reason for deletion]

### Implementation Steps
1. [Step 1]
2. [Step 2]
3. [Step 3]

### Code/Text Diff (If Applicable)
```diff
- Old text or code
+ New text or code
```

---

## Validation

### How Was This Verified?

**Testing Performed:**
1. [Test 1 description and result]
2. [Test 2 description and result]

**Validation Checks:**
- [ ] Re-ran validation script
- [ ] Manual review of affected files
- [ ] Cross-reference checks passed
- [ ] Character counts updated
- [ ] No new conflicts introduced

**Results:**
[Summary of validation results confirming fix]

### Before vs After

| Metric | Before | After | Change |
|--------|--------|-------|--------|
| Character Count | [Count] | [Count] | [+/- Count] |
| Conflicts | [Count] | [Count] | [+/- Count] |
| Redundancy | [%] | [%] | [+/- %] |
| Cross-References | [Count] | [Count] | [+/- Count] |

---

## Lessons Learned

### What Went Well
1. [Positive outcome 1]
2. [Positive outcome 2]

### What Could Be Improved
1. [Improvement area 1]
2. [Improvement area 2]

### Prevention Strategies
**To prevent this issue in the future:**

1. **[Strategy 1]**
   - Action: [Specific action to take]
   - Owner: [Who is responsible]
   - Timeline: [When to implement]

2. **[Strategy 2]**
   - Action: [Specific action to take]
   - Owner: [Who is responsible]
   - Timeline: [When to implement]

### Process Improvements
[Any improvements to validation, governance, or development processes]

---

## Related Files

### Governance Files Affected
- [File 1] - [Brief description of impact]
- [File 2] - [Brief description of impact]

### Documentation Updated
- [Doc 1] - [What was updated]
- [Doc 2] - [What was updated]

### Cross-References Updated
[List of cross-references that were added, modified, or removed]

---

## Character Count Impact

### Overall Impact
- **Before Fix:** [Total character count across affected files]
- **After Fix:** [Total character count across affected files]
- **Net Change:** [+/- Count] characters ([+/- %]%)

### Per-File Impact

| File | Before | After | Change | % Change |
|------|--------|-------|--------|----------|
| [File 1] | [Count] | [Count] | [+/- Count] | [+/- %] |
| [File 2] | [Count] | [Count] | [+/- Count] | [+/- %] |
| **TOTAL** | **[Count]** | **[Count]** | **[+/- Count]** | **[+/- %]** |

### Token Usage Impact
- **Before:** ~[Count] tokens
- **After:** ~[Count] tokens
- **Change:** [+/- Count] tokens ([+/- %]%)

---

## Follow-Up Actions

### Immediate
- [ ] [Action 1]
- [ ] [Action 2]

### Short-Term (Within 1 week)
- [ ] [Action 1]
- [ ] [Action 2]

### Long-Term (Within 1 month)
- [ ] [Action 1]
- [ ] [Action 2]

### Monitoring Plan
[Describe how to monitor for recurrence or related issues]

---

## References

### Validation Reports
- [Report filename] - [Date] - [Key findings]

### Related Revisions
- [Revision filename] - [Date] - [Relation to this revision]

### External References
- [Link or reference to external resource]
- [Link or reference to external resource]

---

## Approval & Sign-Off

**Reviewed By:** [Name/Role]  
**Approved By:** [Name/Role]  
**Approval Date:** YYYY-MM-DD

**Status Change History:**
- **YYYY-MM-DD:** In Progress
- **YYYY-MM-DD:** Resolved
- **YYYY-MM-DD:** Monitoring (if applicable)
- **YYYY-MM-DD:** Closed (if applicable)

---

## Appendix: Examples

### Example 1: Bug Fix Revision
```
# Forge Revision - Inconsistent Designer File Rules

**Date:** 2026-01-03
**Type:** Bug Fix
**Severity:** Medium
**Status:** Resolved

## What Went Wrong
Constitution stated "AI never edits Designer files" but Branch-Coding allowed it under certain conditions.

## Resolution
Updated Branch-Coding § 4.2 to explicitly state "Designer files are read-only for AI."

## Character Count Impact
- Branch-Coding: 10,500 ? 10,587 (+87 chars)
```

### Example 2: Conflict Resolution
```
# Forge Revision - Spec-Kit /implement Conflicts with Forge Rules

**Date:** 2026-01-03
**Type:** Conflict Resolution
**Severity:** High
**Status:** Resolved

## What Went Wrong
Spec-Kit's /implement agent could generate code that violated Forge Branch-Coding rules.

## Resolution
Added explicit reference to Branch-Coding rules in Spec-Kit implement.prompt.md

## Character Count Impact
- implement.prompt.md: 3,200 ? 3,450 (+250 chars)
```

---

## Template Usage Notes

### When to Create a Revision Document

**Always create a revision for:**
- Critical or high-severity issues
- Conflicts between governance systems
- Architectural changes to governance
- Issues affecting multiple files

**Consider creating a revision for:**
- Medium-severity issues
- Recurring patterns of similar issues
- Complex fixes requiring detailed explanation

**Usually skip revision for:**
- Typo fixes
- Character count corrections
- Minor formatting changes

### File Naming Convention
`FORGE_REVISION_YYYYMMDD_[SHORT-DESCRIPTION].md`

**Examples:**
- `FORGE_REVISION_20260103_DESIGNER-RULES-CONFLICT.md`
- `FORGE_REVISION_20260103_REDUNDANCY-REMOVAL.md`
- `FORGE_REVISION_20260103_SPECKIT-INTEGRATION.md`

### Storage Location
- **Forge-specific issues:** `.github/revisions/forge/`
- **Spec-Kit-specific issues:** `.github/revisions/speckit/`
- **Cross-cutting issues:** Choose the primary system affected

---

**End of Revision Template**
