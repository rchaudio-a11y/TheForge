### \# Repository-wide Copilot Instructions



This repository uses the Forge rule stack and deterministic engineering standards.

Copilot must follow all rules below without deviation.



----------------------------------------------------------------------

Authoritative Rule Sources

----------------------------------------------------------------------



Copilot must treat the following documents as authoritative and binding:



1\. Prompts/ForgeOrchestrator.md  

&nbsp;  - Defines rule hierarchy, file scope boundaries, orchestration behavior,

&nbsp;    and how Copilot must interpret tasks.



2\. Documentation/Master.md (or equivalent master index)

&nbsp;  - Defines the full rule map and where each rule lives.



3\. Forge rule stack (file1.md through file6.md)

&nbsp;  - Defines architectural, structural, naming, documentation, and governance rules.



Copilot must not override, reinterpret, or restate rules already defined in these files.



----------------------------------------------------------------------

Language \& Project Standards

----------------------------------------------------------------------



• All code must be written in VB.NET.  

• Option Strict On, Option Explicit On, Option Infer Off.  

• Do not modify project structure or delete files unless explicitly instructed.  



----------------------------------------------------------------------

WinForms Standards

----------------------------------------------------------------------



• Use deterministic layout only:

&nbsp; - TableLayoutPanel

&nbsp; - SplitContainer

&nbsp; - Dock



• Do not use anchoring.



• Do not create default names (e.g., Form1, Button1).  

&nbsp; Always use explicit, descriptive names.



• UI layer must remain thin:

&nbsp; - No business logic in designer files.

&nbsp; - No business logic in event handlers beyond routing to services.



----------------------------------------------------------------------

File Size \& Multi-File Output Rules (Non‑Negotiable)

----------------------------------------------------------------------



• No generated or modified file may exceed \*\*10,000 characters\*\*.



• If output would exceed this limit, Copilot must automatically split the content

&nbsp; into sequentially numbered files using this pattern:



&nbsp;   \[BaseName]\_01.md  

&nbsp;   \[BaseName]\_02.md  

&nbsp;   \[BaseName]\_03.md  

&nbsp;   ...continue until complete.



• Splitting must occur at logical section boundaries.  

• No content may be truncated.  

• This rule applies to:

&nbsp; - Documentation

&nbsp; - Chronicle entries

&nbsp; - Audit reports

&nbsp; - Prompt files

&nbsp; - Generated code

&nbsp; - Any multi-part output



• This rule has \*\*no exceptions\*\* unless explicitly stated in the Forge rule stack.



----------------------------------------------------------------------

Milestone Documentation Rules

----------------------------------------------------------------------



For every milestone (vXXX):



• Create a Chronicle entry in `/Documentation/Chronicle` named:

&nbsp;   vXXX\_entry.md



• Each milestone entry must include:

&nbsp; - Description

&nbsp; - What It Does

&nbsp; - Issues Encountered

&nbsp; - Development Patterns

&nbsp; - Build Status

&nbsp; - Any rule drift or architectural changes



• Update `DevelopmentLog.index.md` to reference the new milestone.



• Update `IssueSummary.md` when new recurring patterns or violations appear.



• Update `VersionHistory.chronicle.md` unless explicitly deprecated.



• If a milestone entry exceeds 10,000 characters, split it using the multi-file rule.



----------------------------------------------------------------------

Documentation \& Folder Standards

----------------------------------------------------------------------



• Every folder containing code must include a README.md describing:

&nbsp; - Purpose

&nbsp; - Rules

&nbsp; - Examples

&nbsp; - Architectural notes



• Documentation must never contradict the code.  

• Documentation must be updated with every change.



----------------------------------------------------------------------

General Behavior Rules

----------------------------------------------------------------------



• Follow file scope boundaries defined in ForgeOrchestrator.md.  

• Follow naming, structure, and architectural rules from file1–file6.  

• Prefer separation of UI, services, and models.  

• No unused code, commented-out code, or TODOs without owners.  

• No temporary files (\_old, \_temp, \_vXXX) unless explicitly requested.  

• All public APIs must include XML documentation comments.



----------------------------------------------------------------------

End of Instructions

----------------------------------------------------------------------



Copilot must follow all rules above for every task, file, and milestone.

