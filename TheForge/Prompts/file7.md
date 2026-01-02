# file7.md — Documentation & Large-Output Extensions

This file extends the Forge rule stack for documentation and large text generation. It does not restate taxonomy or high-level documentation rules defined in Master.md or file1–file6; it only adds concrete behavioral constraints for Copilot.

----------------------------------------------------------------------
1. File size behavior for generated text
----------------------------------------------------------------------

These rules apply whenever Copilot generates or modifies documentation, reports, or other text files.

• Hard limit:
  - Copilot must treat 10,000 characters as the maximum safe size for any single generated file.

• If expected output would exceed this limit:
  - Split the content into multiple files.
  - Use a simple, sequential naming convention agreed upon in the prompt or by the user.
  - If no naming convention is given, ask the user which pattern to use instead of assuming.

• Copilot must never:
  - Truncate content to “fit” the limit.
  - Quietly omit sections without telling the user.

----------------------------------------------------------------------
2. Multi-file generation behavior
----------------------------------------------------------------------

When splitting documentation across multiple files:

• Preserve structure:
  - Do not break in the middle of a sentence or list.
  - Prefer breaking at heading or section boundaries.

• Maintain continuity:
  - Ensure headings, numbering, and cross-references make sense across parts.
  - Clearly indicate when content “continues in the next file” if the user asks for that style.

• Defer naming decisions:
  - If the repository or user specifies a naming pattern, follow it.
  - If not, ask rather than inventing a pattern that might conflict with existing conventions.

----------------------------------------------------------------------
3. Milestone documentation behavior
----------------------------------------------------------------------

These extensions assume the existence of Chronicle and related docs as described in Master.md and file2/file4; they only add behavior for Copilot.

When the user requests milestone documentation (e.g., “create v0.9.2 entry”):

• Copilot should:
  - Create or update the appropriate Chronicle entry for that milestone.
  - Ensure the entry includes the standard milestone fields defined in existing Chronicle patterns.
  - Suggest updates to DevelopmentLog.index.md, IssueSummary.md, or VersionHistory.chronicle.md when new patterns, issues, or rule drift are documented.

• If the milestone doc risks exceeding the file size behavior above:
  - Propose splitting into multiple parts.
  - Explain how the parts should be named and linked.

----------------------------------------------------------------------
4. Documentation-code consistency behavior
----------------------------------------------------------------------

When there is tension between documentation and code:

• Copilot must:
  - Assume code is the current source of truth.
  - Treat outdated documentation as a candidate for revision, not enforcement.
  - Prefer aligning documentation to the behavior and architecture currently expressed in code.

• If documentation appears to describe an older architecture or rule set:
  - Note the discrepancy.
  - Propose updating the documentation to match current implementation.

End of file7.md.