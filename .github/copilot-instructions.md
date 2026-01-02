# Repository-wide Copilot Instructions

This repository uses the Forge rule stack. Copilot must treat the listed documents as authoritative and must not restate or redefine rules already present in them.

----------------------------------------------------------------------
Authoritative governance sources
----------------------------------------------------------------------

Copilot must resolve behavior and rules in this order:

1. Documentation/Master.md  
   - Canonical index of all rules, owners, and file locations.

2. Prompts/ForgeOrchestrator.md  
   - Defines rule hierarchy, file scope boundaries, orchestration behavior,
     and how Copilot should interpret tasks and delegate across files.

3. Forge rule stack:
   - file1.md through file6.md
   - file7.md (documentation & large-output extensions)
   - file8.md (code behavior & implementation extensions)

Copilot must:
- Prefer these files over any assumptions or defaults.
- Avoid rephrasing or duplicating rules from these documents.
- Ask the user for clarification if instructions appear to conflict.

----------------------------------------------------------------------
Instruction routing
----------------------------------------------------------------------

When generating or modifying code:
- Consult Master.md, ForgeOrchestrator.md, file1–file6, and file8.md.
- Treat file8.md as an extension/clarification when core rules are ambiguous.

When generating documentation or large text outputs:
- Consult Master.md, ForgeOrchestrator.md, file1–file6, and file7.md.
- Treat file7.md as an extension/clarification for file size, multi-file output,
  and milestone documentation behavior.

----------------------------------------------------------------------
Behavior constraints
----------------------------------------------------------------------

• Do not invent new rules or governance concepts.  
• Do not override, weaken, or reinterpret rules defined in the MD files above.  
• If user instructions conflict with the Forge rule stack, request clarification
  instead of silently ignoring the rules.


