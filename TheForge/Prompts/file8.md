# file8.md — Code Behavior & Implementation Extensions

This file extends the Forge rule stack for code generation and modification. It does not restate core language, architecture, or layout rules already defined in Master.md or file1–file6; it only clarifies how Copilot should behave when implementing them.

----------------------------------------------------------------------
0. Rule precedence (file8.md scope)
----------------------------------------------------------------------

This file operates at the lowest precedence in the Forge rule stack:

Precedence order (highest to lowest):
1. Master.md — Canonical governance index
2. ForgeOrchestrator.md — Orchestration and execution rules
3. File6.md — Prime Directives (architectural principles)
4. file1–file5 — Core domain rules (Scriptorium, Layout, Taxonomy, Config)
5. file7.md — Documentation extensions (file size, multi-file output)
6. file8.md — Code implementation clarifications (this file)

Conflict resolution:
• If file8.md contradicts file1–file6: file1–file6 wins
• If file8.md contradicts File6.md Prime Directives: File6.md wins
• file8.md clarifies *how* to implement rules, not *what* the rules are
• At same precedence level: more specific beats more general

When in doubt: Ask user for clarification per section 1 below.

----------------------------------------------------------------------
1. When rules appear ambiguous
----------------------------------------------------------------------

If multiple MD files apply and their guidance seems ambiguous:

• Copilot must:
  - Prefer the most specific rule (narrowest scope) over generic guidance.
  - Prefer Master.md and ForgeOrchestrator.md for resolving precedence.
  - Ask the user for clarification rather than guessing when in doubt.

----------------------------------------------------------------------
2. UI, services, and business logic behavior
----------------------------------------------------------------------

When generating or modifying code in the UI layer:

• Copilot must:
  - Route non-trivial logic into services or dedicated classes, consistent with existing patterns.
  - Keep event handlers as orchestration only: parameter gathering, calling services, updating UI.

When generating or modifying services:

• Copilot should:
  - Prefer pure, stateless services where practical.
  - Make state explicit when it must exist (e.g., caches, in-memory collections), following the style in current services.

----------------------------------------------------------------------
3. Plugin and reflection-based behavior
----------------------------------------------------------------------

When dealing with dynamically loaded modules:

• Copilot must:
  - Respect the established plugin pattern in the solution (e.g., reflection-based loading).
  - Use Activator.CreateInstance or equivalent only where the existing architecture already does so.
  - Ensure that any required services or configuration are injected or provided via clearly defined initialization methods after instantiation, not hidden globals.

----------------------------------------------------------------------
4. Performance and logging behavior
----------------------------------------------------------------------

When introducing new operations that might be expensive or performance-sensitive:

• Copilot should:
  - Follow existing caching and timing patterns present in the codebase.
  - Add diagnostic logging only where it aligns with existing logging conventions.
  - Avoid premature optimization that introduces complexity without clear benefit.

----------------------------------------------------------------------
5. Naming and structure behavior
----------------------------------------------------------------------

When introducing new types, methods, or members:

• Copilot must:
  - Mirror existing naming patterns and suffixes/prefixes already in use in the same folder or layer.
  - Avoid introducing new naming schemes without a clear reason grounded in existing docs (Master, NamingCanon, etc.).

When creating new files:

• Copilot should:
  - Place them in the folder that matches existing architectural patterns (UI, Services, Models, etc.).
  - Ask the user before creating new top-level folders or structural changes.

----------------------------------------------------------------------
6. Terminal command and error handling behavior
----------------------------------------------------------------------

When executing terminal commands (PowerShell, bash, cmd, etc.):

• Copilot must:
  - NEVER use -ErrorAction SilentlyContinue or equivalent silent error suppression
  - Allow errors to surface explicitly so failures are visible
  - Prefer -ErrorAction Stop for critical operations that must not fail silently
  - Report command failures clearly to the user

• Copilot should:
  - Validate file/path existence before destructive operations when practical
  - Use appropriate tools (edit_file, create_file, remove_file) instead of terminal commands for file modifications when available
  - Provide clear context when terminal command execution fails
  - Follow existing error handling patterns in the codebase

Rationale:
Silent error suppression violates the Forge principle of explicit over implicit behavior.
Errors must be visible so they can be addressed, not hidden.
## 7. Designer File Handling

This section defines required behavior when generating, modifying, or interacting with
files managed by the Visual Studio Windows Forms Designer. These rules ensure that
Designer‑generated code remains stable, synchronized, and free from corruption.

### 7.1 Scope
These rules apply to:
- Any `.Designer.vb` file
- Any `.vb` file paired with a Designer file through `Partial Class`
- Any UserControl or Form using `InitializeComponent`
- Any project file entries involving `<SubType>UserControl</SubType>` or `<DependentUpon>`

### 7.2 File Lock Awareness
- Assume Designer‑managed files may be locked by Visual Studio at any time.
- If a file is locked, do not attempt shell‑based modification, deletion, or replacement.
- When a lock is detected (e.g., “Could not get text view”), stop immediately and surface
  the error instead of retrying or falling back to shell commands.

### 7.3 Modification Rules
- Do not modify `.Designer.vb` files using shell commands under any circumstances.
- Do not modify Designer‑paired `.vb` files using shell commands if Visual Studio is open.
- All modifications must occur through the Visual Studio text buffer when available.
- If Designer is open for a file, do not attempt automated edits; require the user to close
  the Designer before proceeding.

### 7.4 Partial Class Consistency
- When generating or modifying a UserControl or Form, ensure that:
  - Both the main `.vb` file and `.Designer.vb` file declare `Partial Class` with identical names.
  - The `DesignerGenerated` attribute is preserved where required.
- Never introduce mismatched class declarations that would desynchronize Designer state.

### 7.5 Project File Integrity
- When creating or restructuring Designer‑managed files:
  - Ensure the `.Designer.vb` file is marked with `<DependentUpon>` in the project file.
  - Ensure the main `.vb` file includes `<SubType>UserControl</SubType>` or `<SubType>Form</SubType>` as appropriate.
- Do not create temporary files inside the project directory that may be auto‑imported by Visual Studio.

### 7.6 External Change Handling
- If a Designer‑managed file must be edited externally, require Visual Studio to be closed first.
- After external edits, Visual Studio must be allowed to reload the file and resynchronize Designer state.
- Never bypass Visual Studio’s reload mechanism.

### 7.7 Error Handling
- Do not suppress errors when interacting with Designer‑managed files.
- Surface locking, synchronization, or project‑structure errors explicitly.
- Do not attempt fallback operations that could corrupt Designer state.

### 7.8 Automation Limitations
- Do not attempt to automate Designer interactions (open/close Designer, reload Designer, etc.).
- Do not attempt to modify Designer‑generated code structures beyond what Visual Studio itself produces.
- Respect that Designer files are authoritative for layout and component initialization.

End of file8.md.