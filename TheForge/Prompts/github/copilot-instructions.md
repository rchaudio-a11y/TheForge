# copilot-instructions  
**Document Type:** Router  
**Purpose:** Route all tasks to the correct Forge branch based on user intent, and enable Spec-Kit agents  
**Created:** 2026-01-02  
**Last Updated:** 2025-01-04  
**Status:** Final  
**Character Count:** TBD  
**Related:** TheForge/Prompts/ForgeCharter.md, .github/agents/speckit.*.agent.md

---

# 1. Purpose
This file acts as the routing layer for the RCH UI Forge and enables Spec-Kit workflow agents.  
It contains **no rules**.  
All rules live in ForgeCharter and the branch files.

---

# 2. Forge Routing Model
All tasks must be routed according to the governance defined in **ForgeCharter**.

**File Locations:**
- **ForgeCharter:** `TheForge/Prompts/ForgeCharter.md`
- **Branch-Coding:** `TheForge/Prompts/Branch-Coding.md`
- **Branch-Architecture:** `TheForge/Prompts/Branch-Architecture.md`
- **Branch-Documentation:** `TheForge/Prompts/Branch-Documentation.md`
- **ForgeAudit:** `TheForge/Prompts/ForgeAudit.md`

**Routing:**
- If the task involves **code** → Use `TheForge/Prompts/Branch-Coding.md`
- If the task involves **structure or design** → Use `TheForge/Prompts/Branch-Architecture.md`
- If the task involves **documentation** → Use `TheForge/Prompts/Branch-Documentation.md`
- If the task involves **evaluation or drift detection** → Use `TheForge/Prompts/ForgeAudit.md`

**ForgeCharter always governs the process:** `TheForge/Prompts/ForgeCharter.md`

---

# 3. Spec-Kit Agents (Enabled)

The following Spec-Kit workflow agents are available:

**Spec-Kit Agent Files:**
- `.github/agents/speckit.specify.agent.md` - Create specifications
- `.github/agents/speckit.plan.agent.md` - Generate implementation plans
- `.github/agents/speckit.tasks.agent.md` - Generate task breakdowns
- `.github/agents/speckit.implement.agent.md` - Execute tasks
- `.github/agents/speckit.clarify.agent.md` - Ask clarifying questions
- `.github/agents/speckit.analyze.agent.md` - Analyze consistency
- `.github/agents/speckit.checklist.agent.md` - Generate checklists
- `.github/agents/speckit.constitution.agent.md` - Project constitution
- `.github/agents/speckit.taskstoissues.agent.md` - Convert tasks to issues

**To use Spec-Kit agents, invoke them with:**
- `/speckit.specify` - Define feature requirements
- `/speckit.plan` - Create technical plan
- `/speckit.tasks` - Generate task breakdown
- `/speckit.implement` - Execute implementation
- `/speckit.clarify` - Ask questions
- `/speckit.analyze` - Check consistency
- `/speckit.checklist` - Generate checklists

**Spec-Kit workflow:**
```
specify → plan → tasks → implement
```

**Integration:** Spec-Kit handles planning, Forge handles governance.

---

# 4. Responsibilities
This router must:

- Load `TheForge/Prompts/ForgeCharter.md`
- Enable Spec-Kit agents from `.github/agents/`
- Identify the task type  
- Delegate to the correct branch file  
- Never interpret rules  
- Never override rules  
- Never duplicate rules  
- Never merge branch responsibilities  
- Never perform work itself  

---

# 5. Non-Responsibilities
This router must **not**:

- Contain rules  
- Modify files  
- Generate files  
- Evaluate work  
- Perform audits  
- Apply naming canon  
- Apply layout rules  
- Apply architecture rules  
- Apply documentation rules  

All rule enforcement occurs in the branches and ForgeCharter.

---

# 6. Drift Prevention
If routing is ambiguous:

1. Ask the user to clarify the task  
2. Do not guess  
3. Do not route incorrectly  
4. Do not perform any work until routing is clear  

---

# 7. File Reference Quick Guide

**Governance:**
- `TheForge/Prompts/ForgeCharter.md` - Master governance

**Branch Files:**
- `TheForge/Prompts/Branch-Coding.md` - Code generation rules
- `TheForge/Prompts/Branch-Architecture.md` - Project structure rules
- `TheForge/Prompts/Branch-Documentation.md` - Documentation rules

**Audit:**
- `TheForge/Prompts/ForgeAudit.md` - Compliance evaluation

**Spec-Kit Agents:**
- `.github/agents/` directory - All Spec-Kit workflow agents

**All Forge files located in:** `TheForge/Prompts/` directory  
**All Spec-Kit agents located in:** `.github/agents/` directory

---

# End of copilot-instructions.md
