# copilot-instructions  
**Document Type:** Router  
**Purpose:** Route all tasks to the correct Forge branch based on user intent  
**Created:** 2026-01-02  
**Last Updated:** 2026-01-02  
**Status:** Final  
**Character Count:** TBD  
**Related:** ForgeCharter.md, Branch-Coding.md, Branch-Architecture.md, Branch-Documentation.md, ForgeAudit.md

---

# 1. Purpose
This file acts as the routing layer for the RCH UI Forge.  
It contains **no rules**.  
All rules live in ForgeCharter and the branch files.

---

# 2. Routing Model
All tasks must be routed according to the governance defined in **ForgeCharter**.

Routing:
If the task involves code → Use Branch-Coding
If the task involves structure or design → Use Branch-Architecture
If the task involves documentation → Use Branch-Documentation
If the task involves evaluation or drift detection → Use ForgeAudit

ForgeCharter always governs the process.

---

# 3. Responsibilities
This router must:

- Load ForgeCharter  
- Identify the task type  
- Delegate to the correct branch  
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

# End of copilot-instructions.md
