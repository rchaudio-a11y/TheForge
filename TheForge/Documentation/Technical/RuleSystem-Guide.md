# How to Use The Forge Rule System

**Document Type:** Technical Reference  
**Created:** 2025-01-02  
**Character Count:** 11167  
**Status:** Final  
**Purpose:** Guide for understanding and using The Forge rule hierarchy  
**Related:** CodeRules-Summary.md, ArchitectureRules-Summary.md, DocumentationRules-Summary.md

---

## Overview

The Forge uses a **tiered rule system** with quick-reference summaries and detailed source files. This guide explains when to use each tier for maximum efficiency.

---

## Rule System Structure

### **Two Types of Rules:**

1. **Summary Files** (Quick Reference)
   - Optimized for speed and action
   - 90% of daily tasks covered
   - Include checklists and decision trees
   - ~25k characters total

2. **Source Files** (Complete Reference)
   - Optimized for completeness and context
   - 100% coverage with rationale
   - Authoritative for edge cases
   - ~60k+ characters total

---

## Tier 1: Quick Reference Summaries (Check First)

**Use these for daily coding tasks.**

### 1. CodeRules-Summary.md
**When to use:**
- Writing or modifying code
- Designer file questions
- Layout decisions (Anchor vs Dock)
- Error handling patterns
- Terminal command usage

**Key sections:**
- UI & Services architecture
- Designer file handling (THE GOLDEN RULE)
- Layout strategy (Anchor > Dock > Absolute)
- Quick reference checklists

**Character count:** 7,845

---

### 2. ArchitectureRules-Summary.md
**When to use:**
- Project structure decisions
- Folder placement questions
- Naming conventions
- Architectural patterns
- Module design

**Key sections:**
- 10 Prime Directives
- Standard folder structure
- Layered architecture
- Naming conventions
- Quality checklists

**Character count:** 6,892

---

### 3. DocumentationRules-Summary.md
**When to use:**
- Creating documentation
- Choosing doc folder (Codex, Chronicle, Tomes, etc.)
- File size checks
- Multi-file splitting
- Metadata headers

**Key sections:**
- 6-folder taxonomy
- File size rules (10k limit)
- Pre-generation checklist
- Multi-file patterns
- Compliance workflows

**Character count:** 9,987

---

## Tier 2: Detailed Source Files (If Summaries Unclear)

**Use these for edge cases and deep context.**

### 1. file8.md - Code Implementation
**When summaries don't cover:**
- Complex Designer file scenarios
- Unusual layout requirements
- Terminal command edge cases
- Plugin/reflection details

**Character count:** ~12k

**Link from summary:** CodeRules-Summary.md ? file8.md

---

### 2. File6.md - Prime Directives
**When summaries don't cover:**
- Philosophical questions
- "Why" behind architectural decisions
- Forge ethos details

**Character count:** ~2k

**Link from summary:** ArchitectureRules-Summary.md ? File6.md

---

### 3. file4.md - Documentation Taxonomy
**When summaries don't cover:**
- Unusual documentation scenarios
- Taxonomy edge cases
- README requirements details

**Character count:** ~2k

**Link from summary:** DocumentationRules-Summary.md ? file4.md

---

### 4. file7.md - Documentation Generation
**When summaries don't cover:**
- Complex multi-file scenarios
- Exception approval processes
- Milestone doc edge cases

**Character count:** ~5k

**Link from summary:** DocumentationRules-Summary.md ? file7.md

---

### 5. file2.md - Project Layout
**When summaries don't cover:**
- Unusual folder structure needs
- New folder creation decisions

**Character count:** ~2k

**Link from summary:** ArchitectureRules-Summary.md ? file2.md

---

## Tier 3: Governance (If Conflicts Arise)

**Use these to resolve rule conflicts.**

### 1. Master.md
- Canonical governance index
- Rule precedence authority
- Owner and file location definitions

### 2. ForgeOrchestrator.md
- Orchestration rules
- Execution behavior
- Task delegation

**When to consult:**
- Rules from multiple files conflict
- Precedence unclear
- Need authoritative resolution

---

## Efficiency Comparison

| Aspect | Summaries | Source Files |
|--------|-----------|--------------|
| **Token Count** | ~25k chars | ~60k+ chars |
| **Read Time** | 5-10 seconds | 15-30 seconds |
| **Coverage** | 90% of tasks | 100% of tasks |
| **Format** | Checklists, tables | Detailed prose |
| **Best For** | Quick decisions | Deep understanding |
| **Updates** | Must regenerate | Source of truth |

---

## Workflow Guide

### **For Most Questions: Tier 1 (Summaries)**

```
User Question
    ?
Check appropriate summary (5 seconds)
    ?
Found clear answer?
    ?? YES ? Respond with answer
    ?? NO ? Go to Tier 2
```

**Example:**
- **Q:** "Should I use Anchor or Dock for a UserControl?"
- **A:** Check CodeRules-Summary.md Section 7.5 ? "Use Anchor (PRIMARY)"
- **Time:** 5 seconds

---

### **For Edge Cases: Tier 2 (Source Files)**

```
User Question
    ?
Check summary (ambiguous)
    ?
Check relevant source file (15 seconds)
    ?
Found detailed answer?
    ?? YES ? Respond with answer + source reference
    ?? NO ? Go to Tier 3 or ask user
```

**Example:**
- **Q:** "Can I edit Designer.vb if Designer is closed but file is open in code view?"
- **A:** Summary unclear ? Check file8.md Section 7.2 ? "Yes, can use edit_file when Designer view closed"
- **Time:** 15 seconds

---

### **For Conflicts: Tier 3 (Governance)**

```
User Question
    ?
Check summaries (conflicting info)
    ?
Check source files (still conflicts)
    ?
Check Master.md / ForgeOrchestrator.md
    ?
Found precedence rule?
    ?? YES ? Apply higher precedence rule
    ?? NO ? Ask user for clarification
```

**Example:**
- **Q:** "file7.md says X but file8.md says Y, which is correct?"
- **A:** Check Master.md ? file7.md (precedence 5) < file8.md (precedence 6) ? file7.md wins
- **Time:** 20 seconds

---

## Common Scenarios

### Scenario 1: Writing Code
**Question Type:** "How should I structure this event handler?"

**Workflow:**
1. ? CodeRules-Summary.md Section 2 (UI & Services)
2. Answer: "Event handlers = orchestration only"
3. Result: Fast answer from summary

---

### Scenario 2: Creating Documentation
**Question Type:** "Where does a tutorial go?"

**Workflow:**
1. ? DocumentationRules-Summary.md Decision Tree
2. Answer: "Tutorial ? Tomes folder"
3. Result: Fast answer from decision tree

---

### Scenario 3: Designer File Edge Case
**Question Type:** "Can I modify Designer.vb while VS is running if Designer view is closed?"

**Workflow:**
1. Check CodeRules-Summary.md Section 7.1-7.3 (ambiguous)
2. Check file8.md Section 7.2 (detailed answer)
3. Answer: "Yes, CAN be modified when Designer is closed"
4. Result: Needed source file for edge case

---

### Scenario 4: Rule Conflict
**Question Type:** "File X says A, File Y says B, which is correct?"

**Workflow:**
1. Check Master.md for precedence
2. Apply higher-precedence rule
3. Answer with precedence explanation
4. Result: Governance file resolved conflict

---

## Quick Reference: Which File for What?

### Code Questions
| Question Type | Check First | If Unclear, Check |
|---------------|-------------|-------------------|
| Layout (Anchor/Dock) | CodeRules-Summary § 7.5 | file8.md § 7.5 |
| Designer files | CodeRules-Summary § 7 | file8.md § 7 |
| Event handlers | CodeRules-Summary § 2 | file8.md § 2 |
| Error handling | CodeRules-Summary § 6 | file8.md § 6 |

### Architecture Questions
| Question Type | Check First | If Unclear, Check |
|---------------|-------------|-------------------|
| Folder structure | ArchitectureRules-Summary Part 2 | file2.md |
| Naming conventions | ArchitectureRules-Summary § Naming | File6.md § 4 |
| Layered architecture | ArchitectureRules-Summary § Patterns | File6.md § 5 |
| Prime Directives | ArchitectureRules-Summary Part 1 | File6.md |

### Documentation Questions
| Question Type | Check First | If Unclear, Check |
|---------------|-------------|-------------------|
| Which folder? | DocumentationRules-Summary Decision Tree | file4.md |
| File size limits | DocumentationRules-Summary § File Size | file7.md § 1-2 |
| Multi-file split | DocumentationRules-Summary § Multi-file | file7.md § 4 |
| Metadata headers | DocumentationRules-Summary § Headers | file7.md § 3 |

---

## Best Practices

### ? Do This
- Start with summaries for 90% of questions
- Use decision trees and checklists
- Check source files for edge cases
- Reference both summary and source when answering
- Ask user if truly ambiguous

### ? Don't Do This
- Read all source files for every question
- Guess when rules are unclear
- Skip summaries and go straight to source
- Ignore precedence when conflicts arise
- Assume summaries cover 100% of cases

---

## For AI Instances

### Loading Priority

**On initialization, load in memory:**
1. CodeRules-Summary.md (most frequently used)
2. ArchitectureRules-Summary.md (frequently used)
3. DocumentationRules-Summary.md (frequently used)

**Load on-demand:**
- Source files (file1-8, Master.md, ForgeOrchestrator.md)
- Only when summaries insufficient

**Benefits:**
- 2.4x less token usage for common tasks
- Faster response times
- Better UX
- Still 100% accurate (source files available)

---

## Maintenance Notes

### When Summaries Need Updates

**Trigger events:**
- Source files (file1-8) are modified
- New rules added to Prime Directives
- Layout patterns change
- New documentation folders added

**Update process:**
1. Modify source file (file1-8)
2. Regenerate affected summary
3. Verify character count <10k
4. Update "Last Updated" date
5. Run build to verify

**Frequency:**
- Source files: As needed (source of truth)
- Summaries: After source changes (derived)

---

## Summary Decision Matrix

| If User Asks... | Start With... | Fallback To... |
|-----------------|---------------|----------------|
| "How do I...?" | Summary | Source |
| "Where does X go?" | Summary (decision tree) | Source |
| "Can I...?" | Summary | Source |
| "Why does...?" | Source (rationale) | Governance |
| "What if X conflicts with Y?" | Governance | Ask user |
| "Is there an exception for...?" | Source | Ask user |

---

## Conclusion

**The Forge rule system provides:**

1. **Speed** - Summaries for 90% of tasks (5 seconds)
2. **Completeness** - Source files for edge cases (15 seconds)
3. **Authority** - Governance for conflicts (20 seconds)
4. **Efficiency** - 2.4x less token usage for common tasks
5. **Accuracy** - 100% coverage when needed

**Recommended workflow:**
```
Summary First (Tier 1) ? Source if Needed (Tier 2) ? Governance if Conflict (Tier 3)
```

**Result:** Fast, accurate, complete answers! ??

---

**Related Documentation:**
- CodeRules-Summary.md (Tier 1)
- ArchitectureRules-Summary.md (Tier 1)
- DocumentationRules-Summary.md (Tier 1)
- Prompts/file1-8.md (Tier 2)
- Prompts/Master.md (Tier 3)
- Prompts/ForgeOrchestrator.md (Tier 3)
