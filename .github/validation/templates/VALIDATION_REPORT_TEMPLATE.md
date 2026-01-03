# Forge Governance Validation Report

**Generated:** [DateTime]  
**Script Version:** [Version]  
**Validation Duration:** [Seconds] seconds

---

## Executive Summary

**Overall Status:** [? Pass | ?? Warning | ? Fail]

- **Consistency Check:** [Pass | Warning | Fail] - [Count] conflicts found
- **Conflict Detection:** [Pass | Warning | Fail] - [Count] conflicts found
- **Token Usage:** [Count] characters (~[Tokens] tokens) - [Status]
- **Redundancy:** [Percentage]% - [Status]
- **Cross-References:** [Valid]/[Total] valid - [Status]
- **Character Counts:** [Accurate]/[Total] accurate - [Status]

**Priority Action Items:** [Count]

---

## Detailed Findings

### 1. Consistency Check

**Status:** [Pass | Warning | Fail]  
**Conflicts Found:** [Count]

[If Pass:]
? No contradictory rules detected across governance files.

[If Warning/Fail:]
?? The following conflicts were detected:

#### Conflict: [Topic Name]
- **File 1:** [FileName] § [Section]
  - Rule: [Rule text]
- **File 2:** [FileName] § [Section]
  - Rule: [Rule text]
- **Severity:** [Low | Medium | High | Critical]
- **Recommendation:** [Specific action to resolve]

---

### 2. Conflict Detection (Spec-Kit ? Forge)

**Status:** [Pass | Warning | Fail]  
**Integration Points Checked:** [Count]

[If Pass:]
? All Spec-Kit and Forge integration points are compatible.

[If Warning/Fail:]
?? The following integration conflicts were detected:

#### Integration Point: [Spec-Kit Command] ? [Forge Component]
- **Status:** [? Compatible | ?? Warning | ? Incompatible]
- **Issue:** [Description of conflict]
- **Impact:** [How this affects usage]
- **Recommendation:** [Specific action]

**Integration Points Summary:**

| Integration Point | Status | Notes |
|-------------------|--------|-------|
| `/specify` ? Constitution | [Status] | [Notes] |
| `/plan` ? Branch-Architecture | [Status] | [Notes] |
| `/implement` ? Branch-Coding | [Status] | [Notes] |
| `/implement` ? Branch-Documentation | [Status] | [Notes] |

---

### 3. Token Usage Analysis

**Status:** [? Optimal | ?? Warning | ? Critical]

**Total Character Count:** [Count] characters  
**Estimated Token Count:** ~[Count] tokens (chars ÷ 4)

**Per-File Breakdown:**

| File | Characters | Est. Tokens | % of Total |
|------|-----------|-------------|------------|
| Constitution | [Count] | [Count] | [%] |
| ForgeCharter | [Count] | [Count] | [%] |
| Branch-Architecture | [Count] | [Count] | [%] |
| Branch-Coding | [Count] | [Count] | [%] |
| Branch-Documentation | [Count] | [Count] | [%] |
| ForgeAudit | [Count] | [Count] | [%] |
| Router | [Count] | [Count] | [%] |
| Spec-Kit Agents | [Count] | [Count] | [%] |
| Spec-Kit Prompts | [Count] | [Count] | [%] |
| **TOTAL** | **[Count]** | **[Count]** | **100%** |

**Thresholds:**
- ?? Warning: 50,000 characters (~12,500 tokens)
- ? Critical: 75,000 characters (~18,750 tokens)

**Assessment:**
[Current status message and recommendations]

---

### 4. Redundancy Scan

**Status:** [? Optimal | ?? Warning | ? Critical]

**Redundancy Summary:**
- **Total Rules:** [Count]
- **Unique Rules:** [Count]
- **Duplicate Rules:** [Count]
- **Redundancy Percentage:** [Percentage]%

**Thresholds:**
- ?? Warning: 15% redundancy
- ? Critical: 30% redundancy

**High Redundancy Areas:**

[If redundancy detected:]
1. **[Topic]** - [Count] duplicates
   - Files: [File1, File2, File3]
   - Example: "[Rule text preview]"
   - Recommendation: [Action]

2. **[Topic]** - [Count] duplicates
   - Files: [File1, File2]
   - Example: "[Rule text preview]"
   - Recommendation: [Action]

[If no significant redundancy:]
? No significant rule redundancy detected.

---

### 5. Cross-Reference Validation

**Status:** [? All Valid | ?? Some Broken | ? Many Broken]

**Cross-Reference Summary:**
- **Total References:** [Count]
- **Valid References:** [Count]
- **Broken References:** [Count]
- **Accuracy:** [Percentage]%

**Broken References:**

[If broken references found:]
1. **[SourceFile] § [Section]** ? **[TargetFile] § [TargetSection]**
   - **Issue:** [Target not found | Target moved | Section missing]
   - **Line:** ~[LineNumber]
   - **Recommendation:** [Update reference to new location | Remove reference | Verify target]

[If all valid:]
? All cross-references are valid.

---

### 6. Character Count Verification

**Status:** [? All Accurate | ?? Some Inaccurate | ? Many Inaccurate]

**Character Count Summary:**
- **Total Files:** [Count]
- **Accurate:** [Count]
- **Inaccurate:** [Count]
- **Not Set (TBD):** [Count]
- **Auto-Fixed:** [Count] (if -AutoFix enabled)

**Files with Inaccurate Counts:**

[If inaccuracies found:]

| File | Declared | Actual | Difference | Status |
|------|----------|--------|------------|--------|
| [File] | [Count] | [Count] | [+/-Count] | [Fixed/Not Fixed] |

[If all accurate:]
? All character counts are accurate.

---

## Efficiency Analysis

### Token Usage Assessment

**Current Load:** [Count] characters (~[Count] tokens)

**Evaluation:**
- **Status:** [? Efficient | ?? Moderate | ? High Overhead]
- **Per-Conversation Load:** [Description of typical load]
- **Optimization Opportunity:** [None | Low | Medium | High]

**Recommendations:**
[If optimization needed:]
1. [Specific recommendation]
2. [Specific recommendation]

[If optimal:]
? Current token usage is within optimal range.

---

### Overhead Assessment

**Complexity Metrics:**
- **Governance File Count:** [Count]
- **Total Character Count:** [Count]
- **Cross-Reference Count:** [Count]
- **Average File Size:** [Count] characters

**Value Assessment:**
- **Problems Prevented:** [Analysis of governance value]
- **Maintenance Cost:** [Low | Medium | High]
- **Cost-Benefit Ratio:** [Favorable | Acceptable | Concerning]

**Overall Recommendation:**
[Summary of whether current governance overhead is justified]

---

## Action Items

### Critical (Fix Now) ??
[If critical issues found:]
- [ ] [Action item with specific file and section]
- [ ] [Action item]

[If none:]
? No critical issues requiring immediate action.

---

### High Priority (Fix Soon) ??
[If high priority issues found:]
- [ ] [Action item]
- [ ] [Action item]

[If none:]
? No high-priority issues.

---

### Medium Priority (Backlog) ??
[If medium priority issues found:]
- [ ] [Action item]
- [ ] [Action item]

[If none:]
? No medium-priority issues.

---

### Low Priority (Optional) ?
[If low priority improvements identified:]
- [ ] [Action item]
- [ ] [Action item]

[If none:]
? No low-priority improvements identified.

---

## Recommendations

### Immediate Actions
[Numbered list of recommended immediate actions based on findings]

### Long-Term Improvements
[Numbered list of strategic improvements for governance system]

### Optimization Opportunities
[Specific opportunities to reduce overhead while maintaining value]

---

## Next Steps

1. **Review this report** - Assess findings and prioritize actions
2. **Address critical issues** - Fix any critical conflicts or errors
3. **Create revision documents** - Document fixes for future reference (use `FORGE_REVISION_TEMPLATE.md`)
4. **Re-run validation** - Confirm issues are resolved
5. **Schedule follow-up** - Plan next validation run

---

## Appendix: Validation Configuration

**Configuration File:** [Path]  
**Validation Checks Enabled:**
- Consistency Check: [Enabled/Disabled]
- Conflict Detection: [Enabled/Disabled]
- Token Usage Analysis: [Enabled/Disabled]
- Redundancy Scan: [Enabled/Disabled]
- Cross-Reference Validation: [Enabled/Disabled]
- Character Count Verification: [Enabled/Disabled]

**Auto-Fix Enabled:** [Yes/No]  
**Auto-Fix Applied To:** [List of what was auto-fixed]

---

## Report Metadata

**Report File:** [Filename]  
**Generated By:** Forge Governance Validation Framework  
**Script Version:** [Version]  
**Execution Time:** [Seconds] seconds  
**Report Character Count:** [Count]

---

**End of Validation Report**

---

## Usage Notes

This template is used by `Validate-ForgeSystem.ps1` to generate validation reports.

**Placeholders:**
- `[DateTime]` - ISO format timestamp
- `[Version]` - Script version number
- `[Seconds]` - Execution time
- `[Count]` - Numeric value
- `[Percentage]` - Percentage value
- `[Status]` - Pass/Warning/Fail
- `[FileName]` - File name or path
- `[Section]` - Section number or heading
- `[Topic]` - Topic or category name

**Status Icons:**
- ? - Pass/Success
- ?? - Warning/Attention needed
- ? - Fail/Critical issue
- ?? - Critical priority
- ?? - High priority
- ?? - Medium priority
- ? - Low priority
