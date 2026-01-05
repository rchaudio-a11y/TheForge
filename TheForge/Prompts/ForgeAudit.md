# ForgeAudit 2.0
**Document Type:** Codex  
**Purpose:** Define rules for evaluating solutions, detecting drift, and enforcing compliance across all Forge branches  
**Created:** 2026-01-02  
**Last Updated:** 2025-01-02  
**Status:** Final  
**Character Count:** 21847  
**Related:** ForgeCharter.md, Branch-Coding.md, Branch-Architecture.md, Branch-Documentation.md

---

# 1. Purpose
ForgeAudit defines the evaluation and compliance rules used to assess any output generated within the RCH UI Forge ecosystem.  
It ensures that all work aligns with ForgeCharter and the branch system, and that drift is detected early and consistently.

ForgeAudit operates in **two modes**:
1. **Read-Only Mode** (for content) - Detects violations, reports findings, never modifies content
2. **Self-Correcting Mode** (for metadata) - Automatically fixes metadata issues, spelling errors, and updates IssueSummary.md

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

- Modify code logic  
- Restructure projects  
- Override branch rules  
- Change authored content (except spelling)
- Make subjective improvements

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
**Tags:** read-only-content, self-correcting-metadata, evaluation, deterministic, branch-aware

## 4.1 Dual-Mode Operation

### Read-Only Mode (Content)
ForgeAudit **detects and reports** but does NOT modify:
- Code files (`.vb`, `.cs`, etc.)
- Code structure
- File placement
- Folder organization
- Business logic
- Architecture decisions
- Content body text (paragraphs, explanations)

### Self-Correcting Mode (Metadata & Housekeeping)
ForgeAudit **automatically fixes**:
- Character counts (computed values)
- Last Updated dates (timestamps)
- Status fields (when deterministic)
- Missing metadata fields (with defaults)
- Spelling errors in documentation
- IssueSummary.md updates (with audit findings)

**Rationale:**
- Metadata = derived/computed values (not authored content)
- Spelling = objective corrections (not subjective changes)
- IssueSummary.md = living audit log (designed to be updated by audits)
- Character counts = mechanical calculation (not creative work)

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

# 5. Metadata Auto-Fix Rules
**Tags:** metadata, character-count, timestamps, auto-correction

## 5.1 What Gets Auto-Fixed

### Always Auto-Fix:
1. **Character Count** - Update to actual computed value
2. **Last Updated** - Update to current date when file modified
3. **Missing Metadata Fields** - Add with appropriate defaults
4. **Spelling Errors** - Fix in documentation files only
5. **IssueSummary.md** - Append new audit findings

### Never Auto-Fix:
1. **Document Type** - Authored field (requires user decision)
2. **Purpose** - Authored field (requires user decision)
3. **Created Date** - Historical field (immutable)
4. **Related Documents** - Authored field (requires context)
5. **Content Body** - Authored content (read-only)

---

## 5.2 Character Count Rules

**When to Update:**
- Any edit to file content (including metadata)
- Always calculate from first character to last character
- Include metadata header in count
- Include all whitespace and newlines

**How to Calculate:**
```
Character Count = Length of entire file content (including header)
```

**Update Location:**
```markdown
**Character Count:** [actual count]
```

---

## 5.3 Last Updated Rules

**When to Update:**
- Any meaningful change to file content
- Metadata updates (including Character Count changes)

**Format:**
```markdown
**Last Updated:** YYYY-MM-DD
```

**Do NOT update for:**
- Pure read operations (viewing file)
- Audit reports generated externally

---

## 5.4 Missing Metadata Rules

**If metadata header is missing entirely:**
```markdown
**Document Type:** TBD  
**Purpose:** TBD  
**Created:** TBD  
**Last Updated:** [current date]  
**Status:** Draft  
**Character Count:** [calculated]  
**Related:** TBD
```

**If specific fields are missing, add:**
- Character Count: [calculate actual]
- Last Updated: [current date]
- Status: "Draft" (if unknown)

**Never add defaults for:**
- Document Type (requires context)
- Purpose (requires understanding)
- Created (historical, unknown)
- Related (requires analysis)

---

## 5.5 Spelling Error Rules

**Auto-Fix spelling errors in:**
- ✅ Documentation files (`.md`, `.txt`)
- ✅ Metadata headers (in any file)
- ✅ Comments in code (typos in documentation comments)

**Do NOT fix spelling in:**
- ❌ String literals in code (might be intentional)
- ❌ Variable names (breaking change)
- ❌ Method names (breaking change)
- ❌ File names (requires refactoring)

**Common Corrections:**
- "Chracter" → "Character"
- "Documnetation" → "Documentation"
- "Complient" → "Compliant"
- "Deterministc" → "Deterministic"

---

## 5.6 IssueSummary.md Update Rules

**Location:** `Documentation/Chronicle/DevelopmentLog/IssueSummary.md`

**Purpose:** Living document that catalogs all recurring issues across the project.

**When to Update:**
- When audit discovers NEW issue pattern not already documented
- When existing issue pattern has new occurrences
- When resolution patterns evolve

**Update Format:**
```markdown
### [Next Number]. [Issue Category Name]

**Pattern:** [Description of the pattern]

**Symptoms:**
- [Symptom 1]
- [Symptom 2]

**Resolution Pattern:**
- [Resolution step 1]
- [Resolution step 2]

**Frequency:** [Low/Medium/High] (encountered in X milestones)
```

**Update Frequency Analysis Table:**
Add new row or update existing row with milestone occurrence.

**Update Recommendations Section:**
Add new recommendation if pattern suggests preventive measure.

**After Updating:**
- Update Character Count
- Update Last Updated date
- Maintain ordering by frequency (High → Low)

---

# 6. Audit Categories
**Tags:** code-review, architecture-review, documentation-review, compliance-check

## 6.1 Coding Audit
Evaluates (Read-Only):
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

**Reports violations, does NOT modify code.**

---

## 6.2 Architecture Audit
Evaluates (Read-Only):
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

**Reports violations, does NOT restructure.**

---

## 6.3 Documentation Audit
Evaluates (Mixed Mode):

**Auto-Fix:**
- ✅ Character count accuracy
- ✅ Last Updated dates
- ✅ Missing metadata fields
- ✅ Spelling errors
- ✅ IssueSummary.md updates

**Report Only:**
- Taxonomy misalignment
- Content contradictions
- Cross-file duplication
- Naming canon violations (authored names)
- Section ordering issues
- Structural problems

---

## 6.4 File Handling Audit
Evaluates (Read-Only):
- Explicit intent for file operations  
- No implicit file creation  
- No implicit file modification  
- No implicit file deletion  
- No implicit file renaming  
- No implicit folder restructuring  
- Deterministic file output rules  
- Designer file governance compliance  

**Reports violations, does NOT move/create/delete files.**

---

# 7. Drift Detection
**Tags:** drift, validation, compliance-monitoring, deviation-detection

## 7.1 Types of Drift
ForgeAudit must detect:

- Structural drift (folder/file placement)
- Naming drift (naming canon violations)
- Metadata drift (missing/outdated metadata) ← **AUTO-FIX**
- Documentation drift (scattered organization)
- Designer drift (mixed responsibilities)
- Layout drift (control ordering)
- Dependency drift (circular deps)
- Branch rule drift (rule violations)
- Charter rule drift (governance violations)

---

## 7.2 Drift Response

### For Metadata Drift (Auto-Fix):
1. Detect the drift
2. Calculate correction
3. Apply fix automatically
4. Report correction in audit output
5. Update IssueSummary.md if new pattern

### For Content Drift (Report Only):
1. Detect the drift
2. Identify the violated rule(s)
3. Identify the responsible branch
4. Provide a correction plan
5. Request user confirmation before any corrective action
6. Update IssueSummary.md if new pattern

ForgeAudit must never auto-correct content drift.

---

# 8. Audit Output Format

## 8.1 Summary
A high-level overview of findings, including:
- Files analyzed
- Violations found (by category)
- Auto-fixes applied
- Compliance score

## 8.2 Auto-Fixes Applied Section
List all automatic corrections made:
```markdown
## Auto-Fixes Applied

### Metadata Corrections (X files)
1. ✅ IssueSummary.md - Character Count: 17764 → 18664
2. ✅ ForgeCharter.md - Last Updated: 2025-01-01 → 2025-01-02
3. ✅ Branch-Coding.md - Added missing Character Count field

### Spelling Corrections (X files)
1. ✅ README.md - "Chracter" → "Character" (line 42)
2. ✅ ForgeTome.md - "Complient" → "Compliant" (line 156)

### IssueSummary.md Updates
1. ✅ Added Category #18: AI/Terminal Interaction Failures
2. ✅ Updated Frequency Analysis Table
3. ✅ Added Recommendations #21-23
```

## 8.3 Branch Breakdown
Findings grouped by:
- Coding (reported only)
- Architecture (reported only)
- Documentation (auto-fixed + reported)
- File Handling (reported only)
- Governance (auto-fixed + reported)

## 8.4 Violations (Content - Reported Only)
Each violation must include:
- Rule violated  
- Branch source  
- Severity  
- Explanation  
- Recommended fix (for user to apply)

## 8.5 Compliance Score
Calculate based on:
- Total rules evaluated
- Rules passing
- Rules with auto-fixed violations
- Rules with reported violations

```
Compliance Score = (Passing + Auto-Fixed) / Total Rules × 100%
```

## 8.6 Summary Statement
Must clearly state:
```markdown
**Audit Type:** Mixed Mode (Auto-Fix Metadata + Report Content)
**Auto-Fixes Applied:** X metadata corrections, Y spelling fixes
**Content Violations Reported:** Z issues (require user action)
**Files Modified:** [list of files with auto-fixes]
**Files Requiring Manual Fixes:** [list of files with content violations]
```

---

# 9. Special File: IssueSummary.md
**Tags:** issue-tracking, pattern-detection, living-document

## 9.1 Purpose
IssueSummary.md is a **living document** that catalogs all recurring development issues.  
It is designed to be **updated by audits** automatically.

## 9.2 Location
`Documentation/Chronicle/DevelopmentLog/IssueSummary.md`

## 9.3 When to Update
Update IssueSummary.md when:
1. **New Issue Pattern Detected** - Pattern not yet documented
2. **Existing Pattern Recurs** - Update frequency count
3. **Resolution Pattern Evolves** - Update resolution steps
4. **Impact Assessment Changes** - Update impact rating

## 9.4 What NOT to Update
Do NOT update IssueSummary.md for:
- Routine violations (already documented patterns)
- Single-occurrence issues (not patterns yet)
- Subjective observations (must be objective)

## 9.5 Update Process
1. Read current IssueSummary.md
2. Determine if issue is new pattern or existing pattern recurrence
3. If new: Add new category with proper numbering
4. If existing: Update frequency count in that category
5. Update Frequency Analysis Table
6. Update Recommendations if new preventive measure identified
7. Sort categories by frequency (High → Low)
8. Update Character Count
9. Update Last Updated date

## 9.6 Example Audit Finding → IssueSummary Update

**Audit Finding:**
"Detected 3 files with Designer/main separation violation. Controls declared in main .vb files instead of Designer.vb files."

**Action:**
Check if "Designer/main separation" pattern exists in IssueSummary.md.
- If yes: Update frequency count
- If no: Add new category with pattern details

---

# 10. Spelling Error Detection & Correction
**Tags:** spelling, typos, documentation-quality

## 10.1 Scope
Auto-correct spelling errors in:
- Documentation files (`.md`, `.txt`)
- Metadata headers (all files)
- XML documentation comments (code files)

## 10.2 Detection Method
Use standard English dictionary plus technical terms:
- Programming terms: "namespace", "metadata", "deterministic"
- Forge terms: "ForgeCharter", "Codex", "Chronicle", "Lore"
- Domain terms: "module", "interface", "assembly"

## 10.3 Common Corrections
Maintain list of common typos:
- "Chracter" → "Character"
- "Documnetation" → "Documentation"
- "Complient" → "Compliant"
- "Deterministc" → "Deterministic"
- "Seperation" → "Separation"
- "Occured" → "Occurred"
- "Recieved" → "Received"

## 10.4 Uncertain Cases
If spelling is ambiguous or domain-specific:
- **Report**, don't auto-fix
- Flag as "Potential Typo" in audit report
- Let user decide

---

# 11. Routing Behavior
ForgeAudit is invoked when:

- The user requests an audit  
- The user requests validation  
- The user requests drift detection  
- The user requests compliance checks  
- The user requests rule verification  

Routing:
```
If task involves evaluation or drift detection → Use ForgeAudit
Otherwise → Delegate to appropriate branch
ForgeCharter always governs the process
```

---

# 12. Audit Workflow

## 12.1 Pre-Audit Phase
1. Identify scope (specific files or entire solution)
2. Load relevant branch rules
3. Initialize violation counters

## 12.2 Audit Phase (Read-Only for Content)
1. **Code Audit** - Detect violations, report only
2. **Architecture Audit** - Detect violations, report only
3. **Documentation Audit** - Detect violations (mixed mode)
4. **File Handling Audit** - Detect violations, report only

## 12.3 Auto-Fix Phase (Metadata Only)
1. **Character Counts** - Calculate and update all modified files
2. **Last Updated** - Update dates for modified files
3. **Missing Metadata** - Add fields with defaults
4. **Spelling Errors** - Fix typos in documentation
5. **IssueSummary.md** - Append new patterns or update frequencies

## 12.4 Report Phase
1. Generate compliance summary
2. List auto-fixes applied
3. List content violations (require user action)
4. Calculate compliance score
5. Provide recommendations

## 12.5 Post-Audit Phase
1. Output audit report
2. Confirm files modified (metadata only)
3. Confirm files requiring manual fixes
4. Update IssueSummary.md with audit metadata

---

# 13. Compliance Scoring

## 13.1 Scoring Formula
```
Total Rules = Rules from all branches applicable to audited files
Passing Rules = Rules with no violations
Auto-Fixed Rules = Rules violated but auto-corrected
Reported Rules = Rules violated requiring user action

Compliance Score = ((Passing + Auto-Fixed) / Total Rules) × 100%
```

## 13.2 Score Interpretation
- **95-100%** - Excellent (few violations, all minor)
- **85-94%** - Good (some violations, mostly auto-fixed)
- **70-84%** - Fair (multiple violations, user action needed)
- **Below 70%** - Poor (significant violations, major work required)

## 13.3 Score Reporting
Report compliance by category:
```markdown
| Category | Score | Status |
|----------|-------|--------|
| **Coding Branch** | 92% | ✅ GOOD |
| **Architecture Branch** | 88% | ✅ GOOD |
| **Documentation Branch** | 96% | ✅ EXCELLENT |
| **ForgeCharter** | 94% | ✅ EXCELLENT |
| **Overall** | **93%** | **✅ GOOD** |
```

---

# 14. Examples

## 14.1 Example: Metadata Auto-Fix

**Before Audit:**
```markdown
**Character Count:** 17764
**Last Updated:** 2025-01-01
```

**Audit Detects:**
- File was modified (added content)
- Character count outdated
- Last Updated date stale

**Auto-Fix Applied:**
```markdown
**Character Count:** 18664
**Last Updated:** 2025-01-02
```

**Audit Report:**
```
✅ Auto-fixed: Updated Character Count (17764 → 18664)
✅ Auto-fixed: Updated Last Updated (2025-01-01 → 2025-01-02)
```

---

## 14.2 Example: Content Violation (Report Only)

**Audit Detects:**
File `ModuleLoaderService.vb` has business logic in event handler.

**Violation Report:**
```markdown
❌ Violation: Business Logic in Event Handler
**Rule:** Branch-Coding Section 6.3 (Event Handler Orchestration)
**Severity:** MEDIUM
**File:** Source/Services/Implementations/ModuleLoaderService.vb
**Issue:** Event handler contains validation logic (should delegate to service method)
**Recommendation:** Extract validation logic to separate method, call from handler
```

**No Auto-Fix** - User must refactor code.

---

## 14.3 Example: IssueSummary.md Update

**Audit Finding:**
"Detected 2 new instances of AI/Terminal interaction hanging on git log command."

**Action:**
1. Check IssueSummary.md for "AI/Terminal" pattern
2. Pattern exists (Category #18)
3. Update frequency: "Low-Medium (encountered in 2 sessions)" → "Low-Medium (encountered in 4 sessions)"
4. Update Character Count
5. Update Last Updated

**Audit Report:**
```
✅ Updated IssueSummary.md: Category #18 frequency count (2 → 4 sessions)
```

---

## 14.4 Example: Spelling Auto-Fix

**Before Audit:**
```markdown
This document defines the chracter count rules for metadata headers.
All files must be complient with governance standards.
```

**Audit Detects:**
- "chracter" → spelling error
- "complient" → spelling error

**Auto-Fix Applied:**
```markdown
This document defines the character count rules for metadata headers.
All files must be compliant with governance standards.
```

**Audit Report:**
```
✅ Fixed spelling: "chracter" → "character" (line 12)
✅ Fixed spelling: "complient" → "compliant" (line 13)
```

---

# 15. Audit Certification

At the end of every audit, include:

```markdown
---

## ForgeAudit 2.0 Certification

✅ **Audit Mode:** Mixed (Auto-Fix Metadata + Report Content)  
✅ **Files Modified:** [count] (metadata corrections only)  
✅ **Auto-Fixes Applied:** [count]  
   - Character Counts: [count]
   - Last Updated Dates: [count]
   - Spelling Corrections: [count]
   - IssueSummary.md Updates: [count]
✅ **Content Violations Reported:** [count] (require user action)  
✅ **Compliance Score:** [score]%  
✅ **Audit Completed:** [date]  

**No code, structure, or authored content was modified.**  
**Only metadata, spelling, and IssueSummary.md were auto-corrected.**
```

---

# End of ForgeAudit 2.0
