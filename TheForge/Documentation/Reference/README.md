# TheForge Reference Hub

**Document Type:** Reference Index  
**Purpose:** Master index of all Spec-Kit and Forge resources  
**Created:** 2025-01-04  
**Last Updated:** 2025-01-04  
**Status:** Active  
**Character Count:** TBD  
**Related:** SPECKIT_SETUP.md, ForgeCharter.md, copilot-instructions.md

---

## ?? Quick Navigation

### Spec-Kit Commands
- `/speckit.constitution` - Define project principles
- `/speckit.specify` - Create specification
- `/speckit.plan` - Generate implementation plan
- `/speckit.tasks` - Break down tasks
- `/speckit.implement` - Execute implementation

### Forge Files
- **ForgeCharter.md** - `TheForge/Prompts/ForgeCharter.md`
- **Branch-Architecture.md** - `TheForge/Prompts/Branch-Architecture.md`
- **Branch-Coding.md** - `TheForge/Prompts/Branch-Coding.md`
- **Branch-Documentation.md** - `TheForge/Prompts/Branch-Documentation.md`
- **ForgeAudit.md** - `TheForge/Prompts/ForgeAudit.md`

### Spec-Kit Files
- **Agents** - `TheForge/Prompts/SpecKit/agents/*.agent.md` (9 files)
- **Prompts** - `TheForge/Prompts/SpecKit/prompts/*.prompt.md` (9 files)
- **Templates** - `TheForge/Prompts/SpecKit/templates/` (5 template files)
- **Scripts** - `TheForge/Prompts/SpecKit/scripts/powershell/` (5 PowerShell scripts)
- **Memory** - `TheForge/Prompts/SpecKit/memory/constitution.md`

---

## ?? Workflow Integration

```
Spec-Kit (Planning)     +     Forge (Governance)
      ?                              ?
  "What to build"            "How to build it"
      ?                              ?
         Compliant Implementation
```

**Tutorial:** `TheForge/Documentation/Technical/SpecKit-Tutorial-AddREADMEs.md`

---

## ?? File Locations

```
TheForge/Prompts/
??? ForgeCharter.md              (Master governance)
??? Branch-*.md                  (Specialized rules)
??? ForgeAudit.md                (Compliance tool)
??? SpecKit/
    ??? agents/                  (9 Spec-Kit agent files)
    ??? prompts/                 (9 Spec-Kit prompt files)
    ??? templates/               (5 template files)
    ??? scripts/powershell/      (5 PowerShell scripts)
    ??? memory/                  (constitution.md)
    ??? features/
        ??? [feature-name]/
            ??? spec.md          (Requirements)
            ??? plan.md          (Implementation plan)
            ??? tasks.md         (Task breakdown)

TheForge/Documentation/
??? Reference/                   (You are here)
??? Technical/                   (Tutorials)
```

---

**End of Reference Hub**
