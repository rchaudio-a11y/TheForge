# copilot-instructions  
**Document Type:** Router  
**Purpose:** Route all tasks to the correct Forge branch based on user intent  
**Created:** 2026-01-02  
**Last Updated:** 2026-01-02  
**Status:** Final  
**Character Count:** 2879  
**Related:** TheForge/Prompts/ForgeCharter.md, TheForge/Prompts/Branch-Coding.md, TheForge/Prompts/Branch-Architecture.md, TheForge/Prompts/Branch-Documentation.md, TheForge/Prompts/ForgeAudit.md

---

# 1. Purpose
This file acts as the routing layer for the RCH UI Forge.  
It contains **no rules**.  
All rules live in ForgeCharter and the branch files.

---

# 2. Routing Model
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

# 3. Responsibilities
This router must:

- Load `TheForge/Prompts/ForgeCharter.md`
- Identify the task type  
- Delegate to the correct branch file  
- Never interpret rules  
- Never override rules  
- Never duplicate rules  
- Never merge branch responsibilities  
- Never perform work itself  

---

# 4. Non-Responsibilities
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

# 5. Drift Prevention
If routing is ambiguous:

1. Ask the user to clarify the task  
2. Do not guess  
3. Do not route incorrectly  
4. Do not perform any work until routing is clear  

---

# 6. File Reference Quick Guide

**Governance:**
- `TheForge/Prompts/ForgeCharter.md` - Master governance

**Branch Files:**
- `TheForge/Prompts/Branch-Coding.md` - Code generation rules
- `TheForge/Prompts/Branch-Architecture.md` - Project structure rules
- `TheForge/Prompts/Branch-Documentation.md` - Documentation rules

**Audit:**
- `TheForge/Prompts/ForgeAudit.md` - Compliance evaluation

**All files located in:** `TheForge/Prompts/` directory

---

# End of copilot-instructions.md
