# RCH UI Forge — Agent Selection Guide
**Document Type:** Codex  
**Purpose:** Define which Forge Agent to use for which task  
**Last Updated:** 2025-12-31  

---

## Overview
This guide explains which AI agent to use for specific tasks within the RCH UI Forge ecosystem.  
Each agent has a unique specialty and should be invoked accordingly.

---

## Agents

### 1. Codex Agent (GPT‑5.1‑Codex‑Max)
**Specialty:** Code generation, project structure, deterministic manifests

Use this agent for:
- Editing `.sln` or `.slnx` files  
- Creating or modifying VB.NET classes  
- Generating modular services or UI components  
- Building multi‑project solution structures  
- Ensuring deterministic ordering, GUID integrity, and layout  
- Producing code that must compile on first run  

**Prompt to use:**  
“Use the Codex Agent.”

---

### 2. Lore Agent (Claude Opus 4.5)
**Specialty:** Architecture, documentation, naming canon, onboarding clarity

Use this agent for:
- Architectural reviews  
- Designing modular systems  
- Documentation drift detection  
- Writing or refining documentation  
- Naming canon decisions  
- Evaluating trade‑offs, risks, and design alternatives  
- Ensuring philosophical alignment with file6.md  

**Prompt to use:**  
“Use the Lore Agent.”

---

## Quick Reference Table

| Task Type                          | Recommended Agent |
|-----------------------------------|-------------------|
| `.slnx` editing                    | Codex Agent       |
| VB.NET class generation            | Codex Agent       |
| Project folder structure           | Codex Agent       |
| Architecture review                | Lore Agent        |
| Documentation updates              | Lore Agent        |
| Naming canon decisions             | Lore Agent        |
| Modularity planning                | Lore Agent        |
| Multi‑project solution setup       | Codex Agent       |
| Drift detection                    | Lore Agent        |

---

## Summary
Use the **Codex Agent** for deterministic code and structure.  
Use the **Lore Agent** for architecture, documentation, and philosophy.

Together, they maintain the integrity of the Forge ecosystem.