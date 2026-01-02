# file8.md — Code Behavior & Implementation Extensions

This file extends the Forge rule stack for code generation and modification. It does not restate core language, architecture, or layout rules already defined in Master.md or file1–file6; it only clarifies how Copilot should behave when implementing them.

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

End of file8.md.